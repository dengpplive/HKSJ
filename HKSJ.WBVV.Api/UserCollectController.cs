using System;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;

namespace HKSJ.WBVV.Api
{
    /// <summary>
    /// 用户收藏
    /// Author : AxOne
    /// </summary>
    public class UserCollectController : ApiControllerBase
    {
        private readonly IUserCollectBusiness _userCollectBusiness;

        public UserCollectController(IUserCollectBusiness userCollectBusiness)
        {
            _userCollectBusiness = userCollectBusiness;
        }

        [HttpGet]
        //[Authorize] // TODO 有关用户的信息需要权限认证
        public Result GetUserCollectList(int uid, int page, int size = 10)
        {
            return CommonResult(() => _userCollectBusiness.GetUserCollectList(uid, page, size), (r) => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost]
        //[Authorize] // TODO 有关用户的信息需要权限认证
        public Result CollectVideo()
        {
            return CommonResult(() => _userCollectBusiness.CollectVideo(JObject.Value<int>("vid"), JObject.Value<int>("uid")), (r) => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost]
        //[Authorize] // TODO 有关用户的信息需要权限认证
        public Result UnCollectVideo()
        {
            var result = CommonResult(() => _userCollectBusiness.UnCollectVideo(JObject.Value<int>("id"), JObject.Value<int>("uid"), JObject.Value<int>("vid")), (r) => Console.WriteLine(r.ToJSON()));
            return result;
        }

        [HttpPost]

        //[Authorize] // TODO 有关用户的信息需要权限认证
        public Result UnCollectVideoWithoutId()
        {
            var result = CommonResult(() => _userCollectBusiness.UnCollectVideo(JObject.Value<int>("uid"), JObject.Value<int>("vid")), (r) => Console.WriteLine(r.ToJSON()));
            return result;
        }

    }
}