using System;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;

namespace HKSJ.WBVV.Api
{
    /// <summary>
    /// зЂВс
    /// Author : AxOne
    /// </summary>
    public class RegisterController : ApiControllerBase
    {
        private readonly IUserBusiness _iuserBusiness;

        public RegisterController(IUserBusiness iuserBusiness)
        {
            _iuserBusiness = iuserBusiness;
        }

        [HttpPost, CheckApp]
        public Result UserRegist()
        {
            return CommonResult(() => _iuserBusiness.RegisterUser(JObject.Value<string>("account"), JObject.Value<string>("pwd"), JObject.Value<int>("type")), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpGet]
        public Result CheckAccount(string account)
        {
            return CommonResult(() => _iuserBusiness.CheckAccount(account), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpGet]
        public Result CheckEmail(string email)
        {
            return CommonResult(() => _iuserBusiness.CheckEmail(email), r => Console.WriteLine(r.ToJSON()));
        }
        [HttpPost, CheckApp]
        public Result ThirdPartyBindAndRegister()
        {
            return CommonResult(() => _iuserBusiness.ThirdPartyBindAndRegister(JObject.Value<string>("account"), JObject.Value<string>("pwd"), JObject.Value<string>("typeCode"), JObject.Value<string>("relatedId"), JObject.Value<string>("nickName"), JObject.Value<string>("figureURL"), JObject.Value<int>("type")), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost, CheckApp]
        public Result AutoRegisterAndBindThirdParty()
        {
            return CommonResult(() => _iuserBusiness.AutoRegisterAndBindThirdParty(JObject.Value<string>("typeCode"), JObject.Value<string>("relatedId"), JObject.Value<string>("nickName"), JObject.Value<string>("figureURL")), r => Console.WriteLine(r.ToJSON()));
        }
    }
}