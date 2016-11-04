using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Logger;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using Newtonsoft.Json.Linq;

namespace HKSJ.WBVV.Api
{
    /// <summary>
    /// 用户留言
    /// </summary>
    public class UserMessageController : ApiControllerBase
    {
        private readonly IUserMessageBusiness _userMessageBusiness;
        public UserMessageController(IUserMessageBusiness userMessageBusiness)
        {
            this._userMessageBusiness = userMessageBusiness;
        }
        /// <summary>
        /// 留言标记已读
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result ReadUserMessage()
        {
            this._userMessageBusiness.UserId = JObject.Value<int>("loginUserId");
            return CommonResult(() => this._userMessageBusiness.ReadUserMessage(JObject.Value<int>("messageId")), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 消息推送
        /// </summary>
        [HttpPost]
        public Result PushMessage()
        {
            return CommonResult(() =>
            {
                this._userMessageBusiness.UserId = JObject.Value<int>("loginUserId");
                return this._userMessageBusiness.PushMessage();
            }, (r) => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 发表留言
        /// </summary>
        [HttpPost]
        public Result Comment()
        {
            this._userMessageBusiness.UserId = JObject.Value<int>("loginUserId");
            return CommonResult(() => this._userMessageBusiness.Comment(
                JObject.Value<int>("userId"),
                JObject.Value<string>("messageContent")
                ), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 留言回复
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result Reply()
        {
            this._userMessageBusiness.UserId = JObject.Value<int>("loginUserId");
            return CommonResult(() => this._userMessageBusiness.Reply(
                        JObject.Value<int>("messageId"),
                        JObject.Value<int>("userId"),
                        JObject.Value<string>("messageContent")
                ), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 获取发表的留言
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public UserMessageDetialsView GetComment(int messageId)
        {
            return this._userMessageBusiness.GetComment(messageId);
        }

        /// <summary>
        /// 分页留言列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PageResult GetUserMessageDetialsPageResult()
        {
            this._userMessageBusiness.UserId = JObject.Value<int>("loginUserId");
            this._userMessageBusiness.PageIndex = this.PageIndex;
            this._userMessageBusiness.PageSize = this.PageSize;
            return this._userMessageBusiness.GetUserMessageDetialsPageResult(JObject.Value<int>("userId"));
        }

        /// <summary>
        /// 评论/留言分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PageResult GetUserMessageViewPageResult()
        {
            this._userMessageBusiness.PageIndex = this.PageIndex;
            this._userMessageBusiness.PageSize = this.PageSize;
            this._userMessageBusiness.UserId = JObject.Value<int>("loginUserId");
            return this._userMessageBusiness.GetUserMessageViewPageResult(this.Condtions, this.OrderCondtions);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result DeleteUserMessage()
        {
            this._userMessageBusiness.UserId = JObject.Value<int>("loginUserId");
            return CommonResult(() => this._userMessageBusiness.DeleteUserMessage(JObject.Value<int>("messageId")), (r) => Console.WriteLine(r.ToJSON()));
        }
    }
}
