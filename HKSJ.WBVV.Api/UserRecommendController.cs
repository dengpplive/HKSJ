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
    /// 用户推荐
    /// </summary>
    public class UserRecommendController : ApiControllerBase
    {
        private readonly IUserRecommendBusiness _userRecommendBusiness;
        public UserRecommendController(IUserRecommendBusiness userRecommendBusiness)
        {
            this._userRecommendBusiness = userRecommendBusiness;

        }
        /// <summary> 
        /// 获取推荐的用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IList<UserRecommendView> GetUserRecommendList()
        {
            this._userRecommendBusiness.PageSize = this.PageSize;
            int userId = this.JObject.Value<int>("userId");
            int loginUserId = this.JObject.Value<int>("loginUserId");
            return this._userRecommendBusiness.GetUserRecommendList(userId, loginUserId, this.Condtions, this.OrderCondtions);
        }




    }
}
