using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HKSJ.WBVV.Api.AppController.Base;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Business.Interface.APP;
using HKSJ.WBVV.Business.Interface.UserCenter;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.Response;
using HKSJ.WBVV.Entity.Response.App;
using HKSJ.WBVV.Entity.RequestPara.App;

namespace HKSJ.WBVV.Api.AppController
{
    /// <summary>
    /// 用户空间
    /// </summary>
    [RoutePrefix("api/app/userspace")]
    public class AppUserSpaceController : AppApiControllerBase
    {
        private readonly IUserSpaceBusiness _userSpaceBusiness;
        private readonly IMessageBusiness _messageBusiness;
        private readonly ICommentBusiness _commentBusiness;

        /// <summary>
        /// AppUserSpaceController
        /// </summary>
        /// <param name="userSpaceBusiness"></param>
        /// <param name="messageBusiness"></param>
        /// <param name="commentBusiness"></param>
        public AppUserSpaceController(IUserSpaceBusiness userSpaceBusiness, IMessageBusiness messageBusiness, ICommentBusiness commentBusiness)
        {
            _userSpaceBusiness = userSpaceBusiness;
            _messageBusiness = messageBusiness;
            _commentBusiness = commentBusiness;
        }

        /// <summary>
        /// 我的空间--他的空间
        /// </summary>
        /// <param name="loginUserId">登录用户编号(可空)</param>
        /// <param name="userId">被浏览用户空间编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        [Route("userspace"), HttpGet]
        public ResponsePackage<AppUserSpaceView> UserSpace(int loginUserId, int userId, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._userSpaceBusiness.UserSpace(loginUserId, userId, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 我的空间--粉丝
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="userId">被访问的用户编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        [Route("userfans"), HttpGet]
        public ResponsePackage<AppUserFanssView> UserFans(int loginUserId,int userId, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._userSpaceBusiness.UserFans(loginUserId, userId,pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 我的空间--关注
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="userId">被访问的用户编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        [Route("usersubscribe"), HttpGet]
        public ResponsePackage<AppUserFanssView> UserSubscribe(int loginUserId,int userId, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._userSpaceBusiness.UserSubscribe(loginUserId,userId, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 我的空间--我的消息
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        [Route("message"), HttpGet, CheckAppLogin]
        public ResponsePackage<AppMessageView> Message(int loginUserId, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._userSpaceBusiness.Message(loginUserId, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 我的空间--我的消息--清空系统消息
        /// </summary>
        /// <param name="clearSystemMessage"></param>
        /// <returns></returns>
        [Route("clearsystemmessage"), HttpPost, CheckAppLogin]
        public ResponsePackage<bool> ClearSystemMessage([FromBody]ClearSystemMessagePara clearSystemMessage)
        {
            this._messageBusiness.UserId = clearSystemMessage.LoginUserId;
            return CommonResult(() => this._messageBusiness.ClearSystemMessage(), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        ///  我的空间--我的消息--评论
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="pageSize">显示多少行</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        [Route("comments"), HttpGet, CheckAppLogin]
        public ResponsePackage<AppVideoCommentsView> Comments(int loginUserId, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._userSpaceBusiness.VideoComments(loginUserId, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        ///  我的空间--我的消息--删除评论
        /// </summary>
        /// <param name="videoComment"></param>
        /// <returns></returns>
        [Route("deletevideocomment"), HttpGet, CheckAppLogin]
        public ResponsePackage<bool> DeleteVideoComment([FromBody]DeleteVideoCommentPara videoComment)
        {
            this._messageBusiness.UserId = videoComment.LoginUserId;
            return CommonResult(() => this._messageBusiness.DeleteVideoComment(videoComment.CommentId), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        ///  我的空间--我的消息--喜欢
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="pageSize">显示多少行</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        [Route("collections"), HttpGet, CheckAppLogin]
        public ResponsePackage<AppUserCollectsView> Collections(int loginUserId, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._userSpaceBusiness.VideoCollections(loginUserId, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 我的空间--留言
        /// </summary>
        /// <param name="userId">登录用户编号或者浏览用户编号</param>
        /// <param name="pageSize">显示多少行</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        [Route("spacecomments"), HttpGet]
        public ResponsePackage<AppComments> SpaceComments(int userId, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._userSpaceBusiness.SpaceComments(userId, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 我的空间--留言详细
        /// </summary>
        /// <param name="userId">登录用户编号或者浏览用户编号</param>
        /// <param name="pid">上级评论编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        [Route("spacecomments"), HttpGet]
        public ResponsePackage<AppCommentsView> SpaceComments(int userId, int pid, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._userSpaceBusiness.SpaceComments(userId, pid, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        ///  我的空间--发表留言
        /// </summary>
        /// <returns></returns>
        [Route("createspacecomment"), HttpPost, CheckAppLogin]
        public ResponsePackage<int> CreateSpaceComment([FromBody]SpaceCommentPara spaceComment)
        {
            this._commentBusiness.UserId = spaceComment.LoginUserId;
            return CommonResult(() => this._commentBusiness.CreateSpaceComment(spaceComment.OwnerUserId, spaceComment.Content), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        ///  我的空间--回复留言
        /// </summary>
        /// <returns></returns>
        [Route("replyspacecomment"), HttpPost, CheckAppLogin]
        public ResponsePackage<int> ReplySpaceComment([FromBody]ReplySpaceCommentPara spaceComment)
        {
            this._commentBusiness.UserId = spaceComment.LoginUserId;
            return CommonResult(() => this._commentBusiness.ReplySpaceComment(spaceComment.OwnerUserId, spaceComment.CommentId, spaceComment.Content), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        ///  我的空间--删除留言
        /// </summary>
        /// <returns></returns>
        [Route("deletespacecomment"), HttpPost, CheckAppLogin]
        public ResponsePackage<bool> DeleteSpaceComment([FromBody]DeleteSpaceCommentPara spaceComment)
        {
            this._commentBusiness.UserId = spaceComment.LoginUserId;
            return CommonResult(() => this._commentBusiness.DeleteSpaceComment(spaceComment.CommentId), r => Console.WriteLine(r.ToJSON()));
        }
    }
}
