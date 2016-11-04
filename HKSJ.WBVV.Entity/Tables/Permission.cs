


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 后台权限表
    /// </summary>
    [Serializable]
    public class Permission
    {
        #region 私有字段
        private Int32 _id ;
        private string _permissionName ;
        private string _descc ;
        private bool _state =false;
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
        /// 权限名称
        /// </summary>
        /// <returns></returns>
          public string PermissionName
        {
            get { return _permissionName;}
            set { _permissionName=value;}
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
        /// 状态（1.删除0.可用（默认））
        /// </summary>
        /// <returns></returns>
          public bool State
        {
            get { return _state;}
            set { _state=value;}
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