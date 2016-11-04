using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using Newtonsoft.Json.Linq;

namespace HKSJ.WBVV.Api
{
    public class PlateVideoController : ApiControllerBase
    {
        private readonly IPlateVideoBusiness _plateVideoBusiness;
        public PlateVideoController(IPlateVideoBusiness plateVideoBusiness)
        {
            this._plateVideoBusiness = plateVideoBusiness;

        }

        /// <summary>
        /// 板块分页显示
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PageResult GetPlateVideoPageResult()
        {
            this._plateVideoBusiness.PageIndex = this.PageIndex;
            this._plateVideoBusiness.PageSize = this.PageSize;
            return this._plateVideoBusiness.GetPlateVideoPageResult(this.Condtions, this.OrderCondtions);
        }

        /// <summary>
        /// 单个板块信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public PlateVideo GetPlateVideo(int id)
        {
            return this._plateVideoBusiness.GetPlateVideo(id);
        }

        /// <summary>
        /// 添加板块视频
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result CreatePlateVideo()
        {
            this._plateVideoBusiness.UserId = JObject.Value<int>("CreateManageId");
            if (JObject.Value<int>("plateId")>0)
            {
                return CommonResult(() => this._plateVideoBusiness.CreatePlateVideo(
                    JObject.Value<int>("plateId"),
                    JObject.Value<int>("sortNum"),
                    JObject.Value<int>("isHot"),
                    JObject.Value<int>("isRecommend"),
                    JObject.Value<int>("videoId")
                    ), (r) => Console.WriteLine(r.ToJSON()));
            }
            else
            {
                return CommonResult(() => this._plateVideoBusiness.CreatePlateCategoryVideo(
                    JObject.Value<int>("categoryId"),
                    JObject.Value<int>("sortNum"),
                    JObject.Value<int>("isHot"),
                    JObject.Value<int>("isRecommend"),
                    JObject.Value<int>("videoId")
                    ), (r) => Console.WriteLine(r.ToJSON()));
            }
        }
        /// <summary>
        /// 添加多个板块视频
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result CreatePlateVideos()
        {
            //this._plateVideoBusiness.UserId = JObject.Value<int>("CreateManageId");
            //var ids = JObject.Value<JArray>("videoIds");
            //return CommonResult(() => this._plateVideoBusiness.CreatePlateVideos(
            //    JObject.Value<int>("plateId"),
            //    ids == null ? null : ids.ToObject<IList<int>>()
            //    ), (r) => Console.WriteLine(r.ToJSON()));
            return null;
        }

        /// <summary>
        /// 修改单个板块视频
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result UpdatePlateVideo()
        {
            if (JObject.Value<int>("plateId") > 0)
            {
                return CommonResult(() => this._plateVideoBusiness.UpdatePlateVideo(
                        JObject.Value<int>("id"),
                        JObject.Value<int>("plateId"),
                        JObject.Value<int>("sortNum"),
                        JObject.Value<int>("isHot"),
                    JObject.Value<int>("isRecommend"),
                        JObject.Value<int>("videoId")
                    ), (r) => Console.WriteLine(r.ToJSON()));
            }
            else
            {
                return CommonResult(() => this._plateVideoBusiness.UpdatePlateVideo(
                    JObject.Value<int>("id"),
                    JObject.Value<int>("sortNum"),
                    JObject.Value<int>("isHot"),
                    JObject.Value<int>("isRecommend"),
                    JObject.Value<int>("videoId")
                    ), (r) => Console.WriteLine(r.ToJSON()));
            }
        }

        /// <summary>
        /// 修改多个板块视频
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result UpdatePlateVideos()
        {
            //var ids = JObject.Value<JArray>("videoIds");
            //return CommonResult(() => this._plateVideoBusiness.UpdatePlateVideos(
            //        JObject.Value<int>("id"),
            //        JObject.Value<int>("plateId"),
            //        ids == null ? null : ids.ToObject<IList<int>>()
            //    ), (r) => Console.WriteLine(r.ToJSON()));
            return null;
        }

        /// <summary>
        /// 板块视频排序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result UpdatePlateVideoSort()
        {
            PlateVideo plateVideo;
            IList<PlateVideo> plateList = new List<PlateVideo>();
            var data = JObject.Value<JArray>("data");
            for (int i = 0; i < data.Count; i++)
            {
                plateVideo = new PlateVideo();
                plateVideo.Id = Convert.ToInt32(data[i]["Id"].ToString());
                plateVideo.SortNum = Convert.ToInt32(data[i]["SortNum"].ToString());
                plateList.Add(plateVideo);
            }
            return CommonResult(() => this._plateVideoBusiness.UpdatePlateVideoSort(plateList), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 删除板块视频
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result DeletePlateVideo()
        {
            return CommonResult(() => this._plateVideoBusiness.DeletePlateVideo(JObject.Value<int>("id")), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 删除多个板块视频
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result DeletePlateVideos()
        {
            var ids = JObject.Value<JArray>("ids");
            return CommonResult(() => this._plateVideoBusiness.DeletePlateVideos(ids == null ? null : ids.ToObject<IList<int>>()), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 删除指定板块下的视频
        /// </summary>
        /// <param name="plateId"></param>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result DeletePlateVideos(int plateId)
        {
            return CommonResult(() => this._plateVideoBusiness.DeletePlateVideos(plateId), (r) => Console.WriteLine(r.ToJSON()));
        }
    }
}
