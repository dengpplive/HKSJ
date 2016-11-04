using System;
using System.Web.Http;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.Response;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Api.AppController.Base;

namespace HKSJ.WBVV.Api
{
    /// <summary>
    /// app短信发送接口
    /// Author : AxOne
    /// </summary>
    [RoutePrefix("api/app")]
    public class AppSmsController : AppApiControllerBase
    {
        private readonly ISMSBusiness _iSmsBusiness;

        /// <summary>
        /// AppSmsController
        /// </summary>
        /// <param name="ismsBusiness"></param>
        public AppSmsController(ISMSBusiness ismsBusiness)
        {
            _iSmsBusiness = ismsBusiness;
        }

        /// <summary>
        /// 首页--短信验证码
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost, Route("sms")]
        public ResponsePackage<SMSResult> SubmitSms([FromBody]SMSApiPara para)
        {
            return CommonResult(() => _iSmsBusiness.SubmitSMS(para), r => Console.WriteLine(r.ToJSON()));
        }

    }
}