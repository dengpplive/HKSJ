using HKSJ.WBVV.Common.Logger;
using System.Web.Mvc;

namespace HKSJ.WBVV.MVC.Manage.Common.Base
{
    [Authorize]
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.ExceptionHandled = true;
                string errorMsg = filterContext.Exception.Message;
                //filterContext.Result = Json(new { ErrorCode = filterContext.Exception.Source, ErrorMsg = errorMsg }, filterContext.Exception.Message);
                filterContext.Result = Json(new { ErrorCode = filterContext.Exception.Source, ErrorMsg = errorMsg }, filterContext.Exception.ToString());
            }
            else
            {
                filterContext.ExceptionHandled = true;
                //var errorView = View("Error", (object)filterContext.Exception.Message);
                var errorView = View("Error", (object)filterContext.Exception.ToString());
                filterContext.Result = errorView;
            }
#if !DEBUG
                    LogBuilder.Log4Net.Error(filterContext.Exception.Message, filterContext.Exception);
#else

#endif

            base.OnException(filterContext);
        }
    }
}
