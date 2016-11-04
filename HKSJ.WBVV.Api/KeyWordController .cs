using System;
using System.Data;
using System.Collections.Generic;
using System.Web.Http;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Entity.ViewModel.Client;
using Newtonsoft.Json;

namespace HKSJ.WBVV.Api
{
    public class KeyWordController : ApiControllerBase
    {

        private readonly IKeyWordsBusiness _iKeyWordsBusiness;

        public KeyWordController(IKeyWordsBusiness iKeyWordsBusiness)
        {
            this._iKeyWordsBusiness = iKeyWordsBusiness;
        }

        [HttpGet]
        public Result GetHotKeyWords()
        {
            return CommonResult(() => this._iKeyWordsBusiness.GetHotKeyWords(), (r) => Console.WriteLine(r.ToJSON()));
        }

        [HttpGet]
        public Result GetFilteredKeyword(string keyword)
       {            
            return CommonResult(() => this._iKeyWordsBusiness.GetFilteredKeyword(keyword),
                (r) => Console.WriteLine(r.ToJSON()));
        }
        [HttpPost]
        public Result AddOrUpdateAKeyWord(string keyword)
        {
            return CommonResult(() => this._iKeyWordsBusiness.AddOrUpdateAKeyWord(keyword),
                (r) => Console.WriteLine(r.ToJSON())); 
        }

        [HttpPost]
        public PageResult GetKeyWordsPageResult()
        {
            this._iKeyWordsBusiness.PageIndex = this.PageIndex;
            this._iKeyWordsBusiness.PageSize = this.PageSize;
            return this._iKeyWordsBusiness.GetKeyWordsPageResult(this.Condtions, this.OrderCondtions);
        }

        [HttpGet]
        public Result GetAKeyWordById(int id)
        {
            return CommonResult(() => this._iKeyWordsBusiness.GetAKeyWordById(id), (r) => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result UpdateAKeyWord()
        {
            var jsonData = JObject.Value<string>("dataModel");
            var keyword = JsonConvert.DeserializeObject<KeyWords>(jsonData);
            return CommonResult(() => this._iKeyWordsBusiness.UpdateAKeyWord(keyword), (r) => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result AddAKeyWord()
        {
            var jsonData = JObject.Value<string>("dataModel");
            var keyword = JsonConvert.DeserializeObject<KeyWords>(jsonData);
            return CommonResult(() => this._iKeyWordsBusiness.AddAKeyWord(keyword), (r) => Console.WriteLine(r.ToJSON()));
        }

        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result DelAKeyWord()
        {
            return CommonResult(() => this._iKeyWordsBusiness.DelAKeyWord(JObject.Value<int>("id")), (r) => Console.WriteLine(r.ToJSON()));
        }




    }
}
