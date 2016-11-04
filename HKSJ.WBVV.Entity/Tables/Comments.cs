


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 评论表
    /// </summary>
    [Serializable]
    public class Comments
    {
        #region 私有字段
        private Int32 _id;
        private Int32 _fromUserId;
        private Int32 _toUserId;
        private Int16 _entityType;
        private Int32 _entityId;
        private string _content;
        private Int32 _praisesNum;
        private Int32 _replyNum;
        private Int32 _parentId;
        private string _localPath;
        private Int32 _createUserId;
        private DateTime _createTime;
        private Int32? _updateUserId;
        private DateTime? _updateTime;
        private Int16 _state;
        private Int32 _reportCount;
        #endregion
        #region 属性
        /// <summary>
        /// 评论编号
        /// </summary>
        /// <returns></returns>
        [Key]
        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 发送消息用户编号
        /// </summary>
        /// <returns></returns>
        public Int32 FromUserId
        {
            get { return _fromUserId; }
            set { _fromUserId = value; }
        }

        /// <summary>
        /// 接受消息用户编号(0表示没有接受消息用户)
        /// </summary>
        /// <returns></returns>
        public Int32 ToUserId
        {
            get { return _toUserId; }
            set { _toUserId = value; }
        }

        /// <summary>
        /// 评论类型（1 用户空间, 2 视频）默认1用户空间
        /// </summary>
        /// <returns></returns>
        public Int16 EntityType
        {
            get { return _entityType; }
            set { _entityType = value; }
        }

        /// <summary>
        /// 评论类型的编号（EntityType=1为用户空间用户编号；EntityType=2为视频编号）
        /// </summary>
        /// <returns></returns>
        public Int32 EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        /// <summary>
        /// 评论内容
        /// </summary>
        /// <returns></returns>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>
        /// 点赞次数
        /// </summary>
        /// <returns></returns>
        public Int32 PraisesNum
        {
            get { return _praisesNum; }
            set { _praisesNum = value; }
        }

        /// <summary>
        /// 回复次数
        /// </summary>
        /// <returns></returns>
        public Int32 ReplyNum
        {
            get { return _replyNum; }
            set { _replyNum = value; }
        }

        /// <summary>
        /// 父评论编号
        /// </summary>
        /// <returns></returns>
        public Int32 ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        /// <summary>
        /// 所在位置
        /// </summary>
        /// <returns></returns>
        public string LocalPath
        {
            get { return _localPath; }
            set { _localPath = value; }
        }

        /// <summary>
        /// 创建用户编号
        /// </summary>
        /// <returns></returns>
        public Int32 CreateUserId
        {
            get { return _createUserId; }
            set { _createUserId = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        /// <summary>
        /// 修改用户编号
        /// </summary>
        /// <returns></returns>
        public Int32? UpdateUserId
        {
            get { return _updateUserId; }
            set { _updateUserId = value; }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }

        /// <summary>
        /// 状态(-2:审核不通过；-1:删除；0:待审核；1:审核通过；)  默认0
        /// </summary>
        /// <returns></returns>
        public Int16 State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 举报次数
        /// </summary>
        /// <returns></returns>
        public Int32 ReportCount
        {
            get { return _reportCount; }
            set { _reportCount = value; }
        }

        #endregion
    }
}