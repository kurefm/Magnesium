using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Magnesium.Core
{
    public interface IDataCollectProvider
    {
        #region Property

        /// <summary>
        /// 用于唯一标识DCP的GUID
        /// </summary>
        Guid GUID { get; }

        /// <summary>
        /// DCP的友好名称
        /// </summary>
        string FriendlyName { get; }
        DataRequestGenerator RequestGenerator { get; }
        GeneratorUI GeneratorUI { get; }
        SettingUI SettingUI { get; }
        DataViewUI DataViewUI { get; }

        #endregion

        #region Method

        /// <summary>
        /// 发送请求(DataRequest)的处理程序
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        DataResponse SendHandler(DataRequest dr);

        /// <summary>
        /// 解析结果(DataResponse)的处理程序
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        DataContainer ParseHandler(DataResponse dr);

        /// <summary>
        /// 存储数据(DataContainer)的处理程序
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        bool StoreHandler(DataContainer dc);

        #endregion
    }
}
