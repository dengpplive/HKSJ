


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 分类
    /// </summary>
    [Serializable]
    public class Category
    {
        #region 私有字段
        private Int32 _id;
        private string _name;
        private Int32 _pageSize = 10;
        private string _keyWord;
        private Int32 _parentId = 0;
        private Int32 _sortNum = 0;
        private string _locationPath;
        private bool _visible = true;
        private Int32 _createManageId;
        private DateTime _createTime;
        private Int32? _updateManageId;
        private DateTime? _updateTime;
        private bool _state = false;
        #endregion
        #region 属性
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Key]
        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 分类名称
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 显示条数(一级分类显示的是二级分类的条数，二级分类显示的是对应的视频的条数)
        /// </summary>
        /// <returns></returns>
        public Int32 PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
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
        /// 父级编号（0：树的根节点）
        /// </summary>
        /// <returns></returns>
        public Int32 ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
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
        /// 位置
        /// </summary>
        /// <returns></returns>
        public string LocationPath
        {
            get { return _locationPath; }
            set { _locationPath = value; }
        }

        /// <summary>
        /// 是否显示(0:隐藏1:显示)
        /// </summary>
        /// <returns></returns>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
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