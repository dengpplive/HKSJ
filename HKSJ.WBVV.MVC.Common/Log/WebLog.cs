using log4net;
using System;
using System.Web;

namespace HKSJ.WBVV.MVC.Common.Log
{
    public class WebLog
    {
        static ILog log;
        static WebLog()
        {
            log = LogManager.GetLogger("WebLog");
        }
        internal static void Error(Exception ex)
        {
            Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(ex));
        }

        internal static void Info(string p)
        {
            if (log != null)
            {
                log.Info(p);
            }
        }
    }
}