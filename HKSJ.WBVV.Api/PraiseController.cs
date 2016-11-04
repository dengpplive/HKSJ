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
using Newtonsoft.Json.Linq;

namespace HKSJ.WBVV.Api
{
    public class PraiseController : ApiControllerBase
    {
        private readonly IPraisesBusiness _praisesBusiness;
        public PraiseController(IPraisesBusiness praisesBusiness)
        {
            this._praisesBusiness = praisesBusiness;
        }

        /// <summary>
        /// 赞评论
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result CreatePraisesComment()
        {
            return CommonResult(() => this._praisesBusiness.CreatePraisesComment(JObject.Value<int>("userId"), JObject.Value<int>("commentId")), (r) => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 取消赞评论
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
         [HttpPost]
        public Result CancelPraisesComment()
        {
            return CommonResult(() => this._praisesBusiness.CancelPraisesComment(JObject.Value<int>("userId"), JObject.Value<int>("commentId")), (r) => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 赞视频
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result CreatePraisesVedio()
        {
            return CommonResult(() => this._praisesBusiness.CreatePraisesVedio(JObject.Value<int>("userId"), JObject.Value<int>("vedioId")), (r) => Console.WriteLine(r.ToJSON()));
        }
    }
}
