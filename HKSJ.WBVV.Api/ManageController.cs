using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using System.Web.Http;
using HKSJ.WBVV.Api.Filters;

namespace HKSJ.WBVV.Api
{
    public class ManageController : ApiControllerBase
    {
        private readonly IManageBusiness _manageBusiness;
        public ManageController(IManageBusiness manageBusiness)
        {
            this._manageBusiness = manageBusiness;
        }
        /// <summary>
        /// 管理员登陆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result ManageLogin()
        {
            return CommonResult(() => this._manageBusiness.GetManageView(
                JObject.Value<string>("loginName"),
                JObject.Value<string>("password")
                ), (r) => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, CheckApp]
        public ManageView GetManage(string loginName)
        {
            return this._manageBusiness.GetManageView(loginName);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp]
        public Result ChangePwd()
        {
            this._manageBusiness.UserId = JObject.Value<int>("id");
            return CommonResult(() => this._manageBusiness.ChangePwd(
                    JObject.Value<string>("oldPwd"),
                    JObject.Value<string>("newPwd"),
                    JObject.Value<string>("confirmPwd")
        ), (r) => Console.WriteLine(r.ToJSON()));
        }
    }
}
