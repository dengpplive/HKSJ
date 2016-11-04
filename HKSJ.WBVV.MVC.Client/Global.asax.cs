using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading;
using HKSJ.WBVV.Common.Logger;

namespace HKSJ.WBVV.MVC.Client
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HKSJ.WBVV.MVC.Client.Config.ClientIocConfig.Register(GlobalConfiguration.Configuration);
            HKSJ.WBVV.MVC.Common.Language.CacheAllTexts();
            HKSJ.WBVV.MVC.Common.DefaultData.InitData();
            LogBuilder.InitLog4Net(HttpContext.Current.Server.MapPath("~/bin/log4net.config"), "WBVVClient");
        }
    }
}