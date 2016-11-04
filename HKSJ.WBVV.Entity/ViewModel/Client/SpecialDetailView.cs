using HKSJ.WBVV.Common.Resource;
using System;
using System.Collections.Generic;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 专辑详细视图
    /// </summary>
    [Serializable]
    public class SpecialDetailView
    {
        #region Fields

        /// <summary>
        /// Local veriable used to combine Small Pic Path the a Album.
        /// </summary>
        string _thumbnail;
        string _remark;

        #endregion

        /// <summary>
        /// 专辑编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 专辑标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 专辑缩略图
        /// </summary>
        public string Thumbnail
        {
            get
            {
                return UrlHelper.QiniuPublicCombine(_thumbnail);
            }

            set
            {
                _thumbnail = value;
            }
        }

        /// <summary>
        /// 简介
        /// </summary>
        public string Remark
        {
            get
            {
                return _remark;
            }

            set
            {
                _remark = value;
            }
        }
        /// <summary>
        /// 专辑下的视频数
        /// </summary>
        public int VideoCount { get; set; }

        /// <summary>
        /// 专辑下的评论数
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 专辑创建者
        /// </summary>
        public string CreateUserNick { get; set; }

        /// <summary>
        /// 专辑创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 专辑创建时间  用于排序
        /// </summary>
        public DateTime OrderCreateTime { get; set; }

        /// <summary>
        /// 专辑修改者
        /// </summary>
        public string UpdateUserNick { get; set; }

        /// <summary>
        /// 专辑修改时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 专辑排序号
        /// </summary>
        public int SortNum { get; set; }

        /// <summary>
        /// 专辑下所有视频播放总数
        /// </summary>
        public int PlayCount { get; set; }

        /// <summary>
        /// 所有视频的播放总数
        /// </summary>
        public string FormatPlayCount
        {
            get
            {
                string formatNum = string.Format("{0:n}", PlayCount);
                return formatNum.Substring(0, formatNum.Length - 3);
            }
        }

        /// <summary>
        /// 专辑下所有视频当日播放总数
        /// </summary>
        public int TheDayPlayCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 专辑中视频集合
        /// </summary>
        public List<SpecialVideoView> SpecialVideoList { get; set; }


    }
}