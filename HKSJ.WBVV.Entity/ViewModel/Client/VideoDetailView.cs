using HKSJ.WBVV.Common.Resource;
using System;
using System.Collections.Generic;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 视频详细视图
    /// </summary>
    [Serializable]
    public class VideoDetailView
    {
        #region Fields

        /// <summary>
        /// Local veriable used to combine Small Pic Path the a video.
        /// </summary>
        string _smallPicPath;

        /// <summary>
        /// Local veriable used to combine Big Pic Path the a video.
        /// </summary>
        string _bigPicPath;

        /// <summary>
        /// 用户头像
        /// </summary>
        string _picture;
        #endregion

        /// <summary>
        /// 视频编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 视频标签
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// 类型分类
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 类型ID
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// 地区和年代的集合
        /// </summary>
        public IDictionary<string, string> DictionaryViews { get; set; }
        /// <summary>
        /// 视频标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 播放路径
        /// </summary>
        public string VideoPath { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Picture
        {
            get
            {
                if (_picture == null || _picture.IndexOf("Content/images", System.StringComparison.Ordinal) > -1)
                    return _picture;
                return UrlHelper.QiniuPublicCombine(_picture);
            }

            set
            {
                _picture = value;
            }
        }
        /// <summary>
        /// 评论次数
        /// </summary>
        /// <returns></returns>
        public int CommentCount { get; set; }
        /// <summary>
        /// 播放次数
        /// </summary>
        public int PlayCount { get; set; }
        /// <summary>
        /// 收藏次数
        /// </summary>
        public int CollectionCount { get; set; }
        /// <summary>
        /// 当前登录用户是否已经收藏该视频
        /// </summary>
        public bool IsCollected { get; set; }
        /// <summary>
        /// 赞的次数
        /// </summary>
        public int PraiseCount { get; set; }
        /// <summary>
        /// 打赏播币总数
        /// </summary>
        public Int64 RewardCount { get; set; }
        /// <summary>
        /// 踩的次数
        /// </summary>
        public int BadCount { get; set; }
        /// <summary>
        /// 主演
        /// </summary>
        /// <returns></returns>
        public string Starring { get; set; }

        /// <summary>
        /// 导演
        /// </summary>
        public string Director { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string About { get; set; }
        /// <summary>
        /// 视频小图片
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
        ///视频来源（0：后台管理员上传(CreateManageId为后台管理员编号)1:前台用户上传(CreateManageId为前台用户编号)）
        /// </summary>
        public int VideoSource { get; set; }


    }
}
