using Microsoft.VisualStudio.TestTools.UnitTesting;
using Magnesium.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Magnesium.Core.Base.Tests
{
    [TestClass()]
    public class BloomFilterTests
    {
        /// <summary>
        /// 此单元测试会测试各个数量级存入的错误率, 非常耗时
        /// </summary>
        [TestMethod()]
        [Timeout(TestTimeout.Infinite)]
        public void ParalleUseTest()
        {
            BaseParalleUselTest(1);
            BaseParalleUselTest(10);
            BaseParalleUselTest(100);
            BaseParalleUselTest(1000);
            BaseParalleUselTest(10000);
            BaseParalleUselTest(100000);
            BaseParalleUselTest(1000000);
            BaseParalleUselTest(10000000);
            //BaseParalleUselTest(100000000);
        }

        public void BaseParalleUselTest(int testNum)
        {
            var bf = new BloomFilter<string>(100000000, 0.001f);

            Parallel.For(0, testNum, i => bf.Add(GetTestString(i).ToString()));

            var failCount = 0;

            Parallel.For(0, testNum, i =>
            {
                if (!bf.Contains(GetTestString(i).ToString()))
                {
                    //Trace.WriteLine($"{i}不存在");
                    failCount += 1;
                }
            });
            Trace.WriteLine($"测试写入{testNum}个对象, 共有{failCount}个({(float)failCount / testNum:f6})对象不存在");
        }

        string GetTestString(int i)
        {
            //一段假设的URL
            return $"https://msdn.microsoft.com/zh-cn/library/hh{i}.aspx";
        }

        string GeneString(int baseNum, int length = 64)
        {
            Random random = new Random(baseNum);
            var data = new byte[length];
            random.NextBytes(data);
            var m = from b in data
                    select (b < ' ' || b > '~') ? '.' : '?';
            return new string(m.ToArray());
        }
    }
}