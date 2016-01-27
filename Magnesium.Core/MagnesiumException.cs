using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core
{
    /// <summary>
    /// Magnesium爬虫框架中异常的基类
    /// </summary>
    [Serializable]
    public class MagnesiumException : Exception
    {
        public MagnesiumException() { }
        public MagnesiumException(string message) : base(message) { }
        public MagnesiumException(string message, Exception inner) : base(message, inner) { }
        protected MagnesiumException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    /// <summary>
    /// 数据解析出错时发生的异常
    /// </summary>
    [Serializable]
    public class ParseErrorException : MagnesiumException
    {
        public ParseErrorException() { }
        public ParseErrorException(string message) : base(message) { }
        public ParseErrorException(string message, Exception inner) : base(message, inner) { }
        protected ParseErrorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    /// <summary>
    /// DCP的Guid已经存在时发生的异常   
    /// </summary>
    [Serializable]
    public class DcpGuidAlreadyExistsException : MagnesiumException
    {
        public DcpGuidAlreadyExistsException() { }
        public DcpGuidAlreadyExistsException(string message) : base(message) { }
        public DcpGuidAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected DcpGuidAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    /// <summary>
    /// 未加载配置文件而去访问配置项时发生的异常
    /// </summary>
    [Serializable]
    public class NotLoadConfigException : MagnesiumException
    {
        public NotLoadConfigException() : base("请先使用LoadConfig加载配置文件") { }
        public NotLoadConfigException(string message) : base(message) { }
        public NotLoadConfigException(string message, Exception inner) : base(message, inner) { }
        protected NotLoadConfigException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
