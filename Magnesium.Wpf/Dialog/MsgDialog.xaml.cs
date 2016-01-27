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
    /// MsgDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MsgDialog : ControlBase
    {
        private string _msg = "Your Message";

        public string Msg
        {
            get { return _msg; }
            set { Set<string>(ref _msg, value, ""); }
        }
        public MsgDialog()
        {
            InitializeComponent();
            DataContext = this;
        }
        public MsgDialog(string msg) : this()
        {
            Msg = msg;
        }

        public static void Show(string msg)
        {
            var ed = new MsgDialog(msg);
            DialogHost.Show(ed, "RootDialog");
        }

    }
}
