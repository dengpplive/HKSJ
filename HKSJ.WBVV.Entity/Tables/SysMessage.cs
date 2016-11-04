


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 系统消息表
    /// </summary>
    [Serializable]
    public class SysMessage
    {
        #region 私有字段
        private Int32 _id ;
        private Int16 _sendType ;
        private Int16 _entityType ;
        private Int32 _entityId ;
        private string _toUserIds ;
        private string _content ;
        private DateTime _createTime ;
        private Int32 _createManageId ;
        private DateTime? _updateTime ;
        private Int32? _updateManagerId ;
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
        /// 用户发送类型(1:选中用户,2:选中用户类型3:全体用户（默认）)
        /// </summary>
        /// <returns></returns>
          public Int16 SendType
        {
            get { return _sendType;}
            set { _sendType=value;}
        }
       
        /// <summary>
        /// 消息通知类型（1:系统消息2:评论3:留言4:喜欢）默认1
        /// </summary>
        /// <returns></returns>
          public Int16 EntityType
        {
            get { return _entityType;}
            set { _entityType=value;}
        }
       
        /// <summary>
        /// 消息通知外键编号（1:0;2:评论编号3:留言编号4:收藏编号）默认0
        /// </summary>
        /// <returns></returns>
          public Int32 EntityId
        {
            get { return _entityId;}
            set { _entityId=value;}
        }
       
        /// <summary>
        /// 接受用户编号，按|拼接用户编号（1|23|34|45）
        /// </summary>
        /// <returns></returns>
          public string ToUserIds
        {
            get { return _toUserIds;}
            set { _toUserIds=value;}
        }
       
        /// <summary>
        /// 系统通知内容
        /// </summary>
        /// <returns></returns>
          public string Content
        {
            get { return _content;}
            set { _content=value;}
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
        /// 创建人编号
        /// </summary>
        /// <returns></returns>
          public Int32 CreateManageId
        {
            get { return _createManageId;}
            set { _createManageId=value;}
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
        /// 修改人编号
        /// </summary>
        /// <returns></returns>
          public Int32? UpdateManagerId
        {
            get { return _updateManagerId;}
            set { _updateManagerId=value;}
        }
       
        /// <summary>
        /// 状态(1.禁用 0.启用(默认))
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