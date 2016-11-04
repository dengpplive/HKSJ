using System.Threading;
using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.MVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Newtonsoft.Json.Linq;
using HKSJ.Utilities;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Common.Http;
using HKSJ.WBVV.MVC.Client.GlobalVariable;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Client.Models;
using Newtonsoft.Json;
using HKSJ.WBVV.Common.Extender.LinqExtender;

namespace HKSJ.WBVV.MVC.Client.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.curr = GetCurrCaterogyId();
            return View();
        }

        /// <summary>
        /// 用户注册协议
        /// Author : AxOne
        /// </summary>
        /// <returns></returns>
        public ActionResult Agreement()
        {
            var lang = WebConfig.ActiveLanguage;
            if (lang == "en-US") return View("Agreement_en_us");
            return View();
        }

        /// <summary>
        /// 头部
        /// Author : AxOne
        /// </summary>
        /// <returns></returns>
        public PartialViewResult _Header()
        {
            var model = GetUserModel();//TODO update 刘强2015-11-3 13：10
            ViewBag.UID = GlobalMemberInfo.UserId;
            return PartialView(model);
        }

        public PartialViewResult _Footer()
        {
            return PartialView();
        }


        /*--------------------------------------------------*/

        private int GetCurrCaterogyId()
        {
            int curCaterogyId = -1;
            if (Request.QueryString["curId"] != null
                && Request.QueryString["curId"] != ""
                )
            {
                if (!int.TryParse(Request.QueryString["curId"].ToString(), out curCaterogyId)) curCaterogyId = -1;
            }
            return curCaterogyId;
        }
        /// <summary>
        /// 获取顶部的一级分类视图
        /// </summary>
        /// <returns></returns>
        public PartialViewResult FirstCategory()
        {
            ViewBag.rootPath = ServerHelper.RootPath;
            var currCaterogyId = GetCurrCaterogyId();
            ViewBag.curr = currCaterogyId;
            //板块信息
            var url = WebConfig.BaseAddress + "Plate/GetPlateViewByCategoryIdList?cid=" + ViewBag.curr;
            if (currCaterogyId <= 0)
                url = WebConfig.BaseAddress + "Plate/GetPlateViewByHomeList";
            string strData = WebApiHelper.InvokeApi<string>(url);
            var plateViewList = (!string.IsNullOrEmpty(strData) && !strData.StartsWith("null")) ? strData.JonsToList<PlateView>() : new List<PlateView>();
            ViewBag.plateData = plateViewList.Count == 0 ? new List<PlateView>() : plateViewList;

            //菜单分类信息
            url = WebConfig.BaseAddress + "Category/GetMenuViewList";
            strData = WebApiHelper.InvokeApi<string>(url);
            var model = (!string.IsNullOrEmpty(strData) && !strData.StartsWith("null")) ? strData.JonsToList<MenuView>() : new List<MenuView>();

            //可显示的分类信息
            url = WebConfig.BaseAddress + "Category/GetMenuViewListVisible";
            strData = WebApiHelper.InvokeApi<string>(url);
            var categoryData = (!string.IsNullOrEmpty(strData) && !strData.StartsWith("null")) ? strData.JonsToList<MenuView>() : new List<MenuView>();
            ViewBag.categoryData = categoryData;

            if (currCaterogyId > 0)
            {
                var childCategorys = model.Where(p => p.ParentCategory.Id == ViewBag.curr).FirstOrDefault();
                if (childCategorys != null)
                    ViewBag.categoryData = childCategorys.ChildCategorys.Count > 0 ? childCategorys.ChildCategorys : new List<MenuView>();
                else
                    ViewBag.categoryData = new List<MenuView>();
            }
            return PartialView(model);
        }
        /// <summary>
        /// Banner
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PartialViewResult Banner(int pageSize = 8)
        {
            var curr = GetCurrCaterogyId();
            var url = WebConfig.BaseAddress + "Video/GetBannerVideoList?categoryId=" + curr + "&pagesize=" + pageSize;
            var ordercondtions = new List<OrderCondtion>(){
                new OrderCondtion(){
                     FiledName="SortNum",
                     IsDesc=true
                }
            };
            var strJson = WebApiHelper.InvokeApi<string>(url, new { ordercondtions });
            var model = (!string.IsNullOrEmpty(strJson) && !strJson.StartsWith("null")) ? strJson.JonsToList<VideoView>() : new List<VideoView>();
            ViewBag.Visible = true;
            ViewBag.rootPath = ServerHelper.RootPath;
            if (model.Count == 0 || curr <= 0) ViewBag.Visible = false;
            return PartialView(model);
        }

        /// <summary>
        /// 区域
        /// </summary>
        /// <returns></returns>
        public PartialViewResult AreaCaterogy()
        {
            var curr = GetCurrCaterogyId();
            var url = WebConfig.BaseAddress + "Dictionary/GetDictionaryViewList?categoryId=" + curr;
            string strData = WebApiHelper.InvokeApi<string>(url);
            var model = (!string.IsNullOrEmpty(strData) && !strData.StartsWith("null")) ? strData.JonsToList<DictionaryView>() : new List<DictionaryView>();

            ViewBag.Visible = true;
            ViewBag.rootPath = ServerHelper.RootPath;
            ViewBag.curr = curr;
            if (curr <= 0) ViewBag.Visible = false;
            return PartialView(model);
        }
        /// <summary>
        /// 加载板块
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadPlateVideo(PlateView plateView)
        {
            ViewBag.curr = plateView.CategoryId;
            ViewBag.rootPath = ServerHelper.RootPath;
            ViewBag.plateView = plateView;
            var url = WebConfig.BaseAddress + "Video/GetRecommendAndHotPlateVideo?plateId=" + plateView.Id;
            string strJson = WebApiHelper.InvokeApi<string>(url);
            var model = (!string.IsNullOrEmpty(strJson) && !strJson.StartsWith("null")) ? strJson.JsonToEntity<RecommendAndHotPlateVideoView>() : new RecommendAndHotPlateVideoView();
            return Json(new { model = model, curr = ViewBag.curr, plateView = plateView }, JsonRequestBehavior.AllowGet);
        }
        //加载分类和视频
        public JsonResult LoadCategoryVideo(Menu category)
        {
            int curId = category.CurId;
            int pageSize = category.PageSize;
            int categoryId = category.Id;
            var url = WebConfig.BaseAddress + "Video/GetCategoryVideoData?categoryId=" + categoryId + "&pagesize=" + pageSize;
            if (curId < 0)//首页
                url += "&isIndexPage=true";
            string strJson = WebApiHelper.InvokeApi<string>(url);
            var model = (!string.IsNullOrEmpty(strJson) && !strJson.StartsWith("null")) ? strJson.JsonToEntity<RecommendAndHotCategoryVideoView>() : new RecommendAndHotCategoryVideoView();
            return Json(new { model = model, cId = curId + "_" + categoryId, category = category }, JsonRequestBehavior.AllowGet);
        }
    }
}