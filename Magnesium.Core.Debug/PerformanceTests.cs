using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core.Debug
{
    class PerformanceTests
    {
        static PerformanceCounter cpuCounter;
        static PerformanceCounter ramCounter;


        static PerformanceTests()
        {
            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public static string GetCurrentCpuUsage()
        {
            return cpuCounter.NextValue() + "%";
        }

        public static string GetAvailableRAM()
        {
            return ramCounter.NextValue() + "MB";
        }
    }
}
