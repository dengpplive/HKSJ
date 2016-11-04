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
    /// 用户关注参数
    /// </summary>
    public class UserFansPara
    {
        /// <summary>
        /// 登录用户编号
        /// </summary>
        [Display(Name = "登录用户编号"), ParaRequired]
        public int LoginUserId { get; set; }

        /// <summary>
        /// 关注用户编号
        /// </summary>
        [Display(Name = "关注用户编号"), ParaRequired]
        public int SubscribeUserId { get; set; }
    }
}
