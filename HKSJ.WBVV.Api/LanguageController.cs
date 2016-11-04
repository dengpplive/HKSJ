using System;
using System.Data;
using System.Collections.Generic;
using System.Web.Http;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Entity.ViewModel.Client;
using Newtonsoft.Json;
using HKSJ.WBVV.Api.Filters;

namespace HKSJ.WBVV.Api
{
    public class LanguageController : ApiControllerBase
    {

        private readonly ILanguageBusiness _ilanguageBusiness;
        public LanguageController(ILanguageBusiness ilanguageBusiness)
        {
            this._ilanguageBusiness = ilanguageBusiness;
        }

        /// <summary>
        /// 根据当前线程语言类型获取语言数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, CheckApp]
        public Result GetAllTexts()
        {
            return CommonResult(() => this._ilanguageBusiness.GetAllTexts(), (r) => Console.WriteLine(r.ToJSON()));
        }


    }
}