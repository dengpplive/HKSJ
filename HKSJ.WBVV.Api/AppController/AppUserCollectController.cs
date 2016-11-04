using System;
using System.Collections.Generic;
using System.Web.Http;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.Response;
using HKSJ.WBVV.Entity.RequestPara;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Api.AppController.Base;
using HKSJ.WBVV.Entity.Response.App;

namespace HKSJ.WBVV.Api
{
    /// <summary>
    /// app收藏接口
    /// Author : AxOne
    /// </summary>
    [RoutePrefix("api/app")]
    public class AppUserCollectController : AppApiControllerBase
    {
        private readonly IUserCollectBusiness _userCollectBusiness;
        private readonly Business.Interface.APP.IUserCollectBusiness _appUserCollectBusiness;

        /// <summary>
        /// AppUserCollectController
        /// </summary>
        /// <param name="userCollectBusiness"></param>
        /// <param name="appUserCollectBusiness"></param>
        public AppUserCollectController(IUserCollectBusiness userCollectBusiness, Business.Interface.APP.IUserCollectBusiness appUserCollectBusiness)
        {
            _userCollectBusiness = userCollectBusiness;
            _appUserCollectBusiness = appUserCollectBusiness;
        }

        /// <summary>
        /// 个人功能--取消所有收藏
        /// </summary>
        /// <param name="para"></param>
        [HttpPost, CheckAppLogin, Route("delallcollect")]
        public ResponsePackage<bool> DelAllCollect([FromBody]UserCollectsPara para)
        {
            return CommonResult(() => _userCollectBusiness.DelAllCollectVideo(para.UserId), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 个人功能--获取用户收藏列表
        /// </summary>
        /// <param name="para"></param>
        [HttpGet, CheckAppLogin, Route("collects")]
        public ResponsePackage<AppChoicenesssView> GetUserCollects([FromUri]UserCollectsPara para)
        {
            return CommonResult(() => _appUserCollectBusiness.GetUserCollects(para.UserId, para.PageIndex, para.PageSize), r => Console.WriteLine(r.ToJSON()));
        }

    }
}