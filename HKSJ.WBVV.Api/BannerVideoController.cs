using System;
using System.Collections;
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
using HKSJ.WBVV.Entity.ViewModel.Manage;
using Newtonsoft.Json.Linq;
using System.Web;
using HKSJ.WBVV.Api.Filters;

namespace HKSJ.WBVV.Api
{
    public class BannerVideoController : ApiControllerBase
    {
        private readonly IBannerVideoBusiness _bannerVideoBusiness;
        public BannerVideoController(IBannerVideoBusiness bannerVideoBusiness)
        {
            this._bannerVideoBusiness = bannerVideoBusiness;
        }

        /// <summary>
        /// 获取板块分页集合
        /// </summary>
        [HttpPost]
        public PageResult GetBannerPageResult()
        {
            this._bannerVideoBusiness.PageIndex = this.PageIndex;
            this._bannerVideoBusiness.PageSize = this.PageSize;
            return this._bannerVideoBusiness.GetBannerVideoPageResult(this.Condtions, this.OrderCondtions);
        }

        /// <summary>
        /// 添加Banner
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result CreateBannerVideo()
        {
            return CommonResult(() => this._bannerVideoBusiness.CreateBannerVideo(
                JObject.Value<string>("BannerImagePath"),
                JObject.Value<string>("BannerSmallImagePath"),
                JObject.Value<int>("CategoryId"),
                JObject.Value<int>("SortNum"),
                JObject.Value<int>("VideoId")
                ), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 修改Banner
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result UpdateBannerVideo()
        {
            return CommonResult(() => this._bannerVideoBusiness.UpdateBannerVideo(
               JObject.Value<int>("Id"),
               JObject.Value<int>("CategoryId"),
               JObject.Value<int>("VideoId"),
               JObject.Value<string>("BannerImagePath"),
               JObject.Value<string>("BannerSmallImagePath"),
               JObject.Value<int>("SortNum")
               ), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// Banner排序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result UpdateBannerVideoSort()
        {
            BannerVideo bannerVideo;
            IList<BannerVideo> plateList = new List<BannerVideo>();
            var data = JObject.Value<JArray>("data");
            for (int i = 0; i < data.Count; i++)
            {
                bannerVideo = new BannerVideo();
                bannerVideo.Id = Convert.ToInt32(data[i]["Id"].ToString());
                bannerVideo.SortNum = Convert.ToInt32(data[i]["SortNum"].ToString());
                plateList.Add(bannerVideo);
            }
            return CommonResult(() => this._bannerVideoBusiness.UpdateBannerVideoSort(plateList), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 单个Banner信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public BannerVideo GetBannerVideo(int id)
        {
            return this._bannerVideoBusiness.GetBannerVideo(id);
        }

        /// <summary>
        /// 删除Banner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result DeleteBannerVideo()
        {
            return CommonResult(() => this._bannerVideoBusiness.DeleteBannerVideo(JObject.Value<int>("id")), (r) => Console.WriteLine(r.ToJSON()));
        }
        /// <summary>
        /// 删除多个Banner
        /// </summary>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result DeleteBannerVideos()
        {
            var ids = JObject.Value<JArray>("ids");
            return CommonResult(() => this._bannerVideoBusiness.DeleteBannerVideos(ids == null ? null : ids.ToObject<IList<int>>()), (r) => Console.WriteLine(r.ToJSON()));
        }
    }
}
