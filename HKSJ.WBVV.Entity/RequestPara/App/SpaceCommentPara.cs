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
    /// 删除空间留言参数
    /// </summary>
    public class DeleteSpaceCommentPara
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

    /// <summary>
    /// 发表空间留言参数
    /// </summary>
   public class SpaceCommentPara
    {
        /// <summary>
        /// 登录用户编号
        /// </summary>
        [Display(Name = "登录用户编号"), ParaRequired]
        public int LoginUserId { get; set; }
        /// <summary>
        /// 空间所属者
        /// </summary>
        [Display(Name = "空间所属者"), ParaRequired]
        public int OwnerUserId { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        [Display(Name = "评论内容"), ParaRequired]
        public string Content { get; set; }
    }
   /// <summary>
   /// 回复空间留言参数
   /// </summary>
   public class ReplySpaceCommentPara
   {
       /// <summary>
       /// 登录用户编号
       /// </summary>
       [Display(Name = "登录用户编号"), ParaRequired]
       public int LoginUserId { get; set; }
       /// <summary>
       /// 空间所属者
       /// </summary>
       [Display(Name = "空间所属者"), ParaRequired]
       public int OwnerUserId { get; set; }
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
}
