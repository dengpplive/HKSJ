using System.IO;
using System.Web;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using HKSJ.WBVV.Api.DocumentController;

namespace HKSJ.WBVV.Api.Host.Config
{
    public class ApiExplorerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "HKSJ.WBVV.Api.XML");
            config.Services.Replace(typeof(IDocumentationProvider), new XmlDocumentationProvider(path));
            HttpRuntime.Cache["ApiExploer"] = config.Services.GetApiExplorer();
        }
    }
}
