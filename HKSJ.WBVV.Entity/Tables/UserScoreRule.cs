


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
    public class UserScoreRule
    {
        #region 私有字段
        private Int32 _id ;
        private string _name ;
        private Int32 _score ;
        private Int32 _limitScore ;
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
          public string Name
        {
            get { return _name;}
            set { _name=value;}
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
          public Int32 Score
        {
            get { return _score;}
            set { _score=value;}
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
          public Int32 LimitScore
        {
            get { return _limitScore;}
            set { _limitScore=value;}
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