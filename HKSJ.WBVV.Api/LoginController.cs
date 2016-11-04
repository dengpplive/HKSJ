using System;
using System.Collections.Generic;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;

namespace HKSJ.WBVV.Api
{
    /// <summary>
    /// 登录
    /// Author : AxOne
    /// </summary>
    public class LoginController : ApiControllerBase
    {
        private readonly IUserBusiness _userBusiness;

        public LoginController(IUserBusiness iuserBusiness)
        {
            this._userBusiness = iuserBusiness;
        }

        [HttpPost, CheckApp]
        //[Authorize] // TODO 需要客户端权限认证
        public Result UserLogin()
        {
            this._userBusiness.IpAddress = IpAddress;
            return CommonResult(() => _userBusiness.LoginUser(JObject.Value<string>("account"), JObject.Value<string>("pwd")), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost, CheckApp]
        //[Authorize] // TODO 需要客户端权限认证
        public Result UserLoginById()
        {
            this._userBusiness.IpAddress = IpAddress;
            return CommonResult(() => _userBusiness.LoginUser(JObject.Value<int>("userId")), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost, CheckApp]
        //[Authorize] // TODO 需要客户端权限认证
        public Result CheckPwd()
        {
            return CommonResult(() => _userBusiness.CheckPwd(JObject.Value<int>("uid"), JObject.Value<string>("opwd")), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost, CheckApp]
        //[Authorize] // TODO 需要客户端权限认证
        public Result CheckPhone()
        {
            return CommonResult(() => _userBusiness.CheckPhone(JObject.Value<int>("uid"), JObject.Value<string>("ophone")), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost, CheckLogin]
        public Result CheckNickName()
        {
            return CommonResult(() => _userBusiness.CheckNickName(JObject.Value<int>("uid"), JObject.Value<string>("nickName")), r => Console.WriteLine(r.ToJSON()));
        }


        #region 第三方绑定、登录处理

        [HttpPost, CheckApp]
        //[Authorize] // TODO 需要客户端权限认证
        public Result ThirdPartyBindAndLogin()
        {
            this._userBusiness.IpAddress = IpAddress;
            return CommonResult(() => _userBusiness.ThirdPartyBindAndLogin(JObject.Value<string>("account"), JObject.Value<string>("pwd"), JObject.Value<string>("typeCode"), JObject.Value<string>("relatedId"), JObject.Value<string>("nickName"), JObject.Value<string>("figureURL")), r => Console.WriteLine(r.ToJSON()));
        }

        #endregion

    }
}