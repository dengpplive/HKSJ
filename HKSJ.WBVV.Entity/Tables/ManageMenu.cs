


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
    public class ManageMenu
    {
        #region 私有字段
        private Int32 _id ;
        private string _menuName ;
        private string _icon ;
        private bool _state =false;
        private string _descc ;
        private string _url ;
        private string _parentMenu ;
        private bool _leaf =false;
        private Int32? _orderId ;
        private Int32 _createManageId ;
        private DateTime _createTime ;
        private Int32? _updateManageId ;
        private DateTime? _updateTime ;
        #endregion
        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [Key]
        public Int32 Id
        {
            get { return _id;}
            set { _id=value;}
        }
       
        /// <summary>
        /// 管理员菜单名
        /// </summary>
        /// <returns></returns>
          public string MenuName
        {
            get { return _menuName;}
            set { _menuName=value;}
        }
       
        /// <summary>
        /// 图片URL
        /// </summary>
        /// <returns></returns>
          public string Icon
        {
            get { return _icon;}
            set { _icon=value;}
        }
       
        /// <summary>
        /// 状态（1.删除0.可用（默认））
        /// </summary>
        /// <returns></returns>
          public bool State
        {
            get { return _state;}
            set { _state=value;}
        }
       
        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
          public string Descc
        {
            get { return _descc;}
            set { _descc=value;}
        }
       
        /// <summary>
        /// 菜单URL
        /// </summary>
        /// <returns></returns>
          public string Url
        {
            get { return _url;}
            set { _url=value;}
        }
       
        /// <summary>
        /// 父菜单
        /// </summary>
        /// <returns></returns>
          public string ParentMenu
        {
            get { return _parentMenu;}
            set { _parentMenu=value;}
        }
       
        /// <summary>
        /// 是否为目录(1.是  0.不是(默认))
        /// </summary>
        /// <returns></returns>
          public bool Leaf
        {
            get { return _leaf;}
            set { _leaf=value;}
        }
       
        /// <summary>
        /// 排序号
        /// </summary>
        /// <returns></returns>
          public Int32? OrderId
        {
            get { return _orderId;}
            set { _orderId=value;}
        }
       
        /// <summary>
        /// 创建管理员编号
        /// </summary>
        /// <returns></returns>
          public Int32 CreateManageId
        {
            get { return _createManageId;}
            set { _createManageId=value;}
        }
       
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
          public DateTime CreateTime
        {
            get { return _createTime;}
            set { _createTime=value;}
        }
       
        /// <summary>
        /// 修改管理员编号
        /// </summary>
        /// <returns></returns>
          public Int32? UpdateManageId
        {
            get { return _updateManageId;}
            set { _updateManageId=value;}
        }
       
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
          public DateTime? UpdateTime
        {
            get { return _updateTime;}
            set { _updateTime=value;}
        }
       
        #endregion
    }
}