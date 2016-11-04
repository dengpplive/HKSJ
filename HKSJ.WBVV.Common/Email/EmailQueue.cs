using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Email.Model;
using HKSJ.WBVV.Common.Email.Enum;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Common.Email
{
    public static class EmailQueue
    {
        public static Queue<EmailModel> EmailModel = new Queue<EmailModel>();

        public static void ClrearQueue()
        {
            while (EmailModel.Count != 0)
            {
                EmailModel.Dequeue();
            }
        }
        /// <summary>
        /// 添加队列
        /// </summary>
        /// <param name="emailModel"></param>
        public static void AddQueue(EmailModel emailModel)
        {
            AssertUtil.IsNotNull(emailModel, LanguageUtil.Translate("com_EmailQueue_AddQueue_check_content"));
            AssertUtil.IsNotNull(emailModel.EmailEnum, LanguageUtil.Translate("com_EmailQueue_AddQueue_check_emailenum"));
            AssertUtil.NotNullOrWhiteSpace(emailModel.Subject, LanguageUtil.Translate("com_EmailQueue_AddQueue_check_subject"));
            AssertUtil.NotNullOrWhiteSpace(emailModel.Body, LanguageUtil.Translate("com_EmailQueue_AddQueue_check_body"));
            AssertUtil.IsNotEmptyCollection(emailModel.Received, LanguageUtil.Translate("com_EmailQueue_AddQueue_check_received"));
            EmailModel.Enqueue(emailModel);
        }

        /// <summary>
        /// 添加队列
        /// </summary>
        /// <param name="emailEnum">邮件类型</param>
        /// <param name="received">接收人</param>
        /// <param name="isBodyHtml">是否是html</param>
        /// <param name="isAsync">是否是异步</param>
        /// <param name="parameters">传入的动态参数</param>
        public static void AddQueue(EmailEnum emailEnum, string received, bool isBodyHtml, bool isAsync, Dictionary<string, string> parameters)
        {
            AddQueue(new EmailModel(emailEnum, EnumHelper.GetEnumDescription(emailEnum), EmailFactory.ProcessTemplate(emailEnum).GetTemplate(parameters), received, isBodyHtml, isAsync));
        }

        /// <summary>
        /// 添加队列
        /// </summary>
        /// <param name="emailEnum">邮件类型</param>
        /// <param name="received">多个接收人</param>
        /// <param name="isBodyHtml">是否是html</param>
        /// <param name="isAsync">是否是异步</param>
        /// <param name="parameters">传入的动态参数</param>
        public static void AddQueue(EmailEnum emailEnum, IList<string> received, bool isBodyHtml, bool isAsync, Dictionary<string, string> parameters)
        {
            AddQueue(new EmailModel(emailEnum, EnumHelper.GetEnumDescription(emailEnum), EmailFactory.ProcessTemplate(emailEnum).GetTemplate(parameters), received, isBodyHtml, isAsync));
        }
    }
}
