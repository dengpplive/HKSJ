


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 字典
    /// </summary>
    [Serializable]
    public class Dictionary
    {
        #region 私有字段
        private Int32 _id;
        private Int32 _categoryId;
        private string _keyWord;
        private string _name;
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
        /// 分类编号
        /// </summary>
        /// <returns></returns>
        public Int32 CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
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
        /// 字典名称
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 排序
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