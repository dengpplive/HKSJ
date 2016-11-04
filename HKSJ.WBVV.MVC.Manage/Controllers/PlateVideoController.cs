using System.Collections.Generic;
using System.Web.Mvc;
using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Common;
using HKSJ.WBVV.MVC.Manage.Common.Base;

namespace HKSJ.WBVV.MVC.Manage.Controllers
{
    public class PlateVideoController : BaseController
    {
        //
        // GET: /PlateVideo/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///     版块视频管理
        /// </summary>
        /// <returns></returns>
        public ActionResult PlateVideoManage()
        {
            var api = WebConfig.BaseAddress;
            var model = new { api };
            return View(model);
        }

        public ActionResult PlateVideoAdd()
        {
            var api = WebConfig.BaseAddress;
            var model = new { api };
            return View(model);
        }

        public ActionResult PlateVideoEdit()
        {
            var api = WebConfig.BaseAddress;
            var model = new { api };
            return View(model);
        }

        public ActionResult VideoList()
        {
            var api = WebConfig.BaseAddress;
            var model = new { api };
            return View(model);
        }

        #region 板块视频管理 Author : Axone

        [HttpPost]
        public JsonResult CreatePlateVideo(int cid, int pid, int vid, int caid, int sNum, int isHot, int isRcd)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<int?>>("PlateVideo/CreatePlateVideo", new { CreateManageId = cid, plateId = pid, sortNum = sNum, isHot, isRecommend = isRcd, videoId = vid, categoryId = caid });
            if (result == null || result.Success == false || result.Data < 1)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdatePlateVideo(int id, int pid, int vid, int sNum, int isHot, int isRcd)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("PlateVideo/UpdatePlateVideo", new { id, plateId = pid, sortNum = sNum, isHot, isRecommend = isRcd, videoId = vid });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePlateVideo(int id)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("PlateVideo/DeletePlateVideo", new { id});
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePlateVideos(List<int> ids)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("PlateVideo/DeletePlateVideos", new { ids });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}