using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core.Web
{
    /// <summary>
    /// 预先定义的一组UserAgent字符串，方便定制请求
    /// </summary>
    public class DefaultUserAgent
    {
        public static string Chrome { get; } = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36";

        public static string Nexus5 { get; } = "Mozilla/5.0 (Linux; Android 4.4.4; Nexus 5 Build/KTU84P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.114 Mobile Safari/537.36";

        public static string iPhone5 { get; } = "Mozilla/5.0 (iPhone; CPU iPhone OS 7_0 like Mac OS X; en-us) AppleWebKit/537.51.1 (KHTML, like Gecko) Version/7.0 Mobile/11A465 Safari/9537.53";

        public static string iPad { get; } = "Mozilla/5.0 (iPad; CPU OS 7_0 like Mac OS X) AppleWebKit/537.51.1 (KHTML, like Gecko) Version/7.0 Mobile/11A465 Safari/9537.53";

        public static string Edge { get; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586";

        public static string Firefox { get; } = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:38.0) Gecko/20100101 Firefox/41.0";
    }
}
