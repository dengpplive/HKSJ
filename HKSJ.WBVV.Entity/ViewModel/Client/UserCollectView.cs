using HKSJ.WBVV.Common.Resource;
using System;
using System.Collections.Generic;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 用户收藏
    /// </summary>
    [Serializable]
    public class UserCollectView
    {
        #region Fields

        /// <summary>
        /// Local veriable used to combine Small Pic Path the a video.
        /// </summary>
        string _smallPicPath;

        #endregion
        
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 视频编号
        /// </summary>
        public int VideoId { get; set; }

        /// <summary>
        /// 小图片路径
        /// </summary>
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
        /// 收藏时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 视频标题
        /// </summary>
        public string Title { get; set; }

    }

    public class UserCollectResult
    {
        public IList<UserCollectView> UserCollectViews { get; set; }
        public int TotalCount { get; set; }
    }
}