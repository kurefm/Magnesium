using Magnesium.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.IO;
using System.Net;
using Magnesium.Core.Base;
using Magnesium.Core.Extension;

namespace Magnesium.Core
{
    public class Magnesium
    {
        private static object _globalLock;
        private static Magnesium _instance;

        private ExecutionDataflowBlockOptions _requestQueueOptions;
        private ActionBlock<DataRequest> _requestQueue;
        private ActionBlock<DataResponse> _parseQueue;
        private ActionBlock<DataContainer> _storeQueue;
        private BloomFilter<string> _bloomFilter;
        private MagnesiumConfig _config;

        private DcpManager _dcpManager;
        private ConfigManager _configManager;

        private long _requestCount = 0;

        public static Magnesium Current => _instance;
        public BloomFilter<string> BloomFilter => _bloomFilter;
        public MagnesiumConfig Config => _config;
        public List<RunningDcpInfo> RunningTask => _dcpManager.AutoRunningList;

        public RunningInfo RunningInfo => new RunningInfo()
        {
            RequestQueueCache = _requestQueue.InputCount,
            ParseQueueCache = _parseQueue.InputCount,
            StoreQueueCache = _storeQueue.InputCount,
            RequestCount = _requestCount
        };

        static Magnesium()
        {
            _globalLock = new object();
        }

        public Magnesium()
        {
            lock (_globalLock)
            {
                if (_instance == null)
                {
                    MagnesiumInit();
                    _instance = this;
                }
            }
        }

        public void MagnesiumInit()
        {
            _dcpManager = new DcpManager();
            _requestQueueOptions = new ExecutionDataflowBlockOptions();
            _storeQueue = new ActionBlock<DataContainer>(dc => _dcpManager.StoreHandler(dc));
            _parseQueue = new ActionBlock<DataResponse>(dresp => _dcpManager.ParseHandler(dresp, _storeQueue));
            _requestQueue = new ActionBlock<DataRequest>(dr => _dcpManager.RequestHandler(dr, _parseQueue), _requestQueueOptions);
            //布隆过滤器, 大小10M, 容错率0.001
            _bloomFilter = new BloomFilter<string>(10000000, 0.001f);
        }

        private void UpdateConfig()
        {
            if (_config == null)
            {
                throw new NotLoadConfigException();
            }
            ServicePointManager.DefaultConnectionLimit = _config.HttpConnectionLimit;
            _requestQueueOptions.MaxDegreeOfParallelism = _config.RequestDegreeOfParallelism;
        }

        /// <summary>
        /// 从指定路径加载配置文件
        /// </summary>
        /// <param name="path">配置文件路径</param>
        public void LoadConfig(string path = "config.json")
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (File.Exists(path) && File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                throw new ArgumentException("提供的path为目录，path必须为配置文件");
            }
            //加载配置文件
            _configManager = new ConfigManager(path);
            _config = _configManager.Config;
            UpdateConfig();
            //配置对象发生变化时更新配置项
            _config.ConfigChanged += (sender, e) => UpdateConfig();
        }

        /// <summary>
        /// 从指定路径加载DCP
        /// </summary>
        /// <param name="path"></param>
        public void LoadDcp(string path) => _dcpManager.RegisterDcp(path);

        public void Run()
        {

        }

        /// <summary>
        /// 获取DCP列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDataCollectProvider> GetDcpList() => _dcpManager.GetDcps();

        /// <summary>
        /// 移除指定Guid对应的DCP
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool RemoveDcp(Guid guid) => _dcpManager.RemoveDcp(guid);

        /// <summary>
        /// 移除由谓词表达式指定的DCP
        /// </summary>
        /// <param name="predicate"></param>
        public void RemoveDcp(Func<IDataCollectProvider, bool> predicate) => _dcpManager.RemoveDcp(predicate);

        public async Task RunAutoGenerator(Guid guid) => await _dcpManager.RunAutoGenerator(guid);

        public List<RunningDcpInfo> AutoRunningList => _dcpManager.AutoRunningList;

        internal bool Post(DataRequest dr)
        {
            _requestCount += 1;
            return _requestQueue.Post(dr);
        }

        internal async Task<bool> PostAsync(DataRequest dr)
        {
            _requestCount += 1;
            return await _requestQueue.SendAsync(dr);
        }
    }

    public class RunningInfo
    {
        public int RequestQueueCache { get; set; }
        public int ParseQueueCache { get; set; }
        public int StoreQueueCache { get; set; }
        public long RequestCount { get; set; }
    }
}
