using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using Newtonsoft.Json.Linq;

namespace HKSJ.WBVV.Api
{
    public class TagsController : ApiControllerBase
    {
        private readonly ITagsBusiness _tagsBusiness;
        public TagsController(ITagsBusiness tagsBusiness)
        {
            _tagsBusiness = tagsBusiness;
        }
        /// <summary>
        /// 推荐标签
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IList<TagsView> GetTags()
        {
            return this._tagsBusiness.GetTags();
        }

        /// <summary>
        /// 用户搜索标签
        /// </summary>
        /// <param name="search">搜索内容</param>
        /// <returns></returns>
        [HttpGet]
        public IList<TagsView> GetTags(string search)
        {
            return this._tagsBusiness.GetTags(search);
        }
        /// <summary>
        /// 推荐标签视图（按分类来分组）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IList<HKSJ.WBVV.Entity.ViewModel.Manage.CategoryTagsView> GetTagsGroupbyCategoryId()
        {
            return this._tagsBusiness.GetTagsGroupbyCategoryId();
        }
        /// <summary>
        /// 某个分类下的推荐标签视图(后台，取所有的)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IList<TagsView> GetTagsByCategoryId(int id)
        {
            return this._tagsBusiness.GetTagsByCategoryId(id);
        }
        /// <summary>
        /// 某个分类下的推荐标签视图(前端的，取前20个)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IList<TagsView> GetTagsOfWebByCategoryId(int id)
        {
            return this._tagsBusiness.GetTagsOfWebByCategoryId(id);
        }
        /// <summary>
        /// 更新标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result UpdateTags()
        {
            this._tagsBusiness.UserId = JObject.Value<int>("uid");
            return CommonResult(() => this._tagsBusiness.UpdateTags(JObject.Value<int>("id"), JObject.Value<string>("name"), JObject.Value<int>("sort")), (r) => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 删除多个标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result DeleteTags()
        {
            this._tagsBusiness.UserId = JObject.Value<int>("uid");
            var ids = JObject.Value<JArray>("ids");
            return CommonResult(() => this._tagsBusiness.DeleteTags(ids == null ? null : ids.ToObject<IList<int>>()), (r) => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 添加标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result AddTags()
        {
            this._tagsBusiness.UserId = JObject.Value<int>("uid");
            return CommonResult(() => this._tagsBusiness.CreateTags(JObject.Value<string>("name"), JObject.Value<int>("sort"), JObject.Value<int>("categoryid")), (r) => Console.WriteLine(r.ToJSON()));
        }
    }
}
