using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core.Log
{
    /// <summary>
    /// 提供统一的日志存储方法
    /// </summary>
    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Info(object message)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[INFO]  {message}");
#endif
            log.Info(message);
        }

        public static void Info(object message, Exception exception)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[INFO]  {message}, EX: {exception}");
#endif
            log.Info(message, exception);
        }

        public static void Debug(object message)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[DEBUG] {message}");
#endif
            log.Debug(message);
        }

        public static void Debug(object message, Exception exception)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[DEBUG] {message}, EX: {exception}");
#endif
            log.Debug(message, exception);
        }

        public static void Warn(object message)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[WARN]  {message}");
#endif
            log.Warn(message);
        }

        public static void Warn(object message, Exception exception)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[WARN]  {message}, EX: {exception}");
#endif
            log.Warn(message, exception);
        }

        public static void Error(object message)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[ERROR] {message}");
#endif
            log.Error(message);
        }

        public static void Error(object message, Exception exception)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[ERROR] {message} EX: {exception}");
#endif
            log.Error(message, exception);
        }

        public static void Fatal(object message)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[FATAL] {message}");
#endif
            log.Fatal(message);
        }

        public static void Fatal(object message, Exception exception)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[FATAL] {message} EX: {exception}");
#endif
            log.Fatal(message, exception);
        }
    }
}
