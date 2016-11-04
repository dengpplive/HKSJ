


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 管理员表
    /// </summary>
    [Serializable]
    public class Manage
    {
        #region 私有字段
        private Int32 _id ;
        private string _loginName ;
        private string _password ;
        private Int32? _sex ;
        private string _email ;
        private Int32? _roleId ;
        private string _nickName ;
        private bool _state =false;
        private DateTime _createTime ;
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
        /// 登录账户
        /// </summary>
        /// <returns></returns>
          public string LoginName
        {
            get { return _loginName;}
            set { _loginName=value;}
        }
       
        /// <summary>
        /// 密码
        /// </summary>
        /// <returns></returns>
          public string Password
        {
            get { return _password;}
            set { _password=value;}
        }
       
        /// <summary>
        /// 性别
        /// </summary>
        /// <returns></returns>
          public Int32? Sex
        {
            get { return _sex;}
            set { _sex=value;}
        }
       
        /// <summary>
        /// 邮箱
        /// </summary>
        /// <returns></returns>
          public string Email
        {
            get { return _email;}
            set { _email=value;}
        }
       
        /// <summary>
        /// 角色表Id
        /// </summary>
        /// <returns></returns>
          public Int32? RoleId
        {
            get { return _roleId;}
            set { _roleId=value;}
        }
       
        /// <summary>
        /// 昵称
        /// </summary>
        /// <returns></returns>
          public string NickName
        {
            get { return _nickName;}
            set { _nickName=value;}
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
        /// 创建时间
        /// </summary>
        /// <returns></returns>
          public DateTime CreateTime
        {
            get { return _createTime;}
            set { _createTime=value;}
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