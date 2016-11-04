using System;
using System.Web.Mvc;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Common;
using HKSJ.WBVV.MVC.Manage.Common.Base;
using HKSJ.WBVV.Entity.ViewModel.Manage;

namespace HKSJ.WBVV.MVC.Manage.Controllers
{
    public class AuditVideoController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var date = DateTime.Now;
            var today = date.ToString("yyyy-MM-dd");
            var thisWeekBegin = date.AddDays(1 - Convert.ToInt32(date.DayOfWeek.ToString("d")));//本周一
            var thisWeekBeginning = thisWeekBegin.ToString("yyyy-MM-dd");
            var lastWeekBeginning = thisWeekBegin.AddDays(-7).ToString("yyyy-MM-dd");
            var lastWeekEnding = thisWeekBegin.AddDays(-1).ToString("yyyy-MM-dd");
            var thisMonthBegin = date.AddDays(1 - date.Day);//本月初
            var thisMonthBeginning = thisMonthBegin.ToString("yyyy-MM-dd");
            var lastMonthBgeinning = thisMonthBegin.AddMonths(-1).ToString("yyyy-MM-dd");
            var lastMonthEnding = thisMonthBegin.AddDays(-1).ToString("yyyy-MM-dd");
            var thisYearBegin = date.AddDays(1 - date.DayOfYear);//本年初
            var thisYearBeginning = thisYearBegin.ToString("yyyy-MM-dd");
            ViewBag.dataformat = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", thisWeekBeginning, today, lastWeekBeginning, lastWeekEnding, thisMonthBeginning, today, lastMonthBgeinning, lastMonthEnding, thisYearBeginning, today);
            return View();
        }

        [HttpGet]
        public ActionResult Approve(int vid, int videoState,string approveContent)
        {
            ViewBag.vid = vid;
            ViewBag.videoState = videoState;
            ViewBag.approveContent = approveContent;
            return View();
        }

        #region 视频管理 Author : Axone

        [HttpPost]
        public JsonResult DeleteAVideo(int vid)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("Video/DeleteAVideo", new { vid });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateAVideo(MyVideoPara dataModel)
        {
            var result = WebApiHelper.InvokeAxApi<Result>("Video/UpdateAVideo", new { dataModel = dataModel.ToJSON() });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddApproveInfo(VideoApproveView dataModel)
        {
            var result = WebApiHelper.InvokeAxApi<Result>("VideoApprove/AddApproveInfo", new { dataModel = dataModel.ToJSON() });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}
