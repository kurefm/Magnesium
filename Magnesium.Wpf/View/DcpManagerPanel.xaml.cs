using Magnesium.Core;
using Magnesium.Core.Extension;
using Magnesium.Wpf.Dialog;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// DcpManagerPanel.xaml 的交互逻辑
    /// </summary>
    public partial class DcpManagerPanel : ControlBase
    {
        public ObservableCollection<IDataCollectProvider> DcpList { get; } =
            new ObservableCollection<IDataCollectProvider>(Core.Magnesium.Current.GetDcpList());
        public DcpManagerPanel()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void ConfigDcpListEdit()
        {
            DcpList.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        Core.Magnesium.Current.RemoveDcp(dcp => e.OldItems.Contains(dcp));
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        break;
                    default:
                        break;
                }
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.Show(new AddDcpDialog(), "RootDialog");
        }

        private async void NewTask_Click(object sender, RoutedEventArgs e)
        {
            if (_dcpList.SelectedItem == null)
            {
                ErrorDialog.Show("Please select a DCP!");
                return;
            }
            var guid = ((IDataCollectProvider)_dcpList.SelectedItem).GUID;
            try
            {
                await Core.Magnesium.Current.RunAutoGenerator(guid);
            }
            catch (MagnesiumException)
            {
                ErrorDialog.Show("This DCP already in running!");
            }
            catch (NotImplementedException)
            {
                ErrorDialog.Show("This DCP not implement shch function");
            }
        }

        private void Rescan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ViewData_Click(object sender, RoutedEventArgs e)
        {
            if (_dcpList.SelectedItem == null)
            {
                ErrorDialog.Show("Please select a DCP!");
                return;
            }
            var dcp = (IDataCollectProvider)_dcpList.SelectedItem;
            try
            {
                var win = new CustomDataViewer(dcp.DataViewUI);
                win.Show();
            }
            catch (NotImplementedException)
            {
                ErrorDialog.Show("This DCP not implement shch function");
            }
            
        }
    }
}
