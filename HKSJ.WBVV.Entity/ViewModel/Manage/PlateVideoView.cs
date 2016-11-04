using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Manage
{
    /// <summary>
    /// 板块视频
    /// </summary>
    [Serializable]
    public class PlateVideoView
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 板块编号
        /// </summary>
        public int PlateId { get; set; }
        /// <summary>
        /// 板块名称
        /// </summary>
        public string PlateName { get; set; }
        /// <summary>
        /// 板块视频排序
        /// </summary>
        public int SortNum { get; set; }
        /// <summary>
        /// 视频编号
        /// </summary>
        public long VideoId { get; set; }
        /// <summary>
        /// 视频标题
        /// </summary>
        public string VideoTitle { get; set; }
        /// <summary>
        /// 视频缩略图
        /// </summary>
        public string VideoImage { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 是否是热门(1=是 0=否)
        /// </summary>
        public string IsHot { get; set; }
         /// <summary>
        /// 是否是推荐(1=是 0=否)
        /// </summary>
        public string IsRecommend { get; set; }
    }
}
