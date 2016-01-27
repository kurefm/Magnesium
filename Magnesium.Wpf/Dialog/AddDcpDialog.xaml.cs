﻿using Magnesium.Core;
using System;
using System.Collections.Generic;
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

namespace Magnesium.Wpf.Dialog
{
    /// <summary>
    /// AddDcpDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AddDcpDialog : UserControl
    {
        public AddDcpDialog()
        {
            InitializeComponent();
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(WorkPath.DcpPath);
            }
            catch { }
        }
    }
}
