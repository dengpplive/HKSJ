using System.Configuration;
using System.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Api.Host.Config;
using HKSJ.WBVV.Api.Host.Route;
using HKSJ.WBVV.Api.Host.Service;
using HKSJ.WBVV.Common.Config;
using Microsoft.Owin.Hosting;
using QiniuConfig = Qiniu.Conf;
using HKSJ.WBVV.Common.Logger;

namespace HKSJ.WBVV.Api.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            if (ApiConfig.IsDev)
            {
                using (WebApp.Start<Startup>(url: ApiConfig.WebApiHost))
                {
                    WebApiService webApiServer = new WebApiService();
                    webApiServer.CheckIndex();
                    webApiServer.CacheLanguageAllTexts();

                    Console.WriteLine("服务已经启动......");
                    Console.ReadLine();

                }
            }
            else
            {
                var servicesToRun = new ServiceBase[] 
                { 
                    new WebApiService() 
                };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
