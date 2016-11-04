using System;
using System.Data;
using System.Collections.Generic;
using System.Web.Http;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Entity.ViewModel.Client;
using Newtonsoft.Json;

namespace HKSJ.WBVV.Api
{
    public class UserSkinController : ApiControllerBase
    {

        private readonly IUserSkinBusiness _userSkinBusiness;
        public UserSkinController(IUserSkinBusiness userSkinBusiness)
        {
            this._userSkinBusiness = userSkinBusiness;
        }
        /// <summary>
        /// 获取皮肤列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Result GetUserSkinList()
        {
            return CommonResult(() => this._userSkinBusiness.GetUserSkinList(), (r) => Console.WriteLine(r.ToJSON()));
        }
    }
}
