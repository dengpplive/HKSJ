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
    /// 清空系统消息参数
    /// </summary>
   public class ClearSystemMessagePara
    {
        /// <summary>
        /// 登录用户编号
        /// </summary>
        [Display(Name = "登录用户编号"), ParaRequired]
        public int LoginUserId { get; set; }
    }
}
