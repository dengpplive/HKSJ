using System.Web;
using Autofac;
using Autofac.Configuration;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using HKSJ.WBVV.Api.Provider;
using HKSJ.WBVV.Common;

namespace HKSJ.WBVV.Api.Host.Config
{
    public class ApiIocConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.Load("HKSJ.WBVV.Api"));//注册api容器的实现

            //注册接口         
            builder.RegisterModule(new ConfigurationSettingsReader("autofac"));
            
            //  AxOne begin
            //注册 Password Grant 授权服务
            //builder.RegisterType<PasswordAuthorizationServerProvider>().AsSelf().SingleInstance();
            //builder.RegisterType<RefreshAuthenticationTokenProvider>().AsSelf().SingleInstance();
            //  AxOne end

            var container = builder.Build();
            HttpRuntime.Cache["containerKey"] = container;
            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;
            StaticObj.Container = container;
        }
    }
}
