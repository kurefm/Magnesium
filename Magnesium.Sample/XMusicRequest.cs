using Magnesium.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Sample
{
    public class XMusicRequest : DataRequest
    {
        public int ID { get; set; }

        public XMusicRequest(Guid guid, string url, int id) : base(guid, url)
        {
            ID = id;
        }
    }
}
