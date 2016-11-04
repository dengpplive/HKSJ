using System;
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
    /// app用户接口
    /// Author : AxOne
    /// </summary>
    [RoutePrefix("api/app")]
    public class AppUserController : AppApiControllerBase
    {
        private readonly IUserBusiness _userBusiness;
        private readonly Business.Interface.APP.IUserBusiness _appUserBusiness;
        private readonly IQiniuUploadBusiness _qiniuUploadBusiness;

        /// <summary>
        /// AppUserController
        /// </summary>
        /// <param name="iuserBusiness"></param>
        /// <param name="appUserBusiness"></param>
        /// <param name="qiniuUploadBusiness"></param>
        public AppUserController(IUserBusiness iuserBusiness, Business.Interface.APP.IUserBusiness appUserBusiness, IQiniuUploadBusiness qiniuUploadBusiness)
        {
            _userBusiness = iuserBusiness;
            _appUserBusiness = appUserBusiness;
            _qiniuUploadBusiness = qiniuUploadBusiness;
        }

        /// <summary>
        /// 个人功能--获取用户信息
        /// </summary>
        /// <param name="para"></param>
        [HttpGet, CheckAppLogin, Route("userinfo")]
        public ResponsePackage<AppUserView> GetUserInfo([FromUri]UserPara para)
        {
            return CommonResult(() => GetUserInfo(para.UserId), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 个人功能--修改用户信息
        /// </summary>
        /// <param name="para"></param>
        [HttpPost, CheckAppLogin, Route("updateuserinfo")]
        public ResponsePackage<bool> UpdateUserInfo([FromBody]UpdateUserPara para)
        {
            return CommonResult(() => _appUserBusiness.UpdateUserInfo(para), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 通用接口--获取视频下载地址
        /// </summary>
        /// <param name="para"></param>
        [HttpGet, Route("videourl")]
        public ResponsePackage<string> VideoUrl([FromUri]VideoUrlPara para)
        {
            return CommonResult(() => _qiniuUploadBusiness.GetDownloadUrl(para.key, "video"));
        }

        /// <summary>
        /// 个人功能--播放历史列表
        /// </summary>
        /// <param name="para"></param>
        [HttpGet, CheckAppLogin, Route("history")]
        public ResponsePackage<AppChoicenesssView> GetUserHistory([FromUri]UserCollectsPara para)
        {
            return CommonResult(() => _appUserBusiness.GetUserHistories(para.UserId, para.PageIndex, para.PageSize), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 个人功能--清除所有播放历史
        /// </summary>
        /// <param name="para"></param>
        [HttpGet, CheckAppLogin, Route("delhistory")]
        public ResponsePackage<bool> DelUserHistories([FromUri]UserPara para)
        {
            return CommonResult(() => _appUserBusiness.DelUserHistories(para.UserId), r => Console.WriteLine(r.ToJSON()));
        }


        #region private method

        private AppUserView GetUserInfo(int userId)
        {
            var data = new AppUserView();
            var uv = _userBusiness.GetUserInfo(userId);
            if (uv == null) return data;
            data.Id = uv.Id;
            data.NickName = uv.NickName;
            data.Picture = uv.Picture;
            data.Account = uv.Account;
            data.Pwd = uv.Pwd;
            data.PlayCount = uv.PlayCount;
            data.FansCount = uv.FansCount;
            data.SkinId = uv.SkinId;
            data.BannerImage = uv.BannerImage;
            data.Bardian = uv.Bardian;
            data.Phone = uv.Phone;
            data.SubscribeNum = uv.SubscribeNum;
            data.Level = uv.Level;
            data.State = uv.State;
            data.IsSubed = uv.IsSubed;
            data.Token = uv.Token;
            data.City = uv.City;
            data.Gender = uv.Gender;
            data.Birthdate = uv.Birthdate;
            return data;
        }

        #endregion
    }
}