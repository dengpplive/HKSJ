using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.Common.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;

namespace HKSJ.WBVV.MVC.Common.Controllers
{
    public class GlobalJsConstantController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Index()
        {
            StringBuilder resultBuilder = new StringBuilder();
            resultBuilder.AppendFormat("api = '{0}';", WebConfig.BaseAddress);
            resultBuilder.AppendFormat("rootPath='{0}';", ServerHelper.RootPath);
            resultBuilder.AppendFormat("ipAddress='{0}';", ClientHelper.GetIPAddress);
            resultBuilder.AppendFormat("imageAddress='{0}';", DefaultData.ImageAddress);
            resultBuilder.AppendLine("");
            resultBuilder.AppendLine(Language.GlobalJSAllTexts());
            string result = resultBuilder.ToString();
            return new HttpResponseMessage { Content = new StringContent(result, Encoding.GetEncoding("UTF-8"), "application/x-javascript") };
        }
    }
}