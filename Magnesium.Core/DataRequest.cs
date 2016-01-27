using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core
{
    public class DataRequest
    {
        #region Property

        public Guid GUID { get; protected set; }
        public string Url { get; protected set; }
        public string Method { get; set; } = "GET";
        public string Data { get; set; }
        public bool IsUseFilter { get; set; } = true;

        #endregion

        public DataRequest(Guid guid, string url)
        {
            GUID = guid;
            Url = url;
        }
    }
}
