using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        /// <param name="value">要测试的字符串。</param>
        /// <returns>如果 value 参数为 null 或 System.String.Empty，或者如果 value 仅由空白字符组成，则为 true。</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        /// <summary>
        /// 指示指定的字符串是 null 还是 System.String.Empty 字符串。
        /// </summary>
        /// <param name="value">要测试的字符串。</param>
        /// <returns>如果 value 参数为 null 或空字符串 ("")，则为 true；否则为 false。</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 将指定字符串中的所有字符编码为一个字节序列。
        /// </summary>
        /// <param name="s">包含要编码的字符的字符串。</param>
        /// <param name="name">首选编码的代码页名称。</param>
        /// <returns>一个字节数组，包含对指定的字符集进行编码的结果。</returns>
        public static byte[] Encode(this string s, string name = "UTF-8")
        {
            return Encoding.GetEncoding(name).GetBytes(s);
        }
    }
}
