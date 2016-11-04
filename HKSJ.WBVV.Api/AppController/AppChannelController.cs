using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HKSJ.WBVV.Api.AppController.Base;
using HKSJ.WBVV.Business.Interface.APP;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.Response;
using HKSJ.WBVV.Entity.Response.App;

namespace HKSJ.WBVV.Api.AppController
{
    /// <summary>
    /// 频道
    /// </summary>
    [RoutePrefix("api/app/channel")]
    public class AppChannelController : AppApiControllerBase
    {
        private readonly IChannelBusiness _channelBusiness;
        private readonly IChoicenessBusiness _choicenessBusiness;

        /// <summary>
        /// AppChannelController
        /// </summary>
        /// <param name="channelBusiness"></param>
        /// <param name="choicenessBusiness"></param>
        public AppChannelController(IChannelBusiness channelBusiness, IChoicenessBusiness choicenessBusiness)
        {
            _channelBusiness = channelBusiness;
            _choicenessBusiness = choicenessBusiness;
        }

        /// <summary>
        /// 频道--频道信息
        /// </summary>
        /// <param name="loginUserId">登录用户编号（可空）</param>
        /// <returns></returns>
        [Route("channel"), HttpGet]
        public ResponsePackage<AppChannelView> Channel(int? loginUserId)
        {
            return CommonResult(() => this._channelBusiness.Channel(loginUserId), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 频道--分类视频
        /// </summary>
        /// <param name="loginUserId">登录用户编号（可空）</param>
        /// <param name="cateId">分类编号</param>
        /// <param name="pageSize">显示多少行</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        [Route("videos"), HttpGet]
        public ResponsePackage<AppChoicenesssView> Videos(int? loginUserId, int cateId, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._choicenessBusiness.Videos(loginUserId, cateId, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 上传--分类
        /// </summary>
        /// <param name="pageSize">显示多少行</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        [Route("category"), HttpGet]
        public ResponsePackage<AppCategorysView> Category(int pageSize, int pageIndex)
        {
            return CommonResult(() => this._channelBusiness.Category(pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }
    }
}
