using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core.Extension
{
    public static class IEnumerableExtension
    {
        #region 泛型迭代器
        /// <summary>        
        /// 用指定操作在泛型迭代器上进行遍历
        /// </summary>
        /// <typeparam name="TSourse"></typeparam>
        /// <param name="source"></param>
        /// <param name="action">执行操作的委托</param>
        public static void ForEach<TSourse>(this IEnumerable<TSourse> source, Action<TSourse> action)
        {
            foreach (var item in source)
            {
                action.Invoke(item);
            }
        }

        /// <summary>
        /// 用指定操作在泛型迭代器上进行遍历
        /// </summary>
        /// <typeparam name="TSourse"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<TSourse>(this IEnumerable<TSourse> source, Action<Int32, TSourse> action)
        {
            int index = -1;
            foreach (var item in source)
            {
                checked { index++; }
                action.Invoke(index, item);
            }
        }

        /// <summary>
        /// 用指定操作在泛型迭代器上进行遍历
        /// </summary>
        /// <typeparam name="TSourse"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="func">执行操作的委托</param>
        /// <returns></returns>
        public static IEnumerable<TResult> ForEach<TSourse, TResult>(this IEnumerable<TSourse> source, Func<TSourse, TResult> func)
        {
            foreach (var item in source)
            {
                yield return func.Invoke(item);
            }
        }

        /// <summary>
        /// 用指定操作在泛型迭代器上进行遍历
        /// </summary>
        /// <typeparam name="TSourse"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="func">执行操作的委托</param>
        /// <returns></returns>
        public static IEnumerable<TResult> ForEach<TSourse, TResult>(this IEnumerable<TSourse> source, Func<Int32, TSourse, TResult> func)
        {
            int index = -1;
            foreach (var item in source)
            {
                checked { index++; }
                yield return func.Invoke(index, item);
            }
        }
        #endregion

        /// <summary>        
        /// 用指定操作在迭代器上进行遍历
        /// </summary>
        /// <typeparam name="TSourse"></typeparam>
        /// <param name="source"></param>
        /// <param name="action">执行操作的委托</param>
        public static void ForEach<TSourse>(this IEnumerable source, Action<TSourse> action)
        {
            foreach (TSourse item in source)
            {
                action.Invoke(item);
            }
        }

        /// <summary>        
        /// 用指定操作在迭代器上进行遍历
        /// </summary>
        /// <typeparam name="TSourse"></typeparam>
        /// <param name="source"></param>
        /// <param name="action">执行操作的委托</param>
        public static void ForEach<TSourse>(this IEnumerable source, Action<Int32, TSourse> action)
        {
            int index = -1;
            foreach (TSourse item in source)
            {
                checked { index++; }
                action.Invoke(index, item);
            }
        }

        /// <summary>
        /// 用指定操作在迭代器上进行遍历
        /// </summary>
        /// <typeparam name="TSourse"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="func">执行操作的委托</param>
        /// <returns></returns>
        public static IEnumerable<TResult> ForEach<TSourse, TResult>(this IEnumerable source, Func<TSourse, TResult> func)
        {
            foreach (TSourse item in source)
            {
                yield return func.Invoke(item);
            }
        }

        /// <summary>
        /// 用指定操作在迭代器上进行遍历
        /// </summary>
        /// <typeparam name="TSourse"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="func">执行操作的委托</param>
        /// <returns></returns>
        public static IEnumerable<TResult> ForEach<TSourse, TResult>(this IEnumerable source, Func<Int32, TSourse, TResult> func)
        {
            int index = -1;
            foreach (TSourse item in source)
            {
                checked { index++; }
                yield return func.Invoke(index, item);
            }
        }
    }
}
