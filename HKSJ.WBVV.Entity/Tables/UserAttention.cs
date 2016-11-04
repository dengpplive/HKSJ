


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 用户关注表
    /// </summary>
    [Serializable]
    public class UserAttention
    {
        #region 私有字段
        private Int32 _id ;
        private Int32? _starUserId ;
        private Int32? _followerUserId ;
        private Int32 _createUserId ;
        private DateTime? _createTime ;
        private Int32? _updateUserId ;
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
        /// 用户业务编号(明星)
        /// </summary>
        /// <returns></returns>
          public Int32? StarUserId
        {
            get { return _starUserId;}
            set { _starUserId=value;}
        }
       
        /// <summary>
        /// 用户业务编号(粉丝)
        /// </summary>
        /// <returns></returns>
          public Int32? FollowerUserId
        {
            get { return _followerUserId;}
            set { _followerUserId=value;}
        }
       
        /// <summary>
        /// 创建用户编号
        /// </summary>
        /// <returns></returns>
          public Int32 CreateUserId
        {
            get { return _createUserId;}
            set { _createUserId=value;}
        }
       
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
          public DateTime? CreateTime
        {
            get { return _createTime;}
            set { _createTime=value;}
        }
       
        /// <summary>
        /// 修改用户编号
        /// </summary>
        /// <returns></returns>
          public Int32? UpdateUserId
        {
            get { return _updateUserId;}
            set { _updateUserId=value;}
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