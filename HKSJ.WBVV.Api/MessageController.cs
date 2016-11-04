using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Business.Interface;
using System.Web.Http;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Business.Interface.UserCenter;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;

namespace HKSJ.WBVV.Api
{
    public class MessageController : ApiControllerBase
    {
        private readonly IMessageBusiness _messageBusiness;

        public MessageController(IMessageBusiness messageBusiness)
        {
            _messageBusiness = messageBusiness;
        }
        [HttpPost]
        public MessageType GetHeaderMessage()
        {
            this._messageBusiness.UserId = JObject.Value<int>("loginUserId");
            return this._messageBusiness.GetHeaderMessage();
        }
        [HttpPost]
        public IList<MessageType> GetMessages()
        {
            this._messageBusiness.UserId = JObject.Value<int>("loginUserId");
            return this._messageBusiness.GetMessages();
        }
        [HttpPost]
        public PageResult GetPageSystemMessages()
        {
            this._messageBusiness.UserId = JObject.Value<int>("loginUserId");
            this._messageBusiness.PageIndex = PageIndex;
            this._messageBusiness.PageSize = PageSize;
            return this._messageBusiness.GetPageSystemMessages();
        }
        [HttpPost]
        public MessageView GetSystemMessages()
        {
            this._messageBusiness.UserId = JObject.Value<int>("loginUserId");
            this._messageBusiness.PageIndex = PageIndex;
            this._messageBusiness.PageSize = PageSize;
            return this._messageBusiness.GetSystemMessages();
        }
        [HttpPost]
        public Result DeleteSystemMessage()
        {
            this._messageBusiness.UserId = JObject.Value<int>("loginUserId");
            return CommonResult(() => this._messageBusiness.DeleteSystemMessage(JObject.Value<int>("messageId")), (r) => Console.WriteLine(r.ToJSON()));
        }
        [HttpPost]
        public Result ClearSystemMessage()
        {
            this._messageBusiness.UserId = JObject.Value<int>("loginUserId");
            return CommonResult(() => this._messageBusiness.ClearSystemMessage(), (r) => Console.WriteLine(r.ToJSON()));
        }
        [HttpPost]
        public PageResult GetPageComments()
        {
            this._messageBusiness.UserId = JObject.Value<int>("loginUserId");
            this._messageBusiness.PageIndex = PageIndex;
            this._messageBusiness.PageSize = PageSize;
            return this._messageBusiness.GetPageComments(JObject.Value<int>("commmentPageSize"), JObject.Value<int>("commmentSize"));
        }
        [HttpPost]
        public MessageView GetComments()
        {
            this._messageBusiness.UserId = JObject.Value<int>("loginUserId");
            this._messageBusiness.PageIndex = PageIndex;
            this._messageBusiness.PageSize = PageSize;
            return this._messageBusiness.GetComments(JObject.Value<int>("commmentPageSize"), JObject.Value<int>("commmentSize"));
        }
        [HttpPost]
        public Result DeleteComment()
        {
            this._messageBusiness.UserId = JObject.Value<int>("loginUserId");
            return CommonResult(() => this._messageBusiness.DeleteVideoComment(JObject.Value<int>("messageId")), (r) => Console.WriteLine(r.ToJSON()));
        }
        [HttpPost]
        public PageResult GetPageUserMessages()
        {
            this._messageBusiness.UserId = JObject.Value<int>("loginUserId");
            this._messageBusiness.PageIndex = PageIndex;
            this._messageBusiness.PageSize = PageSize;
            return this._messageBusiness.GetPageUserMessages(JObject.Value<int>("messagePageSize"), JObject.Value<int>("commmentSize"));
        }
        [HttpPost]
        public MessageView GetUserMessages()
        {
            this._messageBusiness.UserId = JObject.Value<int>("loginUserId");
            this._messageBusiness.PageIndex = PageIndex;
            this._messageBusiness.PageSize = PageSize;
            return this._messageBusiness.GetUserMessages(JObject.Value<int>("messagePageSize"), JObject.Value<int>("commmentSize"));
        }
        [HttpPost]
        public Result DeleteUserMessage()
        {
            this._messageBusiness.UserId = JObject.Value<int>("loginUserId");
            return CommonResult(() => this._messageBusiness.DeleteSpaceComment(JObject.Value<int>("messageId")), (r) => Console.WriteLine(r.ToJSON()));
        }

    }
}
