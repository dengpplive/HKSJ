using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using Newtonsoft.Json.Linq;

namespace HKSJ.WBVV.Api
{
    public class SystemMessageController : ApiControllerBase
    {
        private readonly ISystemMessageBusiness _systemMessageBusiness;
        public SystemMessageController(ISystemMessageBusiness systemMessageBusiness)
        {
            this._systemMessageBusiness = systemMessageBusiness;
        }

        /// <summary>
        /// 添加系统消息
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result CreateSystemMessage()
        {

            return CommonResult(() =>
            {
                this._systemMessageBusiness.UserId = JObject.Value<int>("loginUserId");
                return this._systemMessageBusiness.CreateSystemMessage(
                    JObject.Value<short>("userByType"),
                    JObject.Value<string>("selectUser"),
                    JObject.Value<string>("messageDesc")
                    );
            }
               , (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public SystemMessageUserView GetUsers()
        {
            return this._systemMessageBusiness.GetUsers(JObject.Value<string>("userBy"));
        }

        /// <summary>
        /// 获取系统消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public SysMessage GetSystemMessage(int id)
        {
            return this._systemMessageBusiness.GetSystemMessage(id);
        }

        /// <summary>
        /// 系统列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PageResult GetSystemMessagePageResult()
        {
            this._systemMessageBusiness.PageIndex = this.PageIndex;
            this._systemMessageBusiness.PageSize = this.PageSize;
            return this._systemMessageBusiness.GetSystemMessagePageResult(this.Condtions, this.OrderCondtions);
        }
    }
}
