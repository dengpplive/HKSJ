using System;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.Enums;

namespace HKSJ.WBVV.Api
{
    public class AuthKeysController : ApiControllerBase
    {

        private readonly IAuthKeysBusiness _iAuthKeysBusiness;
        public AuthKeysController(IAuthKeysBusiness iAuthKeysBusiness)
        {

            _iAuthKeysBusiness = iAuthKeysBusiness;
        }

        [HttpPost]
        public Result CreatePublicKey()
        {
            return CommonResult(() => _iAuthKeysBusiness.CreatePublicKey(JObject.Value<int>("uid"), JObject.Value<AuthUserType>("userType")), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost]
        public Result GetAuthKeys(int uid, AuthUserType userType)
        {
            return CommonResult(() => _iAuthKeysBusiness.GetAuthKeys(uid, userType), r => Console.WriteLine(r.ToJSON()));
        }
    }
}
