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
    public class VideoPlayRecordController : ApiControllerBase
    {

        private readonly IVideoPlayRecordBusiness _iVideoPlayRecordBusiness;
        private readonly IUserBusiness _userBusiness;
        public VideoPlayRecordController(IVideoPlayRecordBusiness iVideoPlayRecordBusiness, IUserBusiness userBusiness)
        {
            this._iVideoPlayRecordBusiness = iVideoPlayRecordBusiness;
            _userBusiness = userBusiness;
        }


        [HttpPost]
        public Result AddVideoPlayRecord()
        {
            this._iVideoPlayRecordBusiness.UserId = JObject.Value<int>("userId");
            this._iVideoPlayRecordBusiness.IpAddress = IpAddress;
            this._userBusiness.IpAddress = IpAddress;
            int videoId = JObject.Value<int>("videoId");
            int watchTime = JObject.Value<int>("watchTime");
            var res= CommonResult(() => this._iVideoPlayRecordBusiness.AddVideoPlayRecord(videoId), (r) => Console.WriteLine(r.ToJSON()));
            if (this._iVideoPlayRecordBusiness.UserId > 0)
            {
                return CommonResult(() => this._userBusiness.AddHistoryVideo(videoId, this._iVideoPlayRecordBusiness.UserId, watchTime),
                (r) => Console.WriteLine(r.ToJSON()));  
            }
            return res;
        }


    }
}
