using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Resource;
using HKSJ.WBVV.Common.Language;
using HKSJ.WBVV.Entity.Response.App;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    public class CommentsView
    {
        /// <summary>
        /// 上级评论
        /// </summary>
        public CommentView ParentComment { get; set; }
        /// <summary>
        /// 下级所有评论
        /// </summary>
        public PageResult ChildComments { get; set; }
    }
    /// <summary>
    /// 评论
    /// </summary>
    [Serializable]
    public class CommentView
    {
        /// <summary>
        /// 评论编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 发送消息的用户信息
        /// </summary>
        public UserSimpleView FromUser { get; set; }
        /// <summary>
        /// 接受消息的用户信息
        /// </summary>
        public UserSimpleView ToUser { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 点赞次数
        /// </summary>
        public int PraisesNum { get; set; }
        /// <summary>
        /// 是否已被点赞，当前登录用户对该评论的点赞状态
        /// </summary>
        public bool IsPraised { get; set; }
        /// <summary>
        /// 回复次数
        /// </summary>
        public int ReplyNum { get; set; }
        /// <summary>
        /// 状态(-2:审核不通过；-1:删除；0:待审核；1:审核通过；)  默认0
        /// </summary>
        public Int16 State { get; set; }

    }
}

