using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Magnesium.Wpf.ViewModel
{
    /// <summary>
    /// 可以通知属性变化的基类
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 触发PropertyChanged
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 若字段与新值不相等，则修改字段并触发PropertyChanged
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filed">字段</param>
        /// <param name="newValue">新的值</param>
        /// <param name="propertyName">属性名</param>
        protected void Set<T>(ref T filed, T newValue = default(T), [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(filed, newValue))
            {
                filed = newValue;
                OnPropertyChanged(propertyName);
            }
        }
    }


}
