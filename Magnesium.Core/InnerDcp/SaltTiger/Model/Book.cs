using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core.InnerDcp.SaltTiger.Model
{
    public class Book
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public string OfficialHref { get; set; }

        public string Introduction { get; set; } = "";

        public List<Hyperlink> Hrefs { get; } = new List<Hyperlink>();

        public List<string> Categorys { get; } = new List<string>();

        public List<string> Tags { get; } = new List<string>();

        public Book(string name, DateTime publishDate, string officaialHref)
        {
            Name = name;
            PublishDate = publishDate;
            OfficialHref = officaialHref;
        }

        public bool Equals(Book other)
        {
            return this.Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is Book && this.Equals((Book)obj);
        }

        public static bool operator ==(Book first, Book second)
        {
            return object.Equals(first, second);
        }

        public static bool operator !=(Book first, Book second)
        {
            return !object.Equals(first, second);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
