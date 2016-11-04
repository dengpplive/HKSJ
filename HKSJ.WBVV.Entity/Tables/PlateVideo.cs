


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 视频和板块中间表
    /// </summary>
    [Serializable]
    public class PlateVideo
    {
        #region 私有字段
        private Int32 _id;
        private Int64 _videoId;
        private Int32 _plateId;
        private Int32 _sortNum;
        private Int32 _createManageId;
        private DateTime _createTime;
        private Int32 _categoryId;
        private bool _isHot;
        private bool _isRecommend;
        private DateTime? _updateTime;
        private Int32? _updateManageId;
        private bool _state;
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
        /// 板块编号
        /// </summary>
        /// <returns></returns>
        public Int32 PlateId
        {
            get { return _plateId; }
            set { _plateId = value; }
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
        /// 创建人
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
        /// 分类编号(0:表示首页板块)
        /// </summary>
        /// <returns></returns>
        public Int32 CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        /// <summary>
        /// 是否热门(0:否1是)
        /// </summary>
        /// <returns></returns>
        public bool IsHot
        {
            get { return _isHot; }
            set { _isHot = value; }
        }

        /// <summary>
        /// 是否热门（0：否1是）
        /// </summary>
        /// <returns></returns>
        public bool IsRecommend
        {
            get { return _isRecommend; }
            set { _isRecommend = value; }
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
        /// 修改人
        /// </summary>
        /// <returns></returns>
        public Int32? UpdateManageId
        {
            get { return _updateManageId; }
            set { _updateManageId = value; }
        }

        /// <summary>
        /// 状态（1:删除0：可用）
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