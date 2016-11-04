using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using Newtonsoft.Json.Linq;

namespace HKSJ.WBVV.Api
{
    public class CommentController : ApiControllerBase
    {
        private readonly ICommentBusiness _commentBusiness;
        public CommentController(ICommentBusiness commentBusiness)
        {
            this._commentBusiness = commentBusiness;

        }

        /// <summary>
        /// 视频子级分页评论列表
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="loginUserId"></param>
        /// <param name="pId"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public PageResult GetVideoComments(int videoId,int loginUserId, int pId, int size, int index)
        {
            this._commentBusiness.PageIndex = PageIndex;
            this._commentBusiness.PageSize = PageSize;
            this._commentBusiness.UserId = loginUserId;
            return this._commentBusiness.GetVideoComments(videoId, pId,size, index);
        }

        /// <summary>
        /// 视频分页评论列表
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <param name="loginUserId"></param>
        /// <param name="size">子评论显示条数</param>
        /// <param name="index">子评论显示页数</param>
        /// <returns></returns>
        [HttpGet]
        public PageResult GetVideoComments(int videoId, int loginUserId, int size, int index)
        {
            this._commentBusiness.PageIndex = PageIndex;
            this._commentBusiness.PageSize = PageSize;
            this._commentBusiness.UserId = loginUserId;
            return this._commentBusiness.GetVideoComments(videoId, size, index);
        }

        /// <summary>
        /// 发表视频评论
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result CreateVideoComment()
        {
            this._commentBusiness.UserId = JObject.Value<int>("userId");
            return CommonResult(() => this._commentBusiness.CreateVideoComment(JObject.Value<int>("videoId"), JObject.Value<string>("commentContent")), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 回复视频评论
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result ReplyVideoComment()
        {
            this._commentBusiness.UserId = JObject.Value<int>("userId");
            return CommonResult(() => this._commentBusiness.ReplyVideoComment(JObject.Value<int>("videoId"), JObject.Value<int>("commentId"), JObject.Value<string>("commentContent")), (r) => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 删除视频评论
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result DeleteVideoComment()
        {
            this._commentBusiness.UserId = JObject.Value<int>("userId");
            return CommonResult(() => this._commentBusiness.DeleteVideoComment(JObject.Value<int>("commentId")), (r) => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 用户空间下子级分页留言列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loginUserId"></param>
        /// <param name="pId"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public PageResult GetSpaceComments(int userId, int loginUserId, int pId, int size, int index)
        {
            this._commentBusiness.PageIndex = PageIndex;
            this._commentBusiness.PageSize = PageSize;
            this._commentBusiness.UserId = loginUserId;
            return this._commentBusiness.GetSpaceComments(userId, pId, size, index);
        }

        /// <summary>
        /// 用户空间下分页留言列表
        /// </summary>
        /// <param name="userId">用户空间编号</param>
        /// <param name="loginUserId"></param>
        /// <param name="size">子评论显示条数</param>
        /// <param name="index">子评论显示页数</param>
        /// <returns></returns>
        [HttpGet]
        public PageResult GetSpaceComments(int userId, int loginUserId, int size, int index)
        {
            this._commentBusiness.PageIndex = PageIndex;
            this._commentBusiness.PageSize = PageSize;
            this._commentBusiness.UserId = loginUserId;
            return this._commentBusiness.GetSpaceComments(userId, size, index);
        }

        /// <summary>
        /// 用户发表留言
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result CreateSpaceComment()
        {
            this._commentBusiness.UserId = JObject.Value<int>("userId");
            return CommonResult(() => this._commentBusiness.CreateSpaceComment(JObject.Value<int>("toUserId"), JObject.Value<string>("commentContent")), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 用户回复留言
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result ReplySpaceComment()
        {
            this._commentBusiness.UserId = JObject.Value<int>("userId");
            return CommonResult(() => this._commentBusiness.ReplySpaceComment(JObject.Value<int>("toUserId"), JObject.Value<int>("commentId"), JObject.Value<string>("commentContent")), (r) => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 删除留言
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result DeleteSpaceComment()
        {
            this._commentBusiness.UserId = JObject.Value<int>("userId");
            return CommonResult(() => this._commentBusiness.DeleteSpaceComment(JObject.Value<int>("commentId")), (r) => Console.WriteLine(r.ToJSON()));
        }
    }
}
