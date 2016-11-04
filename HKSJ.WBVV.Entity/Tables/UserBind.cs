


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
    public class UserBind
    {
        #region 私有字段
        private Int32 _id;
        private Int32 _userId;
        private string _typeCode;
        private string _relatedId;
        private Int32 _createUserId;
        private DateTime _createTime;
        private Int32 _updateUserId;
        private DateTime _updateTime;
        private bool _state;
        #endregion
        #region 属性
        /// <summary>
        /// Id
        /// </summary>
        /// <returns></returns>
        [Key]
        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        /// <returns></returns>
        public Int32 UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// 第三方类型编码
        /// </summary>
        /// <returns></returns>
        public string TypeCode
        {
            get { return _typeCode; }
            set { _typeCode = value; }
        }

        /// <summary>
        /// 第三方身份标识
        /// </summary>
        /// <returns></returns>
        public string RelatedId
        {
            get { return _relatedId; }
            set { _relatedId = value; }
        }

        /// <summary>
        /// 创建者ID
        /// </summary>
        /// <returns></returns>
        public Int32 CreateUserId
        {
            get { return _createUserId; }
            set { _createUserId = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        /// <returns></returns>
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        /// <summary>
        /// 更新者ID
        /// </summary>
        /// <returns></returns>
        public Int32 UpdateUserId
        {
            get { return _updateUserId; }
            set { _updateUserId = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        /// <returns></returns>
        public DateTime UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }

        /// <summary>
        /// 绑定状态
        /// </summary>
        /// <returns></returns>
        public bool State
        {
            get { return _state; }
            set { _state = value; }
        }

        #endregion
    }
}