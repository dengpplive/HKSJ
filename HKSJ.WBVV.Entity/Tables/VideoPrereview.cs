


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 审片
    /// </summary>
    [Serializable]
    public class VideoPrereview
    {
        #region 私有字段
        private Int32 _id ;
        private Int32 _videoId ;
        private Int32 _userId ;
        private Int16 _reviewState ;
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
        /// 审核视频编号
        /// </summary>
        /// <returns></returns>
          public Int32 VideoId
        {
            get { return _videoId;}
            set { _videoId=value;}
        }
       
        /// <summary>
        /// 审核人编号
        /// </summary>
        /// <returns></returns>
          public Int32 UserId
        {
            get { return _userId;}
            set { _userId=value;}
        }
       
        /// <summary>
        /// 审核状态（1：必火2：差评）默认1
        /// </summary>
        /// <returns></returns>
          public Int16 ReviewState
        {
            get { return _reviewState;}
            set { _reviewState=value;}
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
        /// 状态（0：可用1：禁用）默认0
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