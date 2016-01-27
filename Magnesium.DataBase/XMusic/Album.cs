using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.DataBase.XMusic
{
    public class Album : XModleBase
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        public string Subtitle { get; set; }
        /// <summary>
        /// 专辑封面路径
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public float Rating { get; set; } = 0;
        /// <summary>
        /// 专辑艺术家ID
        /// </summary>
        public long XArtistId { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 发行商
        /// </summary>
        public string Publisher { get; set; }
        /// <summary>
        /// 发行日期
        /// </summary>
        public DateTime PublishDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 媒体类型
        /// </summary>
        public string MediaType { get; set; }
        /// <summary>
        /// 流派（风格）
        /// </summary>
        public string Genre { get; set; }
        /// <summary>
        /// 收藏计数
        /// </summary>
        public long LikeCount { get; set; } = 0;
        /// <summary>
        /// 专辑的CD数
        /// </summary>
        public int DiscCount { get; set; } = 1;
        /// <summary>
        /// 专辑介绍
        /// </summary>
        public string Introduction { get; set; }

        public Album(long xId, string title, long xArtistId) : base(xId)
        {
            Title = title;
            XArtistId = xArtistId;
        }
    }
}
