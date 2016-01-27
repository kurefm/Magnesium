using Magnesium.Core;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Magnesium.Wpf.View
{
    /// <summary>
    /// LogViewerPanel.xaml 的交互逻辑
    /// </summary>
    public partial class LogViewerPanel : ControlBase
    {
        private const int BACK_OFFSET = 65536;
        private string _filename = "magnesium.log";
        private long _lastOffset;
        public LogViewerPanel()
        {
            InitializeComponent();
        }

        private async void _refresh_Click(object sender, RoutedEventArgs e)
        {
            await InitContent();
        }

        private async Task InitContent()
        {
            using (StreamReader reader = new StreamReader(new FileStream(_filename,
                     FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                if (reader.BaseStream.Length > BACK_OFFSET)
                {
                    reader.BaseStream.Seek(-BACK_OFFSET, SeekOrigin.End);
                    reader.ReadLine();
                }

                _logView.Text = await reader.ReadToEndAsync();
                _logView.ScrollToEnd();
                _lastOffset = reader.BaseStream.Position;
            }
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += async (sender, e) =>
            {
                await UpdateContent();
            };
            timer.Start();
        }

        private async Task UpdateContent()
        {
            using (StreamReader reader = new StreamReader(new FileStream(_filename,
         FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                reader.BaseStream.Seek(_lastOffset, SeekOrigin.Begin);

                string line = "";
                bool changed = false;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    changed = true;
                    _logView.AppendText(line + Environment.NewLine);
                }
                if (changed)
                {

                    _logView.ScrollToEnd();
                }
                _lastOffset = reader.BaseStream.Position;
            }
        }

        private async void ControlBase_Loaded(object sender, RoutedEventArgs e)
        {
            await InitContent();
        }
    }
}
