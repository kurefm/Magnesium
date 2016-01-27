using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core.InnerDcp.SaltTiger.Model
{
    /// <summary>
    /// 表示一个超链接对象
    /// </summary>
    public class Hyperlink
    {
        /// <summary>
        /// 超链接地址
        /// </summary>
        public string Href { get; set; }
        /// <summary>
        /// 超链接名称
        /// </summary>
        public string Name { get; set; }

        public Hyperlink(string href, string name = "")
        {
            Href = href;
            Name = name;
        }

        public bool Equals(Hyperlink other)
        {
            return this.Href == other.Href;
        }

        public override bool Equals(object obj)
        {
            return obj is Hyperlink && this.Equals((Hyperlink)obj);
        }

        public static bool operator ==(Hyperlink first, Hyperlink second)
        {
            return object.Equals(first, second);
        }

        public static bool operator !=(Hyperlink first, Hyperlink second)
        {
            return !object.Equals(first, second);
        }

        public override int GetHashCode()
        {
            return this.Href.GetHashCode();
        }

        public override string ToString()
        {
            return Href;
        }
    }
}
