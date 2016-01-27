using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Magnesium.Core;
using System.Threading;

namespace Magnesium.Wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            var loading = new LoadingWindow();
            loading.Show();
            InitMagnesium();
            try
            {
                InitializeComponent();
            }
            finally
            {
                loading.Close();
            }
        }

        /// <summary>
        /// 初始化Magnesium运行环境
        /// </summary>
        private void InitMagnesium()
        {
            var m = new Core.Magnesium();
            m.MagnesiumInit();
            m.LoadConfig(WorkPath.ConfigPath);
            m.LoadDcp(WorkPath.DcpPath);
        }
    }
}
