using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HKSJ.WBVV.Common.Validation.Attributes;

namespace HKSJ.WBVV.Entity.ApiParaModel
{
    /// <summary>
    /// 短信验证码请求参数
    /// </summary>
    [Serializable]
    public class SMSApiPara
    {
        /// <summary>
        /// 接收短信的手机号码
        /// </summary>
        [Display(Name = "接收短信的手机号码"), ParaRequired]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 账户类型
        /// </summary>
        [Display(Name = "账户类型(0:手机账户 1:邮箱账户)"), ParaRequired]
        public AccountType AccountType { get; set; }

        /// <summary>
        /// 发送类型
        /// </summary>
        [Display(Name = "发送类型(1:模版短信[目前业务只有验证码] 2:任意内容 3:批量发送)"), ParaRequired]
        public SendType ClientSendType { get; set; }

        /// <summary>
        /// 客户端类型
        /// </summary>
        [Display(Name = "客户端类型(1:我播前端 2:我播App)"), ParaRequired]
        public ClientType ClientType { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        [Display(Name = "业务类型(1:注册 2:密码找回 3:绑定手机)"), ParaRequired]
        public BusinessType ClientBusinessType { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Display(Name = "验证码"), ParaRequired]
        public string Code { get; set; }
    }

    public class SMSPara
    {
        /// <summary>
        /// 接收短信的手机号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 业务系统的短信账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 业务系统的短信密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 业务系统ID（业务数据登录短信管理系统）
        /// </summary>
        public int InvokerID { get; set; }
        /// <summary>
        /// 短信内容（验证码的只填写验证码部分，模板已配置）
        /// </summary>
        public string SMSContent { get; set; }
        /// <summary>
        /// 发送类型
        /// </summary>
        public int SendType { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public int BusinessType { get; set; }
    }

    /// <summary>
    /// 短信发送类型
    /// </summary>
    public enum SendType
    {
        /// <summary>
        /// 模版短信(目前业务只有验证码,时效性好[数秒])
        /// </summary>
        [Description("模版短信(目前业务只有验证码)")]
        Template = 1,
        /// <summary>
        /// 任意内容(时效性好[数秒])
        /// </summary>
        [Description("任意内容")]
        AnyContent = 2,
        /// <summary>
        /// 批量发送(时效性差)
        /// </summary>
        [Description("批量发送")]
        Batch = 3
    }

    /// <summary>
    /// 短信业务类型
    /// </summary>
    public enum BusinessType
    {
        /// <summary>
        /// 注册
        /// </summary>
        [Description("注册")]
        Regist = 1,
        /// <summary>
        /// 密码找回
        /// </summary>
        [Description("密码找回")]
        PwdRecover = 2,
        /// <summary>
        /// 绑定手机
        /// </summary>
        [Description("绑定手机")]
        BindingPhone = 3
    }

    /// <summary>
    /// 客户端类型
    /// </summary>
    public enum ClientType
    {
        /// <summary>
        /// 我播前端
        /// </summary>
        [Description("我播前端")]
        Web = 1,
        /// <summary>
        /// 我播App
        /// </summary>
        [Description("我播App")]
        App = 2
        //,
        /// <summary>
        /// 我播后端 (暂不提供)
        /// </summary>
        //[Description("我播后端")]
        //System = 3
    }

    /// <summary>
    /// 账户类型
    /// </summary>
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
