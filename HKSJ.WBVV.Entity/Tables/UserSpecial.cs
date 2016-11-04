


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 用户专辑
    /// </summary>
    [Serializable]
    public class UserSpecial
    {
        #region 私有字段
        private Int32 _id;
        private Int32 _createUserId;
        private DateTime _createTime;
        private Int32? _updateUserId;
        private DateTime? _updateTime;
        private bool _state;
        private string _title;
        private string _remark;
        private string _tag;
        private Int32? _categoryId;
        private string _image;
        private bool _isRecommend;
        private Int32 _sortNum;
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
        /// 创建专辑用户编号
        /// </summary>
        /// <returns></returns>
        public Int32 CreateUserId
        {
            get { return _createUserId; }
            set { _createUserId = value; }
        }

        /// <summary>
        /// 创建专辑时间
        /// </summary>
        /// <returns></returns>
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        /// <summary>
        /// 修改专辑用户编号
        /// </summary>
        /// <returns></returns>
        public Int32? UpdateUserId
        {
            get { return _updateUserId; }
            set { _updateUserId = value; }
        }

        /// <summary>
        /// 修改专辑时间
        /// </summary>
        /// <returns></returns>
        public DateTime? UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }

        /// <summary>
        /// 状态（0:启用(默认）1:删除）
        /// </summary>
        /// <returns></returns>
        public bool State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 专辑标题
        /// </summary>
        /// <returns></returns>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// 专辑简介
        /// </summary>
        /// <returns></returns>
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        /// <summary>
        /// 标签(标签1|标签2|标签3|标签4|标签5)
        /// </summary>
        /// <returns></returns>
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        /// 专辑分类
        /// </summary>
        /// <returns></returns>
        public Int32? CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        /// <summary>
        /// 专辑图片
        /// </summary>
        /// <returns></returns>
        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsRecommend
        {
            get { return _isRecommend; }
            set { _isRecommend = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int32 SortNum
        {
            get { return _sortNum; }
            set { _sortNum = value; }
        }

        #endregion
    }
}