using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HKSJ.WBVV.Common.Logger;

namespace HKSJ.WBVV.MVC.Manage
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HKSJ.WBVV.MVC.Manage.Config.ManageIocConfig.Register(GlobalConfiguration.Configuration);
            HKSJ.WBVV.MVC.Common.Language.CacheAllTexts();
            HKSJ.WBVV.MVC.Common.DefaultData.InitData();
            LogBuilder.InitLog4Net(HttpContext.Current.Server.MapPath("~/bin/log4net.config"), "WBVVManage");
        }
    }
}
