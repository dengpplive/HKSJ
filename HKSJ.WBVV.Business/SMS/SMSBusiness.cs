using System;
using System.Collections.Generic;
using HKSJ.Utilities;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.Common.Email;
using HKSJ.WBVV.Common.Email.Enum;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Logger;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{
    /// <summary>
    /// 短信验证
    /// </summary>
    public class SMSBusiness : ISMSBusiness
    {
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public SMSResult SubmitSMS(SMSApiPara para)
        {
            #region param
            var SMSPara = new SMSPara
            {
                PhoneNumber = para.PhoneNumber,
                SMSContent = string.Format(LanguageUtil.Translate("api_Business_SMS_SubmitSMS_SMSContent"), para.Code)
            };
            switch (para.ClientType)
            {
                case ClientType.Web:
                    SMSPara.InvokerID = 230;
                    SMSPara.Account = "wb230";
                    SMSPara.Password = "wb230";
                    break;
                case ClientType.App:
                    SMSPara.InvokerID = 231;
                    SMSPara.Account = "wb231";
                    SMSPara.Password = "wb231";
                    break;
            }
            switch (para.ClientBusinessType)
            {
                case BusinessType.Regist:
                    SMSPara.BusinessType = 1;
                    break;
                case BusinessType.PwdRecover:
                    SMSPara.BusinessType = 2;
                    break;
                case BusinessType.BindingPhone:
                    SMSPara.BusinessType = 3;
                    break;
            }
            switch (para.ClientSendType)
            {
                case SendType.Template:
                    SMSPara.SendType = 1;
                    break;
                case SendType.AnyContent:
                    SMSPara.SendType = 2;
                    break;
                case SendType.Batch:
                    SMSPara.SendType = 3;
                    break;
            }
            #endregion
            HttpHelper.SMSBaseUri = ApiConfig.SMSBaseUri;
            var result = HttpHelper.SMSPost<SMSResult>("SubmitSMS", SMSPara);
            if (result == null || result.Code < 1)
            {
                var code = result == null ? 0 : result.Code;
                var message = result == null ? "短信API错误" : result.Message;
                var msg = string.Format(LanguageUtil.Translate("api_Business_SMS_SubmitSMS_InfoFormat"), para.PhoneNumber, code, message);
                LogBuilder.Log4Net.Error(msg);
            }
            return result;

        }

        /// <summary>
        /// 查询黑名单
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public int IsBlack(string phoneNumber)
        {

            throw new NotImplementedException();
        }

        /// <summary>
        /// 发送邮件验证码
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public SMSResult SubmitEmail(SMSApiPara para)
        {
            var result = new SMSResult { Code = 0, Message = LanguageUtil.Translate("api_Business_SMS_SubmitEmail_Message") };
            var flag = false;
            var emailType = EmailEnum.Register;
            if (para.ClientBusinessType == BusinessType.BindingPhone)
            {
                emailType = EmailEnum.UpdateEmail;
            }
            else if (para.ClientBusinessType == BusinessType.PwdRecover)
            {
                emailType = EmailEnum.FindPwd;
            }
            EmailHelper.SendEmailHtml(emailType, para.PhoneNumber, new Dictionary<string, string> { { "@code@", para.Code }, { "@imageUrl@", @"http://www.5bvv.com/Content/images/icon_img/5BVV_logo_03.png" } }, r =>
            { flag = r; });
            if (flag)
            {
                result.Code = 1;
                result.Message = LanguageUtil.Translate("api_Business_SMS_SubmitEmail_resultMessage");
            }
            return result;
        }

    }
}
