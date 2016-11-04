using System;
using System.Data;
using System.Collections.Generic;
using System.Web.Http;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Entity.ViewModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HKSJ.WBVV.Entity.ViewModel.Manage;

namespace HKSJ.WBVV.Api
{
    public class VideoApproveController : ApiControllerBase
    {

        private readonly IVideoApproveBusiness _iVideoApproveBusiness;
        public VideoApproveController(IVideoApproveBusiness iVideoApproveBusiness)
        {

            this._iVideoApproveBusiness = iVideoApproveBusiness;
        }

        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result AddApproveInfo()
        {
            return CommonResult(() => this._iVideoApproveBusiness.AddApproveInfo(JObject.Value<string>("dataModel").FromJSON<VideoApproveView>()), (r) => Console.WriteLine(r.ToJSON()));
        }


    }
}
