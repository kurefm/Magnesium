using Magnesium.Core.Extension;
using Magnesium.Core.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Magnesium.Core.Web
{
    public class Http
    {
        #region Default

        private const string DEF_USERAGENT = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
        private const string DEF_ENCODING = "UTF-8";
        private const int DEF_TIMEOUT = 60 * 1000;


        #endregion

        public static HttpResponse Get(string url, int timeout = DEF_TIMEOUT)
        {

            using (var r = GetResponse("GET", url, timeout))
            {

                //使用StreamReader以字符串的方式读出流的内容
                try
                {
                    using (var sr = new StreamReader(r.GetResponseStream(), Encoding.GetEncoding(r.CharacterSet ?? DEF_ENCODING)))
                    {
                        return new HttpResponse() { Text = WebUtility.HtmlDecode(sr.ReadToEnd()).Trim(), StatusCode = (int)r.StatusCode };
                    }
                }
                catch (Exception ex)
                {
                    throw new HttpIOException("网络IO错误", ex);
                }
            }
        }

        public static HttpResponse Post(string url, byte[] data = null, int timeout = DEF_TIMEOUT)
        {

            using (var r = GetResponse("POST", url, timeout, data))
            {

                //使用StreamReader以字符串的方式读出流的内容
                try
                {
                    using (var sr = new StreamReader(r.GetResponseStream(), Encoding.GetEncoding(r.CharacterSet ?? DEF_ENCODING)))
                    {
                        return new HttpResponse() { Text = WebUtility.HtmlDecode(sr.ReadToEnd()).Trim(), StatusCode = (int)r.StatusCode };
                    }
                }
                catch (Exception ex)
                {
                    throw new HttpIOException("网络IO错误", ex);
                }
            }
        }

        public static HttpResponse Post(string url, string data, int timeout = DEF_TIMEOUT)
        {
            return Post(url, data.Encode(), timeout);
        }

        public static HttpResponse Put(string url, byte[] data = null, int timeout = DEF_TIMEOUT)
        {

            using (var r = GetResponse("PUT", url, timeout, data))
            {

                //使用StreamReader以字符串的方式读出流的内容
                try
                {
                    using (var sr = new StreamReader(r.GetResponseStream(), Encoding.GetEncoding(r.CharacterSet ?? DEF_ENCODING)))
                    {
                        return new HttpResponse() { Text = WebUtility.HtmlDecode(sr.ReadToEnd()).Trim(), StatusCode = (int)r.StatusCode };
                    }
                }
                catch (Exception ex)
                {
                    throw new HttpIOException("网络IO错误", ex);
                }
            }
        }

        public static HttpResponse Put(string url, string data, int timeout = DEF_TIMEOUT)
        {
            return Put(url, data.Encode(), timeout);
        }

        public static HttpResponse Delete(string url, int timeout = DEF_TIMEOUT)
        {

            using (var r = GetResponse("DELETE", url, timeout))
            {

                //使用StreamReader以字符串的方式读出流的内容
                try
                {
                    using (var sr = new StreamReader(r.GetResponseStream(), Encoding.GetEncoding(r.CharacterSet ?? DEF_ENCODING)))
                    {
                        return new HttpResponse() { Text = WebUtility.HtmlDecode(sr.ReadToEnd()).Trim(), StatusCode = (int)r.StatusCode };
                    }
                }
                catch (Exception ex)
                {
                    throw new HttpIOException("网络IO错误", ex);
                }
            }
        }


        public static HttpResponse Trace(string url, int timeout = DEF_TIMEOUT)
        {

            using (var r = GetResponse("TRACE", url, timeout))
            {

                //使用StreamReader以字符串的方式读出流的内容
                try
                {
                    using (var sr = new StreamReader(r.GetResponseStream(), Encoding.GetEncoding(r.CharacterSet ?? DEF_ENCODING)))
                    {
                        return new HttpResponse() { Text = WebUtility.HtmlDecode(sr.ReadToEnd()).Trim(), StatusCode = (int)r.StatusCode };
                    }
                }
                catch (Exception ex)
                {
                    throw new HttpIOException("网络IO错误", ex);
                }
            }
        }

        public static HttpResponse SafeGet(string url, int timeout = DEF_TIMEOUT, int retryTimes = 16)
        {
            //失败时使用指数退避来发生请求
            foreach (var i in Enumerable.Range(1, retryTimes))
            {
                try
                {
                    return Get(url, timeout);
                }
                catch (Exception ex)
                {
                    //TODO 必要时记录错误
                    //指数退避
                    int wait = (int)Math.Pow(2, i);
                    Logger.Warn($"Http request error, wait {wait} ms to retry.", ex);
                    Thread.Sleep(wait);
                }
            }
            throw new HttpRetryException($"网络请求重试了{retryTimes}次，依旧未成功");
        }
        private static HttpWebResponse GetResponse(string method, string url, int timeout, byte[] data = null)
        {
            var hwq = (HttpWebRequest)WebRequest.Create(url);

            hwq.Method = method.ToUpper();
            //设置默认UserAgent
            hwq.UserAgent = DEF_USERAGENT;
            //设置接收GZip,Deflate压缩以减少网络带宽消耗
            hwq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            hwq.Timeout = timeout;
            hwq.KeepAlive = true;
            if (data != null)
            {
                hwq.ContentLength = data.Length;
                using (var s = hwq.GetRequestStream())
                {
                    s.Write(data, 0, data.Length);
                }
            }

            try
            {
                Logger.Debug($"HTTP {hwq.Method} {url}");
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

        public static HttpResponse Head(string url, int timeout = DEF_TIMEOUT)
        {

            using (var r = GetResponse("HEAD", url, timeout))
            {
                return new HttpResponse() { Text = "", StatusCode = (int)r.StatusCode };
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
