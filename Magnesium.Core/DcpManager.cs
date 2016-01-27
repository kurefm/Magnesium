using Magnesium.Core.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Collections;
using Magnesium.Core.Log;
using System.Threading;

namespace Magnesium.Core
{
    /// <summary>
    /// DCP管理器，加载与管理DCP
    /// </summary>
    internal class DcpManager
    {
        private Dictionary<Guid, IDataCollectProvider> _dcpList;



        public List<RunningDcpInfo> AutoRunningList { get; } = new List<RunningDcpInfo>();

        public DcpManager()
        {
            _dcpList = new Dictionary<Guid, IDataCollectProvider>();
        }

        #region DCP加载相关
        /// <summary>
        /// 注册一个DCP对象
        /// </summary>
        /// <param name="dcp"></param>
        public void RegisterDcp(IDataCollectProvider dcp)
        {
            if (_dcpList.ContainsKey(dcp.GUID))
            {
                throw new DcpGuidAlreadyExistsException($"当前列表中已存在GUID为: {dcp.GUID}的DCP, 为了避免调用异常，请修改该DCP的GUID");
            }
            _dcpList.Add(dcp.GUID, dcp);
        }

        /// <summary>
        /// 注册一个路径中的所有可能的DCP对象
        /// </summary>
        /// <param name="path">路径</param>
        public void RegisterDcp(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            var attr = File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                // path为目录
                GetDcpFromDir(path).ForEach(dcp => RegisterDcp(dcp));
            }
            else
            {
                //path为文件
                GetDcpFromFile(path).ForEach(dcp => RegisterDcp(dcp));
            }
        }

        /// <summary>
        /// 从指定的文件中加载DCP对象
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public IEnumerable<IDataCollectProvider> GetDcpFromFile(string filePath)
        {
            //TODO [待完善]加载指定路径的dll, 需要处理异常

            Assembly dcpAsb = null;
            //从指定路径加载程序集
            try
            {
                Logger.Info($"Try load DCP from {filePath}");
                dcpAsb = Assembly.LoadFrom(filePath);
            }
            catch (BadImageFormatException)
            {
                Logger.Error($"File {filePath} not a assembly");
                return new IDataCollectProvider[0];
            }

            //生成实现IDataCollectProvider接口的类的实例
            var dcps = from type in dcpAsb.GetTypes()
                       where type.GetInterfaces().Contains(typeof(IDataCollectProvider))
                       select (IDataCollectProvider)Activator.CreateInstance(type);

            return dcps;
        }

        /// <summary>
        /// 从指定的目录中加载DCP对象
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <param name="searchPattern">文件搜索字符串，默认为*.dll</param>
        /// <returns></returns>
        public IEnumerable<IDataCollectProvider> GetDcpFromDir(string dirPath, string searchPattern = "*.dll")
        {
            //TODO 从指定目录加载DCP
            //获取下的所有dll, 为了节省加载时间, 仅加载dll文件
            Logger.Info($"Search dll from {dirPath}");
            var dllPaths = Directory.GetFiles(dirPath, searchPattern, SearchOption.AllDirectories);

            //分别加载各个dll中的DCP
            var dcps = from dllPath in dllPaths                 //遍历DCP文件路径
                       from dcp in GetDcpFromFile(dllPath)      //遍历DCP文件中的DCP对象
                       select dcp;
            return dcps;
        }

        #endregion


        #region 处理函数相关
        /// <summary>
        /// 请求队列处理程序
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="parseQueue"></param>
        public void RequestHandler(DataRequest dr, ActionBlock<DataResponse> parseQueue)
        {
            //TODO [待完善]异常处理
            var dcp = _dcpList[dr.GUID];
            var dresp = dcp.SendHandler(dr);
            parseQueue.Post(dresp);
        }

        /// <summary>
        /// 解析队列处理程序
        /// </summary>
        /// <param name="dresp"></param>
        /// <param name="storeQueue"></param>
        public void ParseHandler(DataResponse dresp, ActionBlock<DataContainer> storeQueue)
        {
            //TODO [待完善]异常处理
            var dcp = _dcpList[dresp.GUID];
            DataContainer dc;
            try
            {
                dc = dcp.ParseHandler(dresp);
            }
            catch (ParseErrorException)
            {
                //TODO [待完善]统一保存异常页面
                var path = Path.Combine(WorkPath.ErrorPagePath, $"{ DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss.fffffff")}.html");
                File.WriteAllText(path, dresp.Text);
                return;
            }
            storeQueue.Post(dc);
        }

        /// <summary>
        /// 存储队列处理程序
        /// </summary>
        /// <param name="dc"></param>
        public void StoreHandler(DataContainer dc)
        {
            //TODO [待完善]异常处理
            var dcp = _dcpList[dc.GUID];
            dcp.StoreHandler(dc);
        }

        #endregion

        /// <summary>
        /// 自动运行指定DCP
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task RunAutoGenerator(Guid guid)
        {
            if (AutoRunningList.Where(dcpInfo => dcpInfo.Guid == guid).Count() > 0)
            {
                throw new MagnesiumException("该DCP已在自动运行中");
            }
            var dcp = _dcpList[guid];
            var cts = new CancellationTokenSource();
            AutoRunningList.Add(new RunningDcpInfo(dcp.GUID, dcp.FriendlyName, DateTime.Now, cts));
            try
            {
                await Task.Run(() => dcp.RequestGenerator.AutoRun(cts.Token), cts.Token);
            }
            catch (OperationCanceledException)
            {
                Logger.Info($"DCP: {dcp.GUID} has been successfully canceled");
            }
            AutoRunningList.RemoveAll(dcpInfo => dcpInfo.Guid == guid);
        }

        #region DCP管理相关
        /// <summary>
        /// 获取所有DCP对象
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDataCollectProvider> GetDcps()
        {
            return _dcpList.ForEach(item => item.Value);
        }

        /// <summary>
        /// 确定管理器中是否存在指定GUID的DCP
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool ContainsDcp(Guid guid) => _dcpList.ContainsKey(guid);

        /// <summary>
        /// 索引访问DCP对象
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public IDataCollectProvider this[Guid guid] => _dcpList[guid];

        /// <summary>
        /// 移除指定Guid对应的DCP
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool RemoveDcp(Guid guid) => _dcpList.Remove(guid);

        /// <summary>
        /// 移除由谓词表达式指定的DCP
        /// </summary>
        /// <param name="predicate"></param>
        public void RemoveDcp(Func<IDataCollectProvider, bool> predicate)
        {
            _dcpList.Where(item => predicate(item.Value))
                    .ForEach(item => RemoveDcp(item.Key));
        }

        #endregion
    }

    /// <summary>
    /// 自动运行中DCP的信息
    /// </summary>
    public class RunningDcpInfo
    {
        public Guid Guid { get; set; }
        public string FriendlyName { get; set; }
        public DateTime StartTime { get; set; }
        public CancellationTokenSource CTS { get; set; }
        public RunningDcpInfo(Guid guid, string friendlyName, DateTime startTime, CancellationTokenSource cts)
        {
            Guid = guid;
            FriendlyName = friendlyName;
            StartTime = startTime;
            CTS = cts;
        }


        public bool Equals(RunningDcpInfo other)
        {
            return Guid == other.Guid;
        }

        public override bool Equals(object obj)
        {
            return obj is RunningDcpInfo && this.Equals((RunningDcpInfo)obj);
        }

        public static bool operator ==(RunningDcpInfo first, RunningDcpInfo second)
        {
            return object.Equals(first, second);
        }

        public static bool operator !=(RunningDcpInfo first, RunningDcpInfo second)
        {
            return !object.Equals(first, second);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }    
}
