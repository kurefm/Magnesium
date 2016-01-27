using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core.Web
{
    /// <summary>
    /// 最基础的HTTP异常，所有HTTP异常均继承自该类
    /// </summary>
    [Serializable]
    public class HttpException : Exception
    {
        public HttpException() { }
        public HttpException(string message) : base(message) { }
        public HttpException(string message, Exception inner) : base(message, inner) { }
        protected HttpException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    /// <summary>
    /// HTTP超时引发的异常
    /// </summary>
    [Serializable]
    public class HttpTimeoutException : HttpException
    {
        public HttpTimeoutException() { }
        public HttpTimeoutException(string message) : base(message) { }
        public HttpTimeoutException(string message, Exception inner) : base(message, inner) { }
        protected HttpTimeoutException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    /// <summary>
    /// HTTP读写中引发的IO错误
    /// </summary>
    [Serializable]
    public class HttpIOException : HttpException
    {
        public HttpIOException() { }
        public HttpIOException(string message) : base(message) { }
        public HttpIOException(string message, Exception inner) : base(message, inner) { }
        protected HttpIOException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }


    [Serializable]
    public class HttpRetryException : HttpException
    {
        public HttpRetryException() { }
        public HttpRetryException(string message) : base(message) { }
        public HttpRetryException(string message, Exception inner) : base(message, inner) { }
        protected HttpRetryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
