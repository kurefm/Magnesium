using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core.Web
{
    public class HttpRequest
    {
        #region Property

        public string Method { get; set; }
        public string Url { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Data { get; set; }
        public Dictionary<string, string> Params { get; set; }

        #endregion

        #region Method

        public HttpResponse GetResponse(string method)
        {
            return new HttpResponse();
        }

        #endregion
    }
}
