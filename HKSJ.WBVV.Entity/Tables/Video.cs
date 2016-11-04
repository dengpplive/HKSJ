


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 视频
    /// </summary>
    [Serializable]
    public class Video
    {
        #region 私有字段
        private Int64 _id;
        private Int32 _categoryId;
        private string _title;
        private string _filter;
        private string _tags;
        private string _about;
        private Int32 _playCount;
        private Int32 _commentCount;
        private Int32 _praiseCount;
        private Int32 _badCount;
        private Int32 _collectionCount;
        private bool _isHot;
        private bool _isOfficial;
        private bool _isRecommend;
        private bool _videoSource;
        private string _videoPath;
        private string _smallPicturePath;
        private string _bigPicturePath;
        private Int16 _copyright;
        private bool _isPublic;
        private string _videoRosella;
        private Int32 _sortNum;
        private string _content;
        private Int32? _timeLength;
        private DateTime? _releaseTime;
        private string _starring;
        private string _director;
        private Int32 _createManageId;
        private DateTime _createTime;
        private Int32? _updateManageId;
        private DateTime? _updateTime;
        private bool _state;
        private Int64 _rewardCount;
        private string _persistentId;
        private string _hashCode;
        private Int16 _videoState;
        private Int32 _reportCount;
        private string _downloadPath;
        #endregion
        #region 属性
        /// <summary>
        /// 视频编号
        /// </summary>
        /// <returns></returns>
        [Key]
        public Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 分类编号
        /// </summary>
        /// <returns></returns>
        public Int32 CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        /// <returns></returns>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// 过滤条件（存放方式【字典编号:字典节点编号;字典编号:字典节点编号;】）
        /// </summary>
        /// <returns></returns>
        public string Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        /// <summary>
        /// 标签(标签1|标签2|标签3|标签4|标签5)
        /// </summary>
        /// <returns></returns>
        public string Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }

        /// <summary>
        /// 简介
        /// </summary>
        /// <returns></returns>
        public string About
        {
            get { return _about; }
            set { _about = value; }
        }

        /// <summary>
        /// 播放次数
        /// </summary>
        /// <returns></returns>
        public Int32 PlayCount
        {
            get { return _playCount; }
            set { _playCount = value; }
        }

        /// <summary>
        /// 评论次数
        /// </summary>
        /// <returns></returns>
        public Int32 CommentCount
        {
            get { return _commentCount; }
            set { _commentCount = value; }
        }

        /// <summary>
        /// 赞的次数
        /// </summary>
        /// <returns></returns>
        public Int32 PraiseCount
        {
            get { return _praiseCount; }
            set { _praiseCount = value; }
        }

        /// <summary>
        /// 踩的次数
        /// </summary>
        /// <returns></returns>
        public Int32 BadCount
        {
            get { return _badCount; }
            set { _badCount = value; }
        }

        /// <summary>
        /// 收藏次数
        /// </summary>
        /// <returns></returns>
        public Int32 CollectionCount
        {
            get { return _collectionCount; }
            set { _collectionCount = value; }
        }

        /// <summary>
        /// 是否热门(0.否1.是)
        /// </summary>
        /// <returns></returns>
        public bool IsHot
        {
            get { return _isHot; }
            set { _isHot = value; }
        }

        /// <summary>
        /// 是否是官方(0:不是,1:是)
        /// </summary>
        /// <returns></returns>
        public bool IsOfficial
        {
            get { return _isOfficial; }
            set { _isOfficial = value; }
        }

        /// <summary>
        /// 是否推荐(0.否1.是)
        /// </summary>
        /// <returns></returns>
        public bool IsRecommend
        {
            get { return _isRecommend; }
            set { _isRecommend = value; }
        }

        /// <summary>
        /// 视频来源（0：后台管理员上传(CreateManageId为后台管理员编号)1:前台用户上传(CreateManageId为前台用户编号)）
        /// </summary>
        /// <returns></returns>
        public bool VideoSource
        {
            get { return _videoSource; }
            set { _videoSource = value; }
        }

        /// <summary>
        /// 视频路径
        /// </summary>
        /// <returns></returns>
        public string VideoPath
        {
            get { return _videoPath; }
            set { _videoPath = value; }
        }

        /// <summary>
        /// 小图片路径
        /// </summary>
        /// <returns></returns>
        public string SmallPicturePath
        {
            get { return _smallPicturePath; }
            set { _smallPicturePath = value; }
        }

        /// <summary>
        /// 大图片路径
        /// </summary>
        /// <returns></returns>
        public string BigPicturePath
        {
            get { return _bigPicturePath; }
            set { _bigPicturePath = value; }
        }

        /// <summary>
        /// 版权(1:原创2:转载)
        /// </summary>
        /// <returns></returns>
        public Int16 Copyright
        {
            get { return _copyright; }
            set { _copyright = value; }
        }

        /// <summary>
        /// 是否公开（1.公开0.保密）
        /// </summary>
        /// <returns></returns>
        public bool IsPublic
        {
            get { return _isPublic; }
            set { _isPublic = value; }
        }

        /// <summary>
        /// 视频原名
        /// </summary>
        /// <returns></returns>
        public string VideoRosella
        {
            get { return _videoRosella; }
            set { _videoRosella = value; }
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        /// <returns></returns>
        public Int32 SortNum
        {
            get { return _sortNum; }
            set { _sortNum = value; }
        }

        /// <summary>
        /// 详细介绍
        /// </summary>
        /// <returns></returns>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>
        /// 时间长度
        /// </summary>
        /// <returns></returns>
        public Int32? TimeLength
        {
            get { return _timeLength; }
            set { _timeLength = value; }
        }

        /// <summary>
        /// 上映时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ReleaseTime
        {
            get { return _releaseTime; }
            set { _releaseTime = value; }
        }

        /// <summary>
        /// 主演
        /// </summary>
        /// <returns></returns>
        public string Starring
        {
            get { return _starring; }
            set { _starring = value; }
        }

        /// <summary>
        /// 导演
        /// </summary>
        /// <returns></returns>
        public string Director
        {
            get { return _director; }
            set { _director = value; }
        }

        /// <summary>
        /// 创建者编号
        /// </summary>
        /// <returns></returns>
        public Int32 CreateManageId
        {
            get { return _createManageId; }
            set { _createManageId = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        /// <summary>
        /// 修改管理员编号
        /// </summary>
        /// <returns></returns>
        public Int32? UpdateManageId
        {
            get { return _updateManageId; }
            set { _updateManageId = value; }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }

        /// <summary>
        /// 状态（1.删除0.可用（默认））
        /// </summary>
        /// <returns></returns>
        public bool State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 视频打赏播币数量
        /// </summary>
        /// <returns></returns>
        public Int64 RewardCount
        {
            get { return _rewardCount; }
            set { _rewardCount = value; }
        }

        /// <summary>
        /// 预处理ID
        /// </summary>
        /// <returns></returns>
        public string PersistentId
        {
            get { return _persistentId; }
            set { _persistentId = value; }
        }

        /// <summary>
        /// 视频的HashCode
        /// </summary>
        /// <returns></returns>
        public string HashCode
        {
            get { return _hashCode; }
            set { _hashCode = value; }
        }

        /// <summary>
        /// 0：转码中，1：转码失败，2：审核中，3：审核通过，4：审核不通过
        /// </summary>
        /// <returns></returns>
        public Int16 VideoState
        {
            get { return _videoState; }
            set { _videoState = value; }
        }

        /// <summary>
        /// 举报次数
        /// </summary>
        /// <returns></returns>
        public Int32 ReportCount
        {
            get { return _reportCount; }
            set { _reportCount = value; }
        }

        /// <summary>
        /// 下载视频
        /// </summary>
        /// <returns></returns>
        public string DownloadPath
        {
            get { return _downloadPath; }
            set { _downloadPath = value; }
        }

        #endregion
    }
}