using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HKSJ.WBVV.Api.AppController.Base;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Business.Interface.APP;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.RequestPara;
using HKSJ.WBVV.Entity.RequestPara.App;
using HKSJ.WBVV.Entity.Response;
using HKSJ.WBVV.Entity.Response.App;

namespace HKSJ.WBVV.Api.AppController
{
    /// <summary>
    /// 热榜
    /// </summary>
    [RoutePrefix("api/app/hotlist")]
    public class AppHotListController : AppApiControllerBase
    {
        private readonly IHotListBusiness _hotListBusiness;

        /// <summary>
        /// AppHotListController
        /// </summary>
        public AppHotListController(IHotListBusiness hotListBusiness)
        {
            _hotListBusiness = hotListBusiness;
        }
        /// <summary>
        /// 热榜--星播客
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        [Route("uploadrank"), HttpGet]
        public ResponsePackage<AppUsersView> UploadRank(int? loginUserId, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._hotListBusiness.UploadRank(loginUserId, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 热榜--搜索结果
        /// </summary>
        /// <param name="content">搜索内容</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        [Route("serach"), HttpGet]
        public ResponsePackage<AppSerachView> Serach(string content, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._hotListBusiness.Serach(content, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        ///热榜--搜索结果--视频详情
        /// </summary>
        /// <param name="content">搜索内容</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        [Route("serachvideo"), HttpGet]
        public ResponsePackage<AppSerachVideoView> SerachVideo(string content, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._hotListBusiness.SerachVideo(content, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 热榜--搜索结果--用户详情
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="content">搜索内容</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        [Route("serachuser"), HttpGet]
        public ResponsePackage<AppSerachUserView> SerachUser(int? loginUserId,string content, int pageSize, int pageIndex)
        {
            return CommonResult(() => this._hotListBusiness.SerachUser(loginUserId,content, pageSize, pageIndex), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 热榜--审片
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <returns></returns>
        [Route("videoprereview"), HttpGet, CheckAppLogin]
        public ResponsePackage<AppVideoView> VideoPrereview(int loginUserId)
        {
            return CommonResult(() => this._hotListBusiness.VideoPrereview(loginUserId), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 热榜--审片--必火
        /// </summary>
        /// <param name="videoPrereview"></param>
        /// <returns></returns>
        [Route("good"), HttpPost, CheckAppLogin]
        public ResponsePackage<bool> Good([FromBody]VideoPrereviewPara videoPrereview)
        {
            return CommonResult(() => this._hotListBusiness.Good(videoPrereview.LoginUserId, videoPrereview.VideoId), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 热榜--审片--差评
        /// </summary>
        /// <param name="videoPrereview"></param>
        /// <returns></returns>
        [Route("bad"), HttpPost, CheckAppLogin]
        public ResponsePackage<bool> Bad([FromBody]VideoPrereviewPara videoPrereview)
        {
            return CommonResult(() => this._hotListBusiness.Bad(videoPrereview.LoginUserId, videoPrereview.VideoId), r => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 热榜--审片--举报视频
        /// </summary>
        /// <param name="reportVideo"></param>
        /// <returns></returns>
        [Route("reportvideo"), HttpPost, CheckAppLogin]
        public ResponsePackage<bool> ReportVideo([FromBody]UserReportVideoPara reportVideo)
        {
            return CommonResult(() => this._hotListBusiness.ReportVideo(reportVideo.LoginUserId, reportVideo.VideoId), r => Console.WriteLine(r.ToJSON()));
        }
    }
}
