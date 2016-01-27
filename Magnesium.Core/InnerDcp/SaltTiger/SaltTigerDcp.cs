using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magnesium.Core;
using System.Text.RegularExpressions;
using Magnesium.Core.Extension;
using Magnesium.Core.InnerDcp.SaltTiger.Model;
using System.Diagnostics;

namespace Magnesium.Core.InnerDcp.SaltTiger
{
    public class SaltTigerDcp : DataCollectProviderBase
    {
        private BookDB db = new BookDB();

        //预编译的正则表达式
        private static Regex parseTitle = new Regex(@"<h1[^>]*?entry-title[^>]*?>(?<Title>[\s\S]*?)</h1>");
        private static Regex parseBaseInfoBody = new Regex(@"<div[^>]*?entry-content[^>]*?>([\s\S]*?)</div>");
        private static Regex parsePublishDate = new Regex(@"(?:(\d{4}\.\d{1,2})|(\d{4}年\d{1,2}月))[\s]*?<br");
        private static Regex parseHref = new Regex(@"<a[\s\S]*?href[\s\S]*?[""'](?<Href>[^""^']*?)[""'][\s\S]*?>(?<Name>[\s\S]*?)</a>");
        private static Regex parseIntroduction = new Regex(@"/strong[\s\S]*?/p[^>]*?>([\s\S]+</p>)");
        private static Regex parseMetaBody = new Regex(@"<footer[^>]*?entry-meta[^>]*?>([\s\S]*?)</footer>");
        private static Regex parseCategory = new Regex(@"<a[^>]*?category[^>]*?>(?<Name>[\s\S]*?)</a>");
        private static Regex parseTag = new Regex(@"<a[^>]*?""tag""[^>]*?>(?<Name>[\s\S]*?)</a>");

        public override GeneratorUI GeneratorUI
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Guid GUID { get; } = Guid.NewGuid();

        public override DataRequestGenerator RequestGenerator => new SaltTigerRequestGenerator(GUID);

        public override SettingUI SettingUI
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override DataContainer ParseHandler(DataResponse dr)
        {
            var html = dr.Text;
            //书的名称
            var name = parseTitle.Match(html).Groups[1].Value;

            var baseInfoHtml = parseBaseInfoBody.Match(html).Value;
            //出版日期, 没有出版日期说明不是书籍页面
            #region 日期处理
            var dateMatch = parsePublishDate.Match(baseInfoHtml).Groups;
            var dateString = string.IsNullOrWhiteSpace(dateMatch[1].Value) ? dateMatch[2].Value : dateMatch[1].Value;
            if (string.IsNullOrWhiteSpace(dateString))
            {
                throw new ParseErrorException("This is not a Book Page");
            }
            DateTime publishDate;
            try
            {
                publishDate = DateTime.Parse(dateString);
            }
            catch (Exception)
            {
                throw new ParseErrorException("This is not a Book Page");
            }
            #endregion
            //链接
            var hrefs = parseHref.Matches(baseInfoHtml).ForEach((Match m) => new Hyperlink(m.Groups[1].Value, m.Groups[2].Value));
            //介绍
            var introduction = parseIntroduction.Match(baseInfoHtml).Groups[1].Value;

            var metaHtml = parseMetaBody.Match(html).Value;
            //分类
            var categorys = parseCategory.Matches(metaHtml).ForEach((Match m) => m.Groups[1].Value);
            //标签
            var tags = parseTag.Matches(metaHtml).ForEach((Match m) => m.Groups[1].Value);

            //生成Book对象
            var book = new Book(name, publishDate, hrefs.ElementAt(0).Href) { Introduction = introduction };
            book.Hrefs.AddRange(hrefs.Skip(1));
            book.Categorys.AddRange(categorys);
            book.Tags.AddRange(tags);

            return new DataContainer(dr.GUID, book);
        }

        public override bool StoreHandler(DataContainer dc)
        {
            var book = (Book)dc.Data;
            db.Insert(book);
            Trace.WriteLine($"Store Book: {book.Name}");
            return true;
        }
    }
}
