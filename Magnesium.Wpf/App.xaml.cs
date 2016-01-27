using Magnesium.Core.Log;
using Magnesium.Wpf.Dialog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Magnesium.Wpf
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
#if !DEBUG
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Fatal("Application Error", (Exception)e.ExceptionObject);
            MsgDialog.Show("Sorry, some errors occurred, the error message is saved in log");
            Application.Current.Shutdown();
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Fatal("Application Error", e.Exception);
            MsgDialog.Show("Sorry, some errors occurred, the error message is saved in log");
            Application.Current.Shutdown();
            e.Handled = true;//使用这一行代码告诉运行时，该异常被处理了，不再作为UnhandledException抛出了。
        }
#endif
    }
}
