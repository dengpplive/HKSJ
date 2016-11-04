using System.Web;
using Autofac;
using Autofac.Configuration;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using HKSJ.WBVV.Common;

namespace HKSJ.WBVV.MVC.Manage.Config
{
    public class ManageIocConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            //注册接口
            builder.RegisterModule(new ConfigurationSettingsReader("autofac"));

            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;
            StaticObj.Container = container;
        }
    }
}
