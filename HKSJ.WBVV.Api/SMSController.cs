using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Email;
using HKSJ.WBVV.Common.Email.Enum;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ApiParaModel;
using Newtonsoft.Json;

namespace HKSJ.WBVV.Api
{
    /// <summary>
    /// ¶ÌÐÅÑéÖ¤Âë
    /// Author : AxOne
    /// </summary>
    public class SMSController : ApiControllerBase
    {
        private readonly ISMSBusiness _iSMSBusiness;

        public SMSController(ISMSBusiness iSMSBusiness)
        {
            this._iSMSBusiness = iSMSBusiness;
        }

        [HttpPost]
        public Result SubmitSMS()
        {
            var jsonData = JObject.Value<string>("para");
            var data = JsonConvert.DeserializeObject<SMSApiPara>(jsonData);
            return CommonResult(() => _iSMSBusiness.SubmitSMS(data), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost]
        public Result SubmitEmail()
        {
            var jsonData = JObject.Value<string>("para");
            var data = JsonConvert.DeserializeObject<SMSApiPara>(jsonData);
            return CommonResult(() => _iSMSBusiness.SubmitEmail(data), r => Console.WriteLine(r.ToJSON()));
        }

        
    }
}