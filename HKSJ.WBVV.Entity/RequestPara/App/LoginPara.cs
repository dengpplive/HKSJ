using System.ComponentModel.DataAnnotations;
using HKSJ.WBVV.Common.Validation.Attributes;

namespace HKSJ.WBVV.Entity.RequestPara
{
    /// <summary>
    /// 登录参数
    /// Author:AxOne
    /// </summary>
    public class LoginPara
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Display(Name = "账号"), ParaRequired]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码"), ParaRequired]
        public string Pwd { get; set; }

    }

}
