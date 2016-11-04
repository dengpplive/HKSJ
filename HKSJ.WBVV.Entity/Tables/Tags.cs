


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Tags
    {
        #region 私有字段
        private Int32 _id;
        private string _keyWord;
        private string _name;
        private Int32 _sortNum;
        private Int32 _createUserId;
        private DateTime _createTime;
        private Int32? _updateUserId;
        private DateTime? _updateTime;
        private bool _state;
        private Int32? _categoryId;
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
        /// 关键字
        /// </summary>
        /// <returns></returns>
        public string KeyWord
        {
            get { return _keyWord; }
            set { _keyWord = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 排序数量
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
        public Int32 CreateUserId
        {
            get { return _createUserId; }
            set { _createUserId = value; }
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
        /// 修改人
        /// </summary>
        /// <returns></returns>
        public Int32? UpdateUserId
        {
            get { return _updateUserId; }
            set { _updateUserId = value; }
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
        /// 状态（0：在用1：删除）
        /// </summary>
        /// <returns></returns>
        public bool State
        {
            get { return _state; }
            set { _state = value; }
        }
        /// <summary>
        ///所属分类Id
        /// </summary>
        /// <returns></returns>
        public Int32? CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }
        #endregion
    }
}