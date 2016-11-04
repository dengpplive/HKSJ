using System;
using System.Collections;
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
using PlateView = HKSJ.WBVV.Entity.ViewModel.Client.PlateView;

namespace HKSJ.WBVV.Api
{
    public class PlateController : ApiControllerBase
    {
        private readonly IPlateBusiness _plateBusiness;
        public PlateController(IPlateBusiness plateBusiness)
        {
            this._plateBusiness = plateBusiness;

        }

        /// <summary>
        /// 删除板块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result DeletePlate()
        {
            return CommonResult(() => this._plateBusiness.DeletePlate(JObject.Value<int>("id")), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 删除多个板块
        /// </summary>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result DeletePlates()
        {
            var ids = JObject.Value<JArray>("ids");
            return CommonResult(() => this._plateBusiness.DeletePlates(ids== null ? null : ids.ToObject<IList<int>>()), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 添加板块
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result CreatePlate()
        {
            return CommonResult(() => this._plateBusiness.CreatePlate(
                JObject.Value<int>("categoryId"),
                JObject.Value<string>("name"),
                JObject.Value<int>("sortNum"),
                JObject.Value<int>("pageSize")
                ), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 修改板块
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result UpdatePlate()
        {
            return CommonResult(() => this._plateBusiness.UpdatePlate(
               JObject.Value<int>("id"),
               JObject.Value<int>("categoryId"),
               JObject.Value<string>("name"),
               JObject.Value<int>("sortNum"),
               JObject.Value<int>("pageSize")
               ), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 板块排序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result UpdatePlateSort()
        {
            Plate plate;
            IList<Plate> plateList = new List<Plate>();
            var data = JObject.Value<JArray>("data");
            for (int i = 0; i < data.Count; i++)
            {
                plate = new Plate();
                plate.Id = Convert.ToInt32(data[i]["Id"].ToString());
                plate.SortNum = Convert.ToInt32(data[i]["SortNum"].ToString());
                plateList.Add(plate);
            }
            return CommonResult(() => this._plateBusiness.UpdatePlateSort(plateList), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 单个板块信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Plate GetPlate(int id)
        {
            return this._plateBusiness.GetPlate(id);
        }

        /// <summary>
        /// 获取板块分页集合
        /// </summary>
        [HttpPost]
        public PageResult GetPlatePageResult()
        {
            this._plateBusiness.PageIndex = this.PageIndex;
            this._plateBusiness.PageSize = this.PageSize;
            return this._plateBusiness.GetPlatePageResult(this.Condtions, this.OrderCondtions);
        }

        /// <summary>
        /// 获取首页显示板块信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IList<PlateView> GetPlateViewByHomeList()
        {
            return this._plateBusiness.GetPlateViewByHomeList();
        }

        /// <summary>
        /// 获取指定分类下的板块信息
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<PlateView> GetPlateViewByCategoryIdList(int cid)
        {
            return this._plateBusiness.GetPlateViewByCategoryIdList(cid);
        }
    }
}
