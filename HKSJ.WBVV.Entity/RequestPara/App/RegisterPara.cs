using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HKSJ.WBVV.Common.Validation.Attributes;

namespace HKSJ.WBVV.Entity.RequestPara
{
    /// <summary>
    /// 注册参数
    /// Author:AxOne
    /// </summary>
    public class RegisterPara
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        [Display(Name = "用户账号"), ParaRequired]
        public string Account { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Display(Name = "用户密码"), ParaRequired]
        public string Pwd { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        [Display(Name = "账户类型(0:手机账户 1:邮箱账户)"), ParaRequired]
        public AccountType AccountType { get; set; }

    }

    /// <summary>
    /// 账户类型(0:手机账户 1:邮箱账户)
    /// </summary>
    [Description("账户类型(0:手机账户 1:邮箱账户)")]
    public enum AccountType
    {
        /// <summary>
        /// 手机账户
        /// </summary>
        [Description("手机账户")]
        Phone = 0,
        /// <summary>
        /// 邮箱账户
        /// </summary>
        [Description("邮箱账户")]
        Email = 1
    }
}
