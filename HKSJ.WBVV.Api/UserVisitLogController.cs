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
    /// <summary>
    /// 历史记录
    /// </summary>
    public class UserVisitLogController : ApiControllerBase
    {
        private readonly IUserVisitLogBusiness _userVisitLogBusiness;
        public UserVisitLogController(IUserVisitLogBusiness userVisitLogBusiness)
        {
            this._userVisitLogBusiness = userVisitLogBusiness;

        }
        /// <summary> 
        /// 获取用户的访问记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IList<UserVisitView> GetUserVisitLogList()
        {
            int browserUserId = this.JObject.Value<int>("browserUserId");
            this._userVisitLogBusiness.PageSize = this.PageSize;
            return this._userVisitLogBusiness.GetUserVisitLogList(browserUserId, this.Condtions, this.OrderCondtions);
        }


        /// <summary> 
        /// 添加和修改个人空间用户的访问记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultView<bool> CreateUserVisitLog()
        {
            int CreateUserId = this.JObject.Value<int>("loginUserId");
            int VisitorUserId = this.JObject.Value<int>("loginUserId");
            int VisitedUserId = this.JObject.Value<int>("browserUserId");
            return this._userVisitLogBusiness.CreateAndUpdateUserVisitLog(CreateUserId, VisitorUserId, VisitedUserId);
        }


    }
}
