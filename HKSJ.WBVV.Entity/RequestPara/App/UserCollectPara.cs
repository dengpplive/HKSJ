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
    /// 喜欢视频参数
    /// </summary>
    public class UserCollectPara
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
    }
}
