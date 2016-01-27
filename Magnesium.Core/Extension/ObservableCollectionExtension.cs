using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core.Extension
{
    public static class ObservableCollectionExtension
    {
        /// <summary>
        /// 将ObservableCollection与List同步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obserList"></param>
        /// <param name="list"></param>
        internal static void SyncWith<T>(this ObservableCollection<T> obserList, List<T> list)
        {
            obserList.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        list.AddRange(e.NewItems.Cast<T>());
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        try
                        {
                            list.RemoveAt(e.OldStartingIndex);
                        }
                        catch (ArgumentOutOfRangeException) { }
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

        public static void AddRange<T>(this ObservableCollection<T> obserList, IEnumerable<T> items)
        {
            items.ForEach(item => obserList.Add(item));
        }
    }

    public static class ObservableCollectionEx
    {
        /// <summary>
        /// 使用List创建ObservableCollection，并将其与List同步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ObservableCollection<T> CreateAndSyncWith<T>(List<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            var obverList = new ObservableCollection<T>(list);
            obverList.SyncWith(list);
            return obverList;
        }
    }
}
