using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.MVC.Common;
using System.Web.Mvc;
using System.Web.Security;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.MVC.Manage.Common;

namespace HKSJ.WBVV.MVC.Manage.Controllers
{
    public class LoginController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public void LoginIn(FormCollection form)
        {
            var loginName = form["loginName"];
            var password = form["password"];
            var returnUrl = form["returnUrl"];
            var data = new
            {
                loginName,
                password
            };
            var result = WebApiHelper.InvokeApi<Result>("Manage/ManageLogin", data);
            if (result.Success)
            {
                var manage = result.Data;
                TempData["errorMessage"] = "";
                TempData["error"] = "";
                //写session
                HttpContext.Session[WebContext.SessionKey] = manage;

                FormsAuthentication.RedirectFromLoginPage(loginName, false);
                var urlReferrer = (string.IsNullOrEmpty(returnUrl) || Request.UrlReferrer == null) ? WebConfig.DefaultUrl : Request.UrlReferrer.AbsoluteUri;
                Response.Redirect(urlReferrer);
            }
            else
            {
                TempData["errorMessage"] = result.ExceptionMessage;
                TempData["loginName"] = loginName;
                TempData["error"] = "display:block";
                var urlReferrer = Request.UrlReferrer == null ? WebConfig.DefaultUrl : Request.UrlReferrer.AbsoluteUri;
                Response.Redirect(urlReferrer);
            }
        }

        public void LoginOut()
        {
            Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            FormsAuthentication.SignOut();

            Session[WebContext.SessionKey] = null;
            var urlReferrer = Request.UrlReferrer == null ? WebConfig.DefaultUrl : Request.UrlReferrer.AbsoluteUri;
            Response.Redirect(urlReferrer);
        }
    }
}
