using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ViewModel;

namespace HKSJ.WBVV.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class UserLevelController : ApiControllerBase
    {
        private readonly IUserLevelBusiness _userLevelBusiness;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLevelBusiness"></param>
        public UserLevelController(IUserLevelBusiness userLevelBusiness)
        {
            _userLevelBusiness = userLevelBusiness;
        }
        /// <summary>
        /// 用户等级列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PageResult GetUserLevelPageResult()
        {
            this._userLevelBusiness.PageIndex = PageIndex;
            this._userLevelBusiness.PageSize = PageSize;
            return this._userLevelBusiness.GetUserLevelPageResult(this.Condtions, this.OrderCondtions);
        }
        /// <summary>
        /// 获取用户等级信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public UserLevel GetUserLevel(int id)
        {
            return this._userLevelBusiness.GetUserLevel(id);
        }
        /// <summary>
        /// 添加用户等级
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result CreateUserLevel()
        {
            return CommonResult(() => this._userLevelBusiness.CreateUserLevel(JObject.Value<string>("levelName"),
                JObject.Value<int>("levelStart"), JObject.Value<int>("levelEnd"), JObject.Value<string>("levelIcon"),
                JObject.Value<int>("createUserId")), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 修改用户等级
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result UpdateUserLevel()
        {
            return CommonResult(() => this._userLevelBusiness.UpdateUserLevel(JObject.Value<int>("id"), JObject.Value<string>("levelName"),
                JObject.Value<int>("levelStart"), JObject.Value<int>("levelEnd"), JObject.Value<string>("levelIcon"),
                JObject.Value<int>("createUserId")), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 积分规则分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PageResult GetUserScoreRulePageResult()
        {
            this._userLevelBusiness.PageIndex = PageIndex;
            this._userLevelBusiness.PageSize = PageSize;
            return this._userLevelBusiness.GetUserScoreRulePageResult(this.Condtions, this.OrderCondtions);
        }
        /// <summary>
        /// 获取积分规则信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public UserScoreRule GetUserScoreRule(int id)
        {
            return this._userLevelBusiness.GetUserScoreRule(id);
        }
        /// <summary>
        /// 添加积分规则
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result CreateUserScoreRule()
        {
            return CommonResult(() => this._userLevelBusiness.CreateUserScoreRule(JObject.Value<string>("name"), JObject.Value<int>("score"),
                JObject.Value<int>("limitScore"), JObject.Value<int>("createUserId")), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 修改积分规则
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result UpdateUserScoreRule(int id, string name, int score, int limitScore, int createUserId)
        {
            return CommonResult(() => this._userLevelBusiness.UpdateUserScoreRule(JObject.Value<int>("id"), JObject.Value<string>("name"), JObject.Value<int>("score"),
               JObject.Value<int>("limitScore"), JObject.Value<int>("createUserId")), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 禁用积分规则
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result DisableUserScoreRule()
        {
            return CommonResult(() => this._userLevelBusiness.DisableUserScoreRule(JObject.Value<int>("id"), JObject.Value<int>("createUserId")), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 启用积分规则
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result EnabledUserScoreRule(int id, int createUserId)
        {
            return CommonResult(() => this._userLevelBusiness.EnabledUserScoreRule(JObject.Value<int>("id"), JObject.Value<int>("createUserId")), r => Console.WriteLine(r.ToJSON()));
        }
    }
}
