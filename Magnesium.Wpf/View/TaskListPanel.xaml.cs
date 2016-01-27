using Magnesium.Core;
using Magnesium.Core.Extension;
using Magnesium.Wpf.Dialog;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Magnesium.Wpf.View
{
    /// <summary>
    /// TaskListPanel.xaml 的交互逻辑
    /// </summary>
    public partial class TaskListPanel : ControlBase
    {
        public ObservableCollection<RunningDcpInfo> TaskList { get; } =
            new ObservableCollection<RunningDcpInfo>(Core.Magnesium.Current.AutoRunningList);

        public TaskListPanel()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void ControlBase_Loaded(object sender, RoutedEventArgs e)
        {
            var noin = from item in Core.Magnesium.Current.AutoRunningList
                       where !TaskList.Contains(item)
                       select item;
            TaskList.AddRange(noin);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_taskList.SelectedItem == null)
            {
                ErrorDialog.Show("Please select a DCP!");
                return;
            }
            var info = (RunningDcpInfo)_taskList.SelectedItem;
            info.CTS.Cancel();
            MsgDialog.Show($"Cancel task success");
            TaskList.Remove(info);
        }
    }
}
