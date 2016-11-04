using HKSJ.WBVV.Common.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    public class RecommendAndHotPlateVideoView
    {
        /// <summary>
        /// 热门
        /// </summary>
        public IList<IndexVideoView> HotPlateVideo { get; set; }
        /// <summary>
        /// 推荐
        /// </summary>
        public IList<IndexVideoView> RecommendPlateVideo { get; set; }
    }
    //首页上的视频数据 
    [Serializable]
    public class IndexVideoView
    {
        #region Fileds
        /// <summary>
        /// 视频小图片
        /// </summary>
        string _smallPicPath;

        /// <summary>
        /// 视频大图片
        /// </summary>
        string _bigPicPath;
        #endregion

        /// <summary>
        /// 视频编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        /// <returns></returns>
        public string Title { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        /// <returns></returns>
        public string About { get; set; }

        public string SmallPicturePath
        {
            get
            {
                return UrlHelper.QiniuPublicCombine(_smallPicPath);
            }

            set
            {
                _smallPicPath = value;
            }
        }
        /// <summary>
        /// 视频大图片
        /// </summary>
        public string BigPicturePath
        {
            get
            {
                return UrlHelper.QiniuPublicCombine(_bigPicPath);
            }

            set
            {
                _bigPicPath = value;
            }
        }
        /// <summary>
        /// 播放次数
        /// </summary>
        /// <returns></returns>
        public Int32 PlayCount { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public Int32 SortNum { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }

}
