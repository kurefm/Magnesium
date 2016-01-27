using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.DataBase.XMusic
{
    /// <summary>
    /// 虾米音乐数据模型的基类，约定基本的属性和方法
    /// </summary>
    public abstract class XModleBase
    {
        #region Property

        /// <summary>
        /// 数据库中的ID
        /// </summary>
        public long? Id { get; set; } = null;
        /// <summary>
        /// 虾米上的数据ID
        /// </summary>
        public long XId { get; set; }
        /// <summary>
        /// 试听计数
        /// </summary>
        public long ListenCount { get; set; } = 0;
        /// <summary>
        /// 分享计数
        /// </summary>
        public long ShareCount { get; set; } = 0;
        /// <summary>
        /// 评论计数
        /// </summary>
        public long CommentCount { get; set; } = 0;
        /// <summary>
        /// 记录最后更新时间
        /// </summary>
        public DateTime LastUpdate { get; set; } = DateTime.Now;

        #endregion

        #region Method

        public XModleBase(long xId)
        {
            XId = xId;
        }

        #endregion
    }
}
