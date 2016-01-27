using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Crawler.Web
{
    public class HttpRespones
    {
        /// <summary>
        /// 响应正文
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 响应状态码
        /// </summary>
        public int StatusCode { get; set; }
    }
}
