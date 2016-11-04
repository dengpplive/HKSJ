


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
    public class UserLevel
    {
        #region 私有字段
        private Int32 _id ;
        private string _levelName ;
        private Int32 _levelStart ;
        private Int32 _levelEnd ;
        private string _levelIcon ;
        private Int32 _createUserId ;
        private DateTime _createTime ;
        private Int32? _updateUserId ;
        private DateTime? _updateTime ;
        private bool _state ;
        #endregion
        #region 属性
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Key]
        public Int32 Id
        {
            get { return _id;}
            set { _id=value;}
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
          public string LevelName
        {
            get { return _levelName;}
            set { _levelName=value;}
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
          public Int32 LevelStart
        {
            get { return _levelStart;}
            set { _levelStart=value;}
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
          public Int32 LevelEnd
        {
            get { return _levelEnd;}
            set { _levelEnd=value;}
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
          public string LevelIcon
        {
            get { return _levelIcon;}
            set { _levelIcon=value;}
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
          public Int32 CreateUserId
        {
            get { return _createUserId;}
            set { _createUserId=value;}
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
          public DateTime CreateTime
        {
            get { return _createTime;}
            set { _createTime=value;}
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
          public Int32? UpdateUserId
        {
            get { return _updateUserId;}
            set { _updateUserId=value;}
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
          public DateTime? UpdateTime
        {
            get { return _updateTime;}
            set { _updateTime=value;}
        }
       
        /// <summary>
        /// 
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