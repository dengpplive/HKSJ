using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HKSJ.Cache;
using HKSJ.WBVV.Common.Email;
using HKSJ.WBVV.Common.Email.Enum;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Common;
using Newtonsoft.Json;

namespace HKSJ.WBVV.MVC.Client.Controllers
{
    /// <summary>
    /// 短信控制器
    /// Author : AxOne
    /// </summary>
    public class SMSController : BaseController
    {
        /// <summary>
        /// 获取短信验证码
        /// Author : AxOne
        /// </summary>
        /// <param name="para">短信参数</param>
        /// <returns></returns>
        public JsonResult SubmitSMS(SMSApiPara para)
        {
            para.Code = new Random().Next(1000, 9999).ToString();
            if (para.AccountType == AccountType.Email)
            {
                var response = WebApiHelper.InvokeApi<string>("SMS/SubmitEmail", new { para = para.ToJSON() });
                if (string.IsNullOrWhiteSpace(response)) { return Json(new { Success = false, Msg = "Api:SubmitEmail Error" }); }
                var result = JsonConvert.DeserializeObject(response, typeof(ResultView<SMSResult>)) as ResultView<SMSResult>;
                if (result != null && result.Success && result.Data != null && result.Data.Code > 0)
                {
                    CacheHelper.Insert(para.PhoneNumber + "SmsCode", para.Code, null, DateTime.Now.AddSeconds(43200));
                    return Json(result);
                }
            }
            else if (para.AccountType == AccountType.Phone)
            {
                para.ClientSendType = SendType.AnyContent;
                para.ClientType = ClientType.Web;
                para.ClientBusinessType = BusinessType.Regist;
                var response = WebApiHelper.InvokeApi<string>("SMS/SubmitSMS", new { para = para.ToJSON() });
                if (string.IsNullOrWhiteSpace(response)) { return Json(new { Success = false, Msg = "Api:SubmitSMS Error" }); }
                var result = JsonConvert.DeserializeObject(response, typeof(ResultView<SMSResult>)) as ResultView<SMSResult>;
                if (result != null && result.Success && result.Data != null && result.Data.Code > 0)
                {
                    CacheHelper.Insert(para.PhoneNumber + "SmsCode", para.Code, null, DateTime.Now.AddSeconds(43200));
                    return Json(result);
                }
            }
            return Json(new { Success = false });
        }

        /// <summary>
        /// 校验短信验证码
        /// Author : AxOne
        /// </summary>
        /// <param name="code">验证码</param>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        public JsonResult CheckSmsCode(string code, string phone)
        {
            var vcode = CacheHelper.Get(phone + "SmsCode") ?? "";
            if (!string.IsNullOrWhiteSpace(vcode.ToString()) && string.Equals(vcode.ToString(), code))
            {
                return Json(new { Success = true, Data = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true, Data = false }, JsonRequestBehavior.AllowGet);
        }

    }

}
