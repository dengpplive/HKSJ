using System.Collections.Generic;
using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.MVC.Common;
using System.Web.Mvc;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Manage.Common.Base;

namespace HKSJ.WBVV.MVC.Manage.Controllers
{
    public class BannerController : BaseController
    {
        //
        // GET: /Banner/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 板览图管理
        /// </summary>
        /// <returns></returns>
        public ActionResult BannerVideoManage()
        {
            var api = WebConfig.BaseAddress;
            var model = new { api };
            return View(model);
        }

        public ActionResult BannerVideoAdd()
        {
            var api = WebConfig.BaseAddress;
            var model = new { api };
            return View(model);
        }

        public ActionResult BannerVideoEdit()
        {
            var api = WebConfig.BaseAddress;
            var model = new { api };
            return View(model);
        }

        #region Banner管理 Author : Axone
        [HttpPost]
        public JsonResult CreateBannerVideo(int cid, int sNum, int vid, string simgPath, string imgPath)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<int?>>("BannerVideo/CreateBannerVideo", new { CategoryId = cid, SortNum = sNum, VideoId = vid, BannerSmallImagePath = simgPath, BannerImagePath = imgPath });
            if (result == null || result.Success == false || result.Data < 1)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateBannerVideo(int id, int cid, int vid, int sNum, string simgPath, string imgPath)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("BannerVideo/UpdateBannerVideo", new { Id = id, CategoryId = cid, VideoId = vid, SortNum = sNum, BannerSmallImagePath = simgPath, BannerImagePath = imgPath });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteBannerVideo(int id)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("BannerVideo/DeleteBannerVideo", new { id });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteBannerVideos(List<int> ids)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("BannerVideo/DeleteBannerVideos", new { ids });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
