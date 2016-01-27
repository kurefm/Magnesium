using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Magnesium.Core
{
    public abstract class DataRequestGenerator
    {
        public Guid GUID { get; protected set; }

        public DataRequestGenerator(Guid guid)
        {
            GUID = guid;
        }

        /// <summary>
        /// 提供自动运行的方法
        /// 该方法应以阻塞的方式实现, 当方法体运行完毕后, 可能无法保证方法内资源的线程安全
        /// </summary>
        public abstract void AutoRun(CancellationToken token);

        /// <summary>
        /// 将数据请求加入请求队列
        /// </summary>
        /// <param name="dr"></param>
        /// <returns>如果成功添加到请求队列中则返回true，否则返回false</returns>
        public bool Add(DataRequest dr)
        {
            var bloomFilter = Magnesium.Current.BloomFilter;
            //检查布隆过滤器中是否有对应的URL
            if (dr.IsUseFilter && !bloomFilter.Contains(dr.Url))
            {
                if (Magnesium.Current.Post(dr))
                {
                    bloomFilter.Add(dr.Url);
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// 异步地将数据请求加入请求队列
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(DataRequest dr)
        {
            var bloomFilter = Magnesium.Current.BloomFilter;
            //检查布隆过滤器中是否有对应的URL
            if (dr.IsUseFilter && !bloomFilter.Contains(dr.Url))
            {
                if (await Magnesium.Current.PostAsync(dr))
                {
                    bloomFilter.Add(dr.Url);
                    return true;
                }
                return false;
            }
            return false;
        }

    }
}
