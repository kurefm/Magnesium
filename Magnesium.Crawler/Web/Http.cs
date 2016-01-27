using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Crawler.Web
{
    public class Http
    {
        #region Default

        private const string DEF_USERAGENT = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
        private const string DEF_ENCODING = "UTF-8";
        private const int DEF_TIMEOUT = 60 * 1000;


        #endregion

        public static HttpRespones Get(string url, int timeout = DEF_TIMEOUT)
        {

            using (var r = GetResponse(url, timeout))
            {

                //使用StreamReader以字符串的方式读出流的内容
                try
                {
                    using (var sr = new StreamReader(r.GetResponseStream(), Encoding.GetEncoding(DEF_ENCODING)))
                    {
                        return new HttpRespones() { Text = WebUtility.HtmlDecode(sr.ReadToEnd()), StatusCode = (int)r.StatusCode };
                    }
                }
                catch (Exception ex)
                {
                    throw new HttpIOException("网络IO错误", ex);
                }
            }
        }

        private static HttpWebResponse GetResponse(string url, int timeout)
        {
            var hwq = (HttpWebRequest)HttpWebRequest.Create(url);

            //设置默认UserAgent
            hwq.UserAgent = DEF_USERAGENT;
            //设置接收GZip,Deflate压缩以减少网络带宽消耗
            hwq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            hwq.Timeout = timeout;
            hwq.KeepAlive = true;

            try
            {
                return (HttpWebResponse)hwq.GetResponse();
            }
            catch (WebException we)
            {
                if (we.Response != null)
                {
                    return (HttpWebResponse)we.Response;
                }
                throw new HttpException("发生未知错误", we);
            }
        }

        private static async Task<HttpWebResponse> GetResponseAsync(string url, int timeout)
        {
            var hwq = (HttpWebRequest)HttpWebRequest.Create(url);

            //设置默认UserAgent
            hwq.UserAgent = DEF_USERAGENT;
            //设置接收GZip,Deflate压缩以减少网络带宽消耗
            hwq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            hwq.Timeout = timeout;
            hwq.KeepAlive = true;

            try
            {
                return (HttpWebResponse)await hwq.GetResponseAsync();
            }
            catch (WebException we)
            {
                if (we.Response != null)
                {
                    return (HttpWebResponse)we.Response;
                }
                throw new HttpException("发生未知错误", we);
            }
        }
    }
}
