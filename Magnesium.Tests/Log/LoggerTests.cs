using Microsoft.VisualStudio.TestTools.UnitTesting;
using Magnesium.Core.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core.Log.Tests
{
    [TestClass()]
    public class LoggerTests
    {
        [TestMethod()]
        public void LoggerTest()
        {
            Logger.Debug("This is a debug");
            Logger.Info("This is a info");
            Logger.Warn("This is a warn");
            Logger.Error("This is a error");
            Logger.Fatal("This is a fatal");
            //记录异常的日志
            Logger.Debug("This is a debug", new Exception("debug"));
            Logger.Info("This is a info", new Exception("info"));
            Logger.Warn("This is a warn", new Exception("warn"));
            Logger.Error("This is a error", new Exception("error"));
            Logger.Fatal("This is a fatal", new Exception("fatal"));
        }
    }
}