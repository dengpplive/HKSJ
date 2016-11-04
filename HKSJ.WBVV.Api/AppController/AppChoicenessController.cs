using HKSJ.WBVV.Api.AppController.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Business.Interface.APP;
using HKSJ.WBVV.Business.Interface.UserCenter;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.RequestPara;
using HKSJ.WBVV.Entity.RequestPara.App;
using HKSJ.WBVV.Entity.Response;
using HKSJ.WBVV.Entity.Response.App;
using HKSJ.WBVV.Entity.ViewModel.Client;

namespace HKSJ.WBVV.Api.AppController
{
    /// <summary>
    /// 精选
    /// </summary>
    [RoutePrefix("api/app/choiceness")]
    public class AppChoicenessController : AppApiControllerBase
    {
        private readonly IChoicenessBusiness _choicenessBusiness;
        private readonly ICommentBusiness _commentBusiness;
        private readonly IQiniuUploadBusiness _qiniuUploadBusiness;

        /// <summary>
        /// AppChoicenessController
        /// </summary>
        /// <param name="choicenessBusiness"></param>
        /// <param name="commentBusiness"></param>
        public AppChoicenessController(IChoicenessBusiness choicenessBusiness, ICommentBusiness commentBusiness, IQiniuUploadBusiness qiniuUploadBusiness)
        {
            _choicenessBusiness = choicenessBusiness;
            _commentBusiness = commentBusiness;
            _qiniuUploadBusiness = qiniuUploadBusiness;
        }

        /// <summary>
        /// 首页--精选
        /// </summary>
        /// <param name="loginUserId">登录用户编号（可空）</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        [Route("choicenessvideos"), HttpGet]
        public ResponsePackage<AppChoicenesssView> ChoicenessVideos(int? loginUserId, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._choicenessBusiness.ChoicenessVideos(loginUserId, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 首页--朋友(需要登录)
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        [Route("friendvideos"), HttpGet, CheckAppLogin]
        public ResponsePackage<AppChoicenesssView> FriendVideos(int loginUserId, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._choicenessBusiness.FriendVideos(loginUserId, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 首页--精选--评论
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        [Route("videocomments"), HttpGet]
        public ResponsePackage<AppComments> VideoComments(int videoId, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._choicenessBusiness.VideoComments(videoId, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 首页--精选--评论详细
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <param name="pid">上级评论编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        [Route("videocomments"), HttpGet]
        public ResponsePackage<AppCommentsView> VideoComments(int videoId, int pid, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._choicenessBusiness.VideoComments(videoId, pid, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 首页--精选--喜欢
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        [Route("videocollections"), HttpGet]
        public ResponsePackage<AppUserCollectionsView> VideoCollections(int videoId, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._choicenessBusiness.VideoCollections(videoId, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 首页--精选--评论--举报
        /// </summary>
        /// <param name="reportComment"></param>
        /// <returns></returns>
        [Route("reportcomment"), HttpGet,CheckAppLogin]
        public ResponsePackage<bool> ReportComment([FromBody]UserReportCommentPara reportComment)
        {
            return CommonResult(() => this._choicenessBusiness.ReportComment(reportComment.LoginUserId, reportComment.CommentId), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 首页--精选--关注
        /// </summary>
        /// <param name="userFans"></param>
        /// <returns></returns>
        [Route("usersubscribe"), HttpPost, CheckAppLogin]
        public ResponsePackage<bool> UserSubscribe([FromBody]UserFansPara userFans)
        {
            return CommonResult(() => this._choicenessBusiness.UserSubscribe(userFans.LoginUserId, userFans.SubscribeUserId), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 首页--精选--取消关注
        /// </summary>
        /// <param name="userFans"></param>
        /// <returns></returns>
        [Route("unusersubscribe"), HttpPost, CheckAppLogin]
        public ResponsePackage<bool> UserCancelSubscribe([FromBody]UserFansPara userFans)
        {
            return CommonResult(() => this._choicenessBusiness.UserCancelSubscribe(userFans.LoginUserId, userFans.SubscribeUserId), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 首页--精选--发表评论
        /// </summary>
        /// <param name="videoComment"></param>
        /// <returns></returns>
        [Route("createvideocomment"), HttpPost, CheckAppLogin]
        public ResponsePackage<int> CreateVideoComment([FromBody]VideoCommentPara videoComment)
        {
            return CommonResult(() => this._choicenessBusiness.CreateVideoComment(videoComment.LoginUserId, videoComment.VideoId, videoComment.Content), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 首页--精选--回复评论
        /// </summary>
        /// <param name="videoComment"></param>
        /// <returns></returns>
        [Route("replyvideocomment"), HttpPost, CheckAppLogin]
        public ResponsePackage<int> ReplyVideoComment([FromBody]ReplyVideoCommentPara videoComment)
        {
            return CommonResult(() => this._choicenessBusiness.ReplyVideoComment(videoComment.LoginUserId, videoComment.VideoId, videoComment.CommentId, videoComment.Content), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 首页--精选--删除评论
        /// </summary>
        /// <param name="videoComment"></param>
        /// <returns></returns>
        [Route("deletevideocomment"), HttpPost, CheckAppLogin]
        public ResponsePackage<bool> DeleteVideoComment([FromBody]DeleteVideoCommentPara videoComment)
        {
            this._commentBusiness.UserId = videoComment.LoginUserId;
            return CommonResult(() => this._commentBusiness.DeleteVideoComment(videoComment.CommentId), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 首页--精选--喜欢视频
        /// </summary>
        /// <param name="userCollect"></param>
        /// <returns></returns>
        [Route("collectvideo"), HttpPost, CheckAppLogin]
        public ResponsePackage<bool> CollectVideo([FromBody]UserCollectPara userCollect)
        {
            return CommonResult(() => this._choicenessBusiness.CollectVideo(userCollect.LoginUserId, userCollect.VideoId), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 首页--精选--取消喜欢
        /// </summary>
        /// <returns></returns>
        [Route("uncollectvideo"), HttpPost, CheckAppLogin]
        public ResponsePackage<bool> UnCollectVideo([FromBody]UserCollectPara userCollect)
        {
            return CommonResult(() => this._choicenessBusiness.UnCollectVideo(userCollect.LoginUserId, userCollect.VideoId), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 首页--上传--获取token
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="type">video:视频 pic:头像 album:专辑 cover:视频封面 banner：板块图片</param>
        /// <returns></returns>
        [Route("getuploadtoken"), HttpGet, CheckAppLogin]
        public ResponsePackage<AppUploadTokenView> GetUploadToken(int loginUserId, string type)
        {
            return CommonResult(() => _qiniuUploadBusiness.GetAppUploadToken(type,  loginUserId), r => Console.WriteLine(r.ToJSON()));
        }
    }
}
