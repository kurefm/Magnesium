using Magnesium.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SaltTiger
{
    public class DataViewer : DataViewUI
    {
        public DataViewer()
        {
            var panel = new StackPanel();
            panel.Children.Add(new Button() { Content = "Button" });
            this.AddChild(panel);
        }
    }
}
