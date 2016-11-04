using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Validation.Attributes;

namespace HKSJ.WBVV.Entity.RequestPara.App
{
    /// <summary>
    /// 发表视频评论参数
    /// </summary>
    public class VideoCommentPara
    {
        /// <summary>
        /// 登录用户编号
        /// </summary>
        [Display(Name = "登录用户编号"), ParaRequired]
        public int LoginUserId { get; set; }
        /// <summary>
        /// 视频编号
        /// </summary>
        [Display(Name = "视频编号"), ParaRequired]
        public int VideoId { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        [Display(Name = "评论内容"), ParaRequired]
        public string Content { get; set; }
    }
    /// <summary>
    /// 发表视频评论参数
    /// </summary>
    public class ReplyVideoCommentPara
    {
        /// <summary>
        /// 登录用户编号
        /// </summary>
        [Display(Name = "登录用户编号"), ParaRequired]
        public int LoginUserId { get; set; }
        /// <summary>
        /// 视频编号
        /// </summary>
        [Display(Name = "视频编号"), ParaRequired]
        public int VideoId { get; set; }
        /// <summary>
        /// 评论编号
        /// </summary>
        [Display(Name = "评论编号"), ParaRequired]
        public int CommentId { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        [Display(Name = "评论内容"), ParaRequired]
        public string Content { get; set; }
    }
    /// <summary>
    /// 删除视频评论参数
    /// </summary>
    public class DeleteVideoCommentPara
    {
        /// <summary>
        /// 登录用户编号
        /// </summary>
        [Display(Name = "登录用户编号"), ParaRequired]
        public int LoginUserId { get; set; }
        /// <summary>
        /// 评论编号
        /// </summary>
        [Display(Name = "评论编号"), ParaRequired]
        public int CommentId { get; set; }
       
    }
}
