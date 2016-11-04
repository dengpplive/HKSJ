using System;
using System.Data;
using System.Collections.Generic;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Common.Extender;

namespace HKSJ.WBVV.Api
{
    public class DictionaryItemController : ApiControllerBase
    {
        private readonly IDictionaryItemBusiness _dictionaryItemBusiness;
        public DictionaryItemController(IDictionaryItemBusiness dictionaryItemBusiness)
        {
            this._dictionaryItemBusiness = dictionaryItemBusiness;

        }

        /// <summary>
        /// 增加节点
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result CreateDictionaryItem()
        {
            return CommonResult(() => this._dictionaryItemBusiness.CreateDictionaryItem(JObject.Value<string>("name"), JObject.Value<int>("pid")), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result DeleteDictionaryItem()
        {
            return CommonResult(() => this._dictionaryItemBusiness.DeleteDictionaryItem(JObject.Value<int>("id")), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 修改节点
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result UpdateDictionaryItem()
        {
            return CommonResult(() => this._dictionaryItemBusiness.UpdateDictionaryItem(JObject.Value<string>("name"), JObject.Value<int>("id")), (r) => Console.WriteLine(r.ToJSON()));
        }

    }
}