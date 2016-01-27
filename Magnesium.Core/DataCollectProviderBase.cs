using Magnesium.Core.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Magnesium.Core
{
    /// <summary>
    /// 仅实现SendHandler的基础DCP
    /// </summary>
    public abstract class DataCollectProviderBase : IDataCollectProvider
    {
        public virtual string FriendlyName { get; } = "基础DCP";

        public abstract GeneratorUI GeneratorUI { get; }

        public abstract Guid GUID { get; }

        public abstract DataRequestGenerator RequestGenerator { get; }

        public abstract SettingUI SettingUI { get; }

        public abstract DataViewUI DataViewUI { get; }

        public abstract DataContainer ParseHandler(DataResponse dr);

        public virtual DataResponse SendHandler(DataRequest dr)
        {
            //TODO [待完善]仅作临时使用, 目前仅支持DataRequest的GET请求
            Trace.WriteLine($"HTTP GET {dr.Url}");
            var resp = Http.SafeGet(dr.Url);
            return new DataResponse(dr.GUID, resp.Text, resp.StatusCode);
        }

        public abstract bool StoreHandler(DataContainer dc);
    }
}
