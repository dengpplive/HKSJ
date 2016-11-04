using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ApiParaModel;

namespace HKSJ.WBVV.Business.Interface
{
    /// <summary>
    /// 短信验证接口
    /// </summary>
    public interface ISMSBusiness
    {
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        SMSResult SubmitSMS(SMSApiPara para);

        int IsBlack(string phoneNumber);

        /// <summary>
        /// 发送邮件验证码
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        SMSResult SubmitEmail(SMSApiPara para);
    }
}
