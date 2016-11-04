


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 用户等级表
    /// </summary>
    [Serializable]
    public class Level
    {
        #region 私有字段
        private Int32 _id ;
        private string _levelName ;
        private Int32? _levelStart ;
        private Int32? _levelEnd ;
        private string _levelIcon ;
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
        /// 等级名称
        /// </summary>
        /// <returns></returns>
          public string LevelName
        {
            get { return _levelName;}
            set { _levelName=value;}
        }
       
        /// <summary>
        /// 数量区间-头
        /// </summary>
        /// <returns></returns>
          public Int32? LevelStart
        {
            get { return _levelStart;}
            set { _levelStart=value;}
        }
       
        /// <summary>
        /// 数量区间-尾
        /// </summary>
        /// <returns></returns>
          public Int32? LevelEnd
        {
            get { return _levelEnd;}
            set { _levelEnd=value;}
        }
       
        /// <summary>
        /// 图标
        /// </summary>
        /// <returns></returns>
          public string LevelIcon
        {
            get { return _levelIcon;}
            set { _levelIcon=value;}
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