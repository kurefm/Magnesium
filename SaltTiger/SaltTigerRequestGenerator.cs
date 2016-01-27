using Magnesium.Core;
using Magnesium.Core.Extension;
using Magnesium.Core.Log;
using Magnesium.Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SaltTiger
{
    public class SaltTigerRequestGenerator : DataRequestGenerator
    {
        private static int interval = 1000;

        private static Regex parseBookHref = new Regex(@"<li>[\s\S]*?<a href=""([^""]+?)"">[\s\S]*?</a>[^>^<]*?<span title=""评论数量"">[\s\S]*?</span>[^>^<]*?</li>");
        public SaltTigerRequestGenerator(Guid guid) : base(guid)
        {
        }

        public override void AutoRun(CancellationToken token)
        {
            var r = Http.SafeGet("http://www.salttiger.com/archives/");

            var m = parseBookHref.Matches(r.Text);

            //TODO 解析地址, 生成请求
            m.ForEach<Match>(match =>
            {
                var url = match.Groups[1].Value.Trim();
                Logger.Info($"Create request, url: {url}");
                Add(new DataRequest(GUID, url));
                Thread.Sleep(interval);
                token.ThrowIfCancellationRequested();
            });
        }
    }
}
