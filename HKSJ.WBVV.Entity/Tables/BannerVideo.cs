


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 头部推荐
    /// </summary>
    [Serializable]
    public class BannerVideo
    {
        #region 私有字段
        private Int32 _id;
        private Int64 _videoId;
        private Int32 _categoryId;
        private string _bannerImagePath;
        private string _bannerSmallImagePath;
        private Int32 _sortNum = 0;
        private Int32 _createManageId;
        private DateTime _createTime;
        private Int32? _updateManageId;
        private DateTime? _updateTime;
        private bool _state = false;
        #endregion
        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [Key]
        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 视频编号
        /// </summary>
        /// <returns></returns>
        public Int64 VideoId
        {
            get { return _videoId; }
            set { _videoId = value; }
        }

        /// <summary>
        /// 关联的一级分类
        /// </summary>
        /// <returns></returns>
        public Int32 CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        /// <summary>
        /// 大图片路径
        /// </summary>
        /// <returns></returns>
        public string BannerImagePath
        {
            get { return _bannerImagePath; }
            set { _bannerImagePath = value; }
        }

        /// <summary>
        /// 小图片路径
        /// </summary>
        public string BannerSmallImagePath
        {
            get { return _bannerSmallImagePath; }
            set { _bannerSmallImagePath = value; }
        }

        /// <summary>
        /// 排序编号
        /// </summary>
        /// <returns></returns>
        public Int32 SortNum
        {
            get { return _sortNum; }
            set { _sortNum = value; }
        }

        /// <summary>
        /// 创建管理员编号
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

        #endregion
    }
}