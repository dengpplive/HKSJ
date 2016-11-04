


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
    public class MessageRead
    {
        #region 私有字段
        private Int32 _id ;
        private Int32 _userId ;
        private Int32 _messageId ;
        private Int16 _state;
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
        /// 用户编号
        /// </summary>
        /// <returns></returns>
          public Int32 UserId
        {
            get { return _userId;}
            set { _userId=value;}
        }
       
        /// <summary>
        /// 消息编号
        /// </summary>
        /// <returns></returns>
          public Int32 MessageId
        {
            get { return _messageId;}
            set { _messageId=value;}
        }
       
        /// <summary>
        /// 系统消息状态（0:默认状态已读 1:已删除）
        /// </summary>
        /// <returns></returns>
        public Int16 State
        {
            get { return _state; }
            set { _state = value; }
        }

        #endregion
    }
}