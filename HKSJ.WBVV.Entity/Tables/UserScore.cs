


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 用户积分表
    /// </summary>
    [Serializable]
    public class UserScore
    {
        #region 私有字段
        private Int32 _id ;
        private Int32 _score ;
        private string _scoreRluIeName ;
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
        /// 积分数量
        /// </summary>
        /// <returns></returns>
          public Int32 Score
        {
            get { return _score;}
            set { _score=value;}
        }
       
        /// <summary>
        /// 积分规则名称
        /// </summary>
        /// <returns></returns>
          public string ScoreRluIeName
        {
            get { return _scoreRluIeName;}
            set { _scoreRluIeName=value;}
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
        /// 状态（0可用1禁用）默认0
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