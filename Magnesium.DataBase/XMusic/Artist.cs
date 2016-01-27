using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.DataBase.XMusic
{
    public class Artist : XModleBase
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Area { get; set; }
        public string Genre { get; set; }
        public long FansCount { get; set; } = 0;
        public string Profile { get; set; }

        public Artist(long xId, string name) : base(xId)
        {
            Name = name;
        }
    }
}
