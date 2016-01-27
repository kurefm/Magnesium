using Magnesium.Core;
using Magnesium.Core.DataBase.Sqlite;
using Magnesium.Core.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
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

namespace Magnesium.Wpf.View
{
    /// <summary>
    /// DataViewerPanel.xaml 的交互逻辑
    /// </summary>
    public partial class DataViewerPanel : ControlBase
    {
        public ObservableCollection<string> DbFileList { get; } = new ObservableCollection<string>();

        public ObservableCollection<string> DbTableList { get; private set; }

        public DataViewerPanel()
        {
            Init();
            InitializeComponent();
            DataContext = this;
        }

        public void Init()
        {
            Directory.GetFiles(WorkPath.DbPath).ForEach(item => DbFileList.Add(Path.GetFileName(item)));
        }

        private async void _dbFileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_dbFileList.SelectedItem == null)
            {
                return;
            }
            var path = WorkPath.DbPath + @"\" + (string)_dbFileList.SelectedItem;
            DbTableList = await Task.Run(() =>
            {
                var db = new Sqlite3(path);
                return new ObservableCollection<string>(db.GetTables());
            });

            OnPropertyChanged(nameof(DbTableList));
        }

        private async void _dbTableList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_dbFileList.SelectedItem == null)
            {
                return;
            }
            var path = WorkPath.DbPath + @"\" + (string)_dbFileList.SelectedItem;
            var tableName = (string)_dbTableList.SelectedItem;
            _dataViewer.ItemsSource = await Task.Run(() =>
            {
                var db = new Sqlite3(path);
                return db.GetDatas(tableName).DefaultView;
            });
        }

        private async void _refresh_Click(object sender, RoutedEventArgs e)
        {
            if (_dbFileList.SelectedItem == null)
            {
                return;
            }
            var path = WorkPath.DbPath + @"\" + (string)_dbFileList.SelectedItem;
            var tableName = (string)_dbTableList.SelectedItem;
            _dataViewer.ItemsSource = await Task.Run(() =>
            {
                var db = new Sqlite3(path);
                return db.GetDatas(tableName).DefaultView;
            });
        }
    }
}
