using System;
using System.Collections.Generic;
using System.Linq;
using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.MVC.Common;
using System.Web.Mvc;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Manage.Common.Base;

namespace HKSJ.WBVV.MVC.Manage.Controllers
{
    public class PlateController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 版块管理
        /// </summary>
        /// <returns></returns>
        public ActionResult PlateManage()
        {
            var api = WebConfig.BaseAddress;
            var model = new { api };
            return View(model);
        }

        public ActionResult PlateAdd()
        {
            var api = WebConfig.BaseAddress;
            var model = new { api };
            return View(model);
        }

        public ActionResult PlateEdit()
        {
            var api = WebConfig.BaseAddress;
            var model = new { api };
            return View(model);
        }

        #region 板块管理 Author : Axone
        [HttpPost]
        public JsonResult CreatePlate(int categoryId, string name, int sortNum, int pageSize)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<int?>>("Plate/CreatePlate", new { categoryId, name, sortNum, pageSize });
            if (result == null || result.Success == false || result.Data == 0)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true, result.Data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdatePlate(int id, int categoryId, string name, int sortNum, int pageSize)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("Plate/UpdatePlate", new { id, categoryId, name, sortNum, pageSize });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePlate(int id)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("Plate/DeletePlate", new { id });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePlates(List<int> ids)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("Plate/DeletePlates", new { ids });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion




    }
}
