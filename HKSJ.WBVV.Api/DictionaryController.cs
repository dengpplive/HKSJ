
using System;
using System.Data;
using System.Collections.Generic;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Common.Extender;

namespace HKSJ.WBVV.Api
{
    public class DictionaryController : ApiControllerBase
    {
        private readonly IDictionaryBusiness _dictionaryBusiness;
        public DictionaryController(IDictionaryBusiness dictionaryBusiness)
        {
            this._dictionaryBusiness = dictionaryBusiness;

        }
        /// <summary>
        /// 一级分类下的筛选列表
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<DictionaryView> GetDictionaryViewList(int categoryId)
        {
            return this._dictionaryBusiness.GetDictionaryViewList(categoryId);
        }

        /// <summary>
        /// 一级分类下的属性列表
        /// Author:axone
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<DictionaryView> GetDictionaryAndItemViewList(int cid)
        {
            return _dictionaryBusiness.GetDictionaryAndItemViewList(cid);
        }

        /// <summary>
        /// 获取选中的过滤条件
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<DictionaryView> GetDictionaryViewList(int categoryId, string filter)
        {
            return this._dictionaryBusiness.GetDictionaryViewList(categoryId, filter);
        }

        /// <summary>
        /// 后台属性数据管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IList<HKSJ.WBVV.Entity.ViewModel.Manage.CategoryView> GetCategoryAndDictionaryViewList()
        {
            return this._dictionaryBusiness.GetCategoryAndDictionaryViewList();
        }

        /// <summary>
        /// 增加字典
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result CreateDictionary()
        {
            return CommonResult(() => this._dictionaryBusiness.CreateDictionary(JObject.Value<string>("name"), JObject.Value<int>("pid")), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 删除字典（包含子节点一并删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result DeleteDictionary()
        {
            return CommonResult(() => this._dictionaryBusiness.DeleteDictionary(JObject.Value<int>("id")), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result UpdateDictionary()
        {
            return CommonResult(() => this._dictionaryBusiness.UpdateDictionary(JObject.Value<string>("name"), JObject.Value<int>("id")), (r) => Console.WriteLine(r.ToJSON()));
        }


    }
}