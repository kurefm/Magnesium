using Magnesium.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Sample
{
    public class XMusicResponse : DataResponse
    {
        public int ID { get; set; }
        public XMusicResponse(Guid guid, string text, int statusCode, int id) : base(guid, text, statusCode)
        {
            ID = id;
        }
    }
}
