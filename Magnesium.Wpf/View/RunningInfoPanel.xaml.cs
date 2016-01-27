using Magnesium.Core;
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
using System.Windows.Threading;

namespace Magnesium.Wpf.View
{
    /// <summary>
    /// OperationInfoPanel.xaml 的交互逻辑
    /// </summary>
    public partial class RunningInfoPanel : ControlBase
    {
        public RunningInfo Info { get; private set; }
        public RunningInfoPanel()
        {
            InitializeComponent();
            DataContext = this;
            UpdateInfo(Core.Magnesium.Current.RunningInfo);
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += (sender, e) =>
            {
                UpdateInfo(Core.Magnesium.Current.RunningInfo);
            };
            timer.Start();
        }

        private void UpdateInfo(RunningInfo info)
        {
            Info = info;
            OnPropertyChanged(nameof(Info));
        }
    }
}
