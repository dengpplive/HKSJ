using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Entity.Response.App;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 我的消息视图
    /// </summary>
    public class MessageView
    {
        /// <summary>
        /// Header的消息（最近的消息信息）
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// 消息类型集合
        /// </summary>
        public IList<MessageType> MessageTypes { get; set; }

        /// <summary>
        /// 选中类型的数据列表
        /// </summary>
        public PageResult PageResult { get; set; }

    }

    /// <summary>
    /// 消息类型视图
    /// </summary>
    public class MessageType
    {
        /// <summary>
        /// 消息类型编号
        /// </summary>
        public int MessageTypeId { get; set; }

        /// <summary>
        /// 消息类型名称
        /// </summary>
        public string MessageTypeName { get; set; }

        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int UnreadMessageCount { get; set; }
    }

    /// <summary>
    /// 我的消息--系统消息视图
    /// </summary>
    public class SystemMessageView
    {
        /// <summary>
        /// 消息编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }
    /// <summary>
    /// 我的消息--评论视图
    /// </summary>
    public class ViewCommentView
    {
        /// <summary>
        /// 消息编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 评论编号
        /// </summary>
        public int EntityId { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 发消息的人
        /// </summary>
        public UserSimpleView FromUser { get; set; }
        /// <summary>
        /// 接消息的人
        /// </summary>
        public UserSimpleView ToUser { get; set; }
        /// <summary>
        /// 上传视频的人
        /// </summary>
        public UserSimpleView User { get; set; }
        /// <summary>
        /// 视频信息
        /// </summary>
        public VideoSimpleView Video { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 定位在哪一页:{pageindex:1,index:2}
        /// </summary>
        public dynamic Position { get; set; }
    }
    /// <summary>
    /// 我的消息--空间留言视图
    /// </summary>
    public class SpaceCommentView
    {
        /// <summary>
        /// 消息编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 评论编号
        /// </summary>
        public int EntityId { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 发消息的人
        /// </summary>
        public UserSimpleView FromUser { get; set; }
        /// <summary>
        /// 接消息的人
        /// </summary>
        public UserSimpleView ToUser { get; set; }
        /// <summary>
        /// 空间所属的人
        /// </summary>
        public UserSimpleView User { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 定位在哪一页:{pageindex:1,index:2}
        /// </summary>
        public dynamic Position { get; set; }
    }
    
}