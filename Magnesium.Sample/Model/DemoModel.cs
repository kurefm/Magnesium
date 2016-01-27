using Magnesium.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Sample.Model
{
    public class DemoModel : DataContainer
    {
        public DemoModel(Guid guid, string html, int id) : base(guid)
        {
            Html = html;
            ID = id;
        }

        public string Html { get; set; }
        public int ID { get; set; }
    }
}
