using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Email.Enum;

namespace HKSJ.WBVV.Common.Email.Model
{
    public class EmailModel
    {
        /// <summary>
        /// 接收人
        /// </summary>
        public IList<string> Received { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        ///内容是否是html
        /// </summary>
        public bool IsBodyHtml { get; set; }
        /// <summary>
        /// 邮件类型
        /// </summary>
        public EmailEnum EmailEnum { get; set; }
        /// <summary>
        /// 是否是异步
        /// </summary>
        public bool IsAsync { get; set; }

        public EmailModel(EmailEnum emailEnum, string subject, string body, IList<string> received, bool isBodyHtml, bool isAsync)
        {
            this.EmailEnum = emailEnum;
            this.Subject = subject;
            this.Body = body;
            this.IsBodyHtml = isBodyHtml;
            this.Received = received;
            this.IsAsync = isAsync;
        }
        public EmailModel(EmailEnum emailEnum, string subject, string body, string received, bool isBodyHtml, bool isAsync)
            : this(emailEnum, subject, body, new List<string>() { received }, isBodyHtml, isAsync)
        {
        }
    }
}
