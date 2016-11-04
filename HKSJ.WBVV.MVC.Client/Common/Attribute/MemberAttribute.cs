using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HKSJ.WBVV.Common.Language;
using HKSJ.WBVV.MVC.Client.GlobalVariable;

namespace HKSJ.WBVV.MVC.Client.Attribute
{
    /// <summary>
    /// 检测登录过滤器
    /// </summary>
    public class MemberAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (GlobalMemberInfo.UserId >= 1) return;
            var request = HttpContext.Current.Request;
            var urlsb = new StringBuilder();
            var rawUrlArr = request.RawUrl.Split('/');
            urlsb = rawUrlArr.Length > 3 ? urlsb.Append("/" + rawUrlArr[rawUrlArr.Length - 2] + "/" + rawUrlArr[rawUrlArr.Length - 1]) : urlsb.Append(HttpContext.Current.Request.RawUrl);
            var isAjaxRequst = filterContext.HttpContext.Request.IsAjaxRequest();
            if (isAjaxRequst)
            {
                //ajax的请求,返回json格式
                var json = new JsonResult
                {
                    Data = new {Success = true, Data = false, Message = LanguageUtil.Translate("web_Common_Attribute_Meber_OnActionExecuting_Message")},
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                filterContext.Result = json;
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Home", action = "Index", returnurl = urlsb.ToString() }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}