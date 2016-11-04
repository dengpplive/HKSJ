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
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Api
{
    public class UserController : ApiControllerBase
    {

        private readonly IUserBusiness _userBusiness;
        private readonly IVideoBusiness _videoBusiness;
        public UserController(IUserBusiness userBusiness, IVideoBusiness videoBusiness)
        {
            this._userBusiness = userBusiness;
            this._videoBusiness = videoBusiness;
        }

        [HttpGet]
        public dynamic GetUserInfo(int uid)
        {
            User userInfo = _userBusiness.GetEntityByUserId(uid);
            if (userInfo == null) return new { msg = LanguageUtil.Translate("api_Controller_User_GetUserInfo_msg") };
            string nickName = userInfo.NickName;
            int bbNum = userInfo.BB;
            string headPath = userInfo.Picture;

            int playNum = userInfo.PlayCount;
            int fansNum = userInfo.FansCount;
            return new { msg = "success", userNickName = nickName, bBNum = bbNum, imgHeadPath = headPath, videoPlayNum = playNum, userFansNum = fansNum };
        }

        [HttpGet]
        public dynamic GetHistoryVideoByUserId(int uid, int pageIndex = 1, int pageSize = 8)
        {
            UserHistoryViews userHistories = _userBusiness.GetHistoryVideoByUserId(uid, pageIndex, pageSize);
            return new { msg = "success", datalist = userHistories };
        }

        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public PageResult SearchUser()
        {
            _userBusiness.PageIndex = PageIndex;
            _userBusiness.PageSize = PageSize;
            var result = _userBusiness.SearchUser(Condtions, OrderCondtions);
            return result;
        }

        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result ForbiddenUser()
        {
            return CommonResult(() => _userBusiness.ForbiddenUser(JObject.Value<int>("uid"), JObject.Value<bool>("state")), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpGet, CheckApp]
        //[Authorize] // TODO 需要客户端权限认证
        public Result GetAccountInfo(int uid)
        {
            return CommonResult(() => _userBusiness.GetAccountInfo(uid), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost, CheckLogin]
        //[Authorize] // TODO 需要客户端权限认证
        public Result AccountSet()
        {
            var jsonData = JObject.Value<string>("para");
            var data = JsonConvert.DeserializeObject<AccountPara>(jsonData);
            return CommonResult(() => _userBusiness.AccountSet(data), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost, CheckLogin]
        //[Authorize] // TODO 需要客户端权限认证
        public Result SetPassword()
        {
            return CommonResult(() => _userBusiness.SetPassword(JObject.Value<int>("uid"), JObject.Value<string>("pwd")), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost, CheckLogin]
        //[Authorize] // TODO 需要客户端权限认证
        public Result SetPhone()
        {
            return CommonResult(() => _userBusiness.SetPhone(JObject.Value<int>("uid"), JObject.Value<string>("phone"), JObject.Value<string>("pwd")), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost, CheckLogin]
        //[Authorize] // TODO 需要客户端权限认证
        public Result SetEmail()
        {
            return CommonResult(() => _userBusiness.SetEmail(JObject.Value<int>("uid"), JObject.Value<string>("email"), JObject.Value<string>("pwd")), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost, CheckLogin]
        //[Authorize] // TODO 需要客户端权限认证
        public Result UpdateUserPic()
        {
            return CommonResult(() => _userBusiness.UpdateUserPic(JObject.Value<int>("uid"), JObject.Value<string>("key")), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost,CheckApp]
        //[Authorize] // TODO 需要客户端权限认证
        public Result UpdatePwdByPhone()
        {
            _userBusiness.IpAddress = IpAddress;
            return CommonResult(() => _userBusiness.UpdatePwdByPhone(JObject.Value<string>("account"), JObject.Value<string>("pwd")), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpGet]
        //[Authorize] // TODO 需要客户端权限认证
        public Result GetUserPicKey(int uid)
        {
            return CommonResult(() => _userBusiness.GetUserPicKey(uid), r => Console.WriteLine(r.ToJSON()));
        }

        //[HttpPost]  //todo 移动到播放记录接口
        //public Result AddHistoryVideo(int videoId, int userId, int watchTime = 1)
        //{
        //    this._userBusiness.IpAddress = IpAddress;
        //    return CommonResult(() => this._userBusiness.AddHistoryVideo(videoId, userId, watchTime),
        //        (r) => Console.WriteLine(r.ToJSON()));
        //}

        /// <summary>
        /// 分享视频：【分享者】获得播币
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result IncomeShare()
        {
            this._userBusiness.IpAddress = IpAddress;
            return CommonResult(() => this._userBusiness.IncomeShare(JObject.Value<int>("videoId"), JObject.Value<int>("DemandUserId"), JObject.Value<string>("IpAddress"), JObject.Value<int>("ShareUserId"), JObject.Value<string>("ShareIpAddress")), (r) => Console.WriteLine(r.ToJSON()));
        }

        [HttpGet]
        public Result GetUserVideoViews(int userId, int pageIndex = 1, int pageSize = 5, int videoState = -1)
        {
            return CommonResult(() => this._userBusiness.GetUserVideoViews(userId, pageIndex, PageSize, videoState),
                (r) => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 根据用户ID查询用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public UserView GetUser(int id)
        {
            return this._userBusiness.GetUser(id);
        }

        [HttpGet]
        public Result GetAVideoInfoById(int id)
        {
            return CommonResult(() => this._videoBusiness.GetAVideoInfoById(id), (r) => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost]
        public Result UpdateUserBardian()
        {
            return CommonResult(() => _userBusiness.UpdateUserBardian(JObject.Value<int>("userId"), JObject.Value<string>("bardian")), r => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost]
        public Result UpdateUserBannerImage()
        {
            return CommonResult(() => _userBusiness.UpdateUserBardian(JObject.Value<int>("userId"), JObject.Value<string>("bannerImage")), r => Console.WriteLine(r.ToJSON()));
        }


        [HttpPost]
        public Result DelAllRecByUserId(int uid)
        {
            return CommonResult(() => this._userBusiness.DelAllRecByUserId(uid), (r) => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 更换用户的皮肤
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skinId"></param>
        /// <returns></returns>
        [HttpGet]
        public Result SaveSkinByUserId(int userId, int skinId)
        {
            return CommonResult(() => this._userBusiness.SaveSkinByUserId(userId, skinId), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        //获取用户空间的信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skinId"></param>
        /// <returns></returns>
        [HttpGet]
        public Result GetUserRoomInfo(int loginUserId, int browserUserId)
        {
            return CommonResult(() => this._userBusiness.GetUserRoomInfo(loginUserId, browserUserId), (r) => Console.WriteLine(r.ToJSON()));
        }



        /// <summary>
        /// 获取当前用户观看视频的记录
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="videoId">视频ID</param>
        /// <returns>已观看视频的时间，以秒为单位</returns>
        [HttpGet]
        public long GetUserWatchTime(int uId, int videoId)
        {
            return this._userBusiness.GetUserWatchTime(uId, videoId);
        }







    }
}
