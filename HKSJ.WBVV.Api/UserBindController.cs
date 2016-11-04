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
    public class UserBindController : ApiControllerBase
    {

        private readonly IUserBindBusiness _iuserBindBusiness;
        public UserBindController(IUserBindBusiness iuserBindBusiness)
        {
            this._iuserBindBusiness = iuserBindBusiness;
        }

        /// <summary>
        /// 是否存在第三方绑定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Result IsExistedThirdPartyById(string uniquelyId, string typeCode)
        {
            return CommonResult(() => this._iuserBindBusiness.IsExistedThirdPartyById(uniquelyId, typeCode.ToLower()), (r) => Console.WriteLine(r.ToJSON()));
        }
    }
}
