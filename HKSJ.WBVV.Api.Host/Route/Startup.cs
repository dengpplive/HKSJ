using HKSJ.WBVV.Api.Host.Config;
using HKSJ.WBVV.Api.Host.ConsoleLog;
using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.Common.Logger;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Web.Http;

namespace HKSJ.WBVV.Api.Host.Route
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            ApiAssembie.LoadApiAssembie(ApiConfig.ApiAssemblies);
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            //ioc控制反转
            ApiIocConfig.Register(config);
            //启动日志
            LogBuilder.InitLog4Net("WBVV");
            //注册异常过滤器
            config.Filters.Add(new WebApiExceptionFilter());
            //初始化七牛数据
            QiniuData.InitQiniuData();
            //ApiExplorerConfig
            ApiExplorerConfig.Register(config);
            config.Formatters.Clear();
            config.Formatters.Insert(0, new JsonpMediaTypeFormatter());
            var serializerSettings = config.Formatters.JsonFormatter.SerializerSettings;
            var contractResolver = (DefaultContractResolver)serializerSettings.ContractResolver;
            contractResolver.IgnoreSerializableAttribute = true;
            config.MessageHandlers.Add(new MessageDispatcher());
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional });
            appBuilder.UseWebApi(config);
        }
    }
}
