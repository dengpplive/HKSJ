using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Common;
using HKSJ.WBVV.MVC.Manage.Common.Base;
using HKSJ.WBVV.MVC.Manage.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UserView = HKSJ.WBVV.Entity.ViewModel.Manage.UserView;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.MVC.Manage.Controllers
{
    /// <summary>
    /// 用户管理 
    /// Author : AxOne
    /// </summary>
    public class UserController : BaseController
    {
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

        /// <summary>
        /// 查询用户
        /// Author : AxOne
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchUser(ParaModel data)
        {
            var result = WebApiHelper.InvokeAxApi<PageResult<IList<UserView>>>("User/SearchUser", data);
            return result == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(new { result.PageSize, result.PageIndex, result.TotalIndex, result.TotalCount, result.Data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 禁用(启用)用户
        /// Author : AxOne
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ForbiddenUser(int uid, bool state)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("User/ForbiddenUser", new { uid, state });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Data = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Data = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 处理JSON中的时间格式
        /// </summary>       
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new NewtonJsonResult
            {
                ContentEncoding = contentEncoding,
                ContentType = contentType,
                Data = data,
                JsonRequestBehavior = behavior
            };
        }

    }

    public class NewtonJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if ((JsonRequestBehavior == JsonRequestBehavior.DenyGet) && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(LanguageUtil.Translate("admin_Controllers_User_NewtonJsonResult_ExecuteResult_InvalidOperationException"));
            }
            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "text/html";
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data == null) return;
            var timeFormat = new IsoDateTimeConverter {DateTimeFormat = "yyyy-MM-dd HH:mm:ss"};
            //设置输出的时间格式
            var strData = JsonConvert.SerializeObject(Data, Formatting.Indented, timeFormat);
            response.Write(strData);
        }
    }


}
