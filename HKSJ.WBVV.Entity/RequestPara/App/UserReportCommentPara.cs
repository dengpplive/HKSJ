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
    /// 用户举报评论参数
    /// </summary>
    public class UserReportCommentPara
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
