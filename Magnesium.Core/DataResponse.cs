using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core
{
    public class DataResponse
    {
        #region Property

        public Guid GUID { get; set; }
        public string Text { get; set; }
        public int StatusCode { get; set; }

        #endregion

        public DataResponse(Guid guid, string text, int statusCode)
        {
            GUID = guid;
            Text = text;
            StatusCode = statusCode;
        }
    }
}
