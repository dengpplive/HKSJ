using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using HKSJ.WBVV.Api.Host.Config;
using HKSJ.WBVV.Api.Host.Route;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.Common.Extender;
using Microsoft.Owin.Hosting;
using HKSJ.WBVV.Common.Logger;

namespace HKSJ.WBVV.Api.Host.Service
{
    partial class WebApiService : ServiceBase
    {
        IDisposable _webapi;
        IDisposable _webapiJob;
        private Thread th;
        public WebApiService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _webapi = WebApp.Start<Startup>(url: ApiConfig.WebApiHost);
            if (!string.IsNullOrEmpty(ApiConfig.ServicePort))
            {
                _webapiJob = WebApp.Start<Startup>(url: ApiConfig.ServicePort);
            }
            CheckIndex();
            CacheLanguageAllTexts();
        }

        public void CheckIndex()
        {
            //检测索引
            try
            {
                LogBuilder.Log4Net.Info("索引进程1开启");

                th = new Thread(() =>
                {
                    var _videoBusiness = ((Autofac.IContainer)HttpRuntime.Cache["containerKey"]).Resolve<IVideoBusiness>();
                    _videoBusiness.CheckIndexFile();
                });
                th.IsBackground = true;


                th.Start();
                LogBuilder.Log4Net.Info("索引进程1开启成功");

            }
            catch (Exception ex)
            {
#if !DEBUG
                LogBuilder.Log4Net.Error("索引进程1开启失败", ex.MostInnerException());
#else
                Console.WriteLine("索引进程1开启失败" + ex.MostInnerException().Message);
#endif
            }

        }

        public void CacheLanguageAllTexts()
        {
            try
            {
                th = new Thread(() =>
                {
                    //缓存语言文本
                    var language = new Language();
                    language.CacheAllTexts();
                });

                th.IsBackground = true;
                th.Start();
                LogBuilder.Log4Net.Info("语言文本进程开启成功");
            }
            catch (Exception ex)
            {
#if !DEBUG
                LogBuilder.Log4Net.Error("语言文本进程开启失败", ex.MostInnerException());
#else
                Console.WriteLine("语言文本进程开启失败" + ex.MostInnerException().Message);
#endif
            }
        }

        protected override void OnStop()
        {
            if (_webapi != null)
            {
                _webapi.Dispose();
            }
            if (_webapiJob != null)
            {
                _webapiJob.Dispose();
            }
            if (th != null)
            {
                th.Abort();
            }
        }
    }
}
