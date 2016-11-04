


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 用户粉丝
    /// </summary>
    [Serializable]
    public class UserFans
    {
        #region 私有字段
        private Int32 _id ;
        private Int32 _subscribeUserId ;
        private Int32 _createUserId ;
        private DateTime _createTime ;
        private Int32? _updateUserId ;
        private DateTime? _updateTime ;
        private bool _state =false;
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
        /// 被关注的用户编号
        /// </summary>
        /// <returns></returns>
          public Int32 SubscribeUserId
        {
            get { return _subscribeUserId;}
            set { _subscribeUserId=value;}
        }
       
        /// <summary>
        /// 关注用户编号
        /// </summary>
        /// <returns></returns>
          public Int32 CreateUserId
        {
            get { return _createUserId;}
            set { _createUserId=value;}
        }
       
        /// <summary>
        /// 关注时间
        /// </summary>
        /// <returns></returns>
          public DateTime CreateTime
        {
            get { return _createTime;}
            set { _createTime=value;}
        }
       
        /// <summary>
        /// 取消关注用户编号
        /// </summary>
        /// <returns></returns>
          public Int32? UpdateUserId
        {
            get { return _updateUserId;}
            set { _updateUserId=value;}
        }
       
        /// <summary>
        /// 取消关注时间
        /// </summary>
        /// <returns></returns>
          public DateTime? UpdateTime
        {
            get { return _updateTime;}
            set { _updateTime=value;}
        }
       
        /// <summary>
        /// 状态(0.关注（默认）1.取消关注)
        /// </summary>
        /// <returns></returns>
          public bool State
        {
            get { return _state;}
            set { _state=value;}
        }
       
        #endregion
    }
}