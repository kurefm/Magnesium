using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magnesium.Core;
using System.Threading;
using Magnesium.Core.Web;
using System.IO;
using Magnesium.Core.DataBase.Sqlite;
using Magnesium.Core.Extension;
using System.Text.RegularExpressions;
using Magnesium.Core.Log;
using System.Diagnostics;
using Magnesium.Core.Config;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Net;

namespace Magnesium.Core.Debug
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            #region 要运行的程序

            //MagnesiumRunTests.RunTests();
            //MagnesiumRunTests.DbTests();

            var a = new List<int>() { 1, 2, 3, 4, 5, 6 };
            var b = ObservableCollectionEx.CreateAndSyncWith(a);
            b.Add(7);
            b.Add(8);
            a.ForEach(i => Console.Write(i + ","));
            Console.WriteLine();
            b.RemoveAt(0);
            b.RemoveAt(5);
            a.ForEach(i => Console.Write(i + ","));
            #endregion

            stopWatch.Stop();
            Console.WriteLine("\n\nProgram run finish!");
            Console.WriteLine($"Run time {stopWatch.ElapsedMilliseconds} ms.");

            Console.ReadKey();


            //var ma = Magnesium.Instance;
            //foreach (var item in ma.HandleProvider.LoadDcp(@"C:\Users\kuref\OneDrive\Codes\Visual Studio\Projects\Magnesium\Magnesium.Sample\bin\Debug\Magnesium.Sample.dll"))
            //{
            //    ma.HandleProvider.RegisterDcp(item);
            //}

            //ma.HandleProvider.GetDcps().ElementAt(0).RequestGenerator.AutoRun();

            //Thread.Sleep(-1);
        }
    }
}
