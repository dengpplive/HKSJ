


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 举报表
    /// </summary>
    [Serializable]
    public class UserReport
    {
        #region 私有字段
        private Int32 _id ;
        private Int16 _entityType ;
        private Int32 _entityId ;
        private Int32 _userId ;
        private Int32 _createUserId ;
        private DateTime _createTime ;
        private Int32? _updateUserId ;
        private DateTime? _updateTime ;
        private bool _state ;
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
        /// 举报类型（1 评论, 2 视频）默认1评论
        /// </summary>
        /// <returns></returns>
          public Int16 EntityType
        {
            get { return _entityType;}
            set { _entityType=value;}
        }
       
        /// <summary>
        /// 举报类型的编号（EntityType=1为评论编号；EntityType=2为视频编号）
        /// </summary>
        /// <returns></returns>
          public Int32 EntityId
        {
            get { return _entityId;}
            set { _entityId=value;}
        }
       
        /// <summary>
        /// 举报人
        /// </summary>
        /// <returns></returns>
          public Int32 UserId
        {
            get { return _userId;}
            set { _userId=value;}
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
          public DateTime CreateTime
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
       
        /// <summary>
        /// 状态(1.取消举报(0.举报(默认))
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