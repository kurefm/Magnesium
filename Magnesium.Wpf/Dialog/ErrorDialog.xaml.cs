using Magnesium.Wpf.View;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Magnesium.Wpf.Dialog
{
    /// <summary>
    /// ErrorDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ErrorDialog : ControlBase
    {
        private string _errorMsg = "Your Error Message";

        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { Set<string>(ref _errorMsg, value,""); }
        }



        public ErrorDialog()
        {
            InitializeComponent();
            DataContext = this;
        }
        public ErrorDialog(string msg) : this()
        {
            ErrorMsg = msg;
        }

        public static void Show(string msg)
        {
            var ed = new ErrorDialog(msg);
            DialogHost.Show(ed, "RootDialog");
        }
    }
}
