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
    /// 用户的粉丝
    /// </summary>
    public class UserFansController : ApiControllerBase
    {
        private readonly IUserFansBusiness _iuserFansBusiness;
        public UserFansController(IUserFansBusiness iuserFansBusiness)
        {
            this._iuserFansBusiness = iuserFansBusiness;

        }
       
        //订阅和取消订阅
        [HttpPost]
        public Result SaveSubscribe()
        {
            int createUserId = this.JObject.Value<int>("createUserId");
            int subscribeUserId = this.JObject.Value<int>("subscribeUserId");
            bool careState = this.JObject.Value<bool>("careState");
            return CommonResult(() => this._iuserFansBusiness.SaveSubscribe(createUserId, subscribeUserId, careState), (r) => { Console.WriteLine(r.ToJSON()); });
        }
        /// <summary> 
        /// 获取当前用户的粉丝集合 含有分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PageResult GetUserFunsList()
        {
            int userId = this.JObject.Value<int>("userId");
            int loginUserId = this.JObject.Value<int>("loginUserId");
            this._iuserFansBusiness.PageIndex = this.PageIndex;
            this._iuserFansBusiness.PageSize = this.PageSize;
            return this._iuserFansBusiness.GetUserFunsList(userId, loginUserId);
        }
        /// <summary> 
        /// 获取当前用户的订阅集合 含有分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PageResult GetUserSubscribeList()
        {
            int userId = this.JObject.Value<int>("userId");
            int loginUserId = this.JObject.Value<int>("loginUserId");
            this._iuserFansBusiness.PageIndex = this.PageIndex;
            this._iuserFansBusiness.PageSize = this.PageSize;
            return this._iuserFansBusiness.GetUserSubscribeList(userId, loginUserId);
        }



        /// <summary> 
        /// 获取当前用户的订阅  用户列表 含有分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PageResult GetUserSubscribeUserList()
        {
            int loginUserId = this.JObject.Value<int>("loginUserId");
            this._iuserFansBusiness.PageIndex = this.PageIndex;
            this._iuserFansBusiness.PageSize = this.PageSize;
            return this._iuserFansBusiness.GetUserSubscribeUserList(loginUserId);
        }
        /// <summary> 
        /// 获取当前用户的订阅  所有用户视频列表 含有分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PageResult GetUserSubscribeVideoList()
        {
            int loginUserId = this.JObject.Value<int>("loginUserId");
            this._iuserFansBusiness.PageIndex = this.PageIndex;
            this._iuserFansBusiness.PageSize = this.PageSize;
            return this._iuserFansBusiness.GetUserSubscribeVideoList(loginUserId);
        }

        [HttpPost]
        public ResultView<bool> IsSubscribe()
        {
            int createUserId = this.JObject.Value<int>("createUserId");
            int subscribeUserId = this.JObject.Value<int>("subscribeUserId");
            return this._iuserFansBusiness.IsSubscribe(createUserId, subscribeUserId);
        }
    }
}
