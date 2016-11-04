
using HKSJ.WBVV.Common.Http;
using System;
using System.Collections;
using System.Configuration;
using System.Linq;

namespace HKSJ.WBVV.Common.Config
{
    public static class ApiConfig
    {
        /// <summary>
        /// webapi启动服务地址
        /// </summary>
        public static string WebApiHost = ConfigurationManager.AppSettings["WebApiHost"];
        /// <summary>
        /// 服务监听地址
        /// </summary>
        /// <returns></returns>
        public static string ServicePort
        {
            get
            {
                if (!ConfigurationManager.AppSettings.AllKeys.Contains("ServicePort"))
                {
                    return string.Empty;
                }
                string ip = ServerHelper.GetLocalIP;
                string strport = ConfigurationManager.AppSettings["ServicePort"].Trim();
                int port;
                bool res = int.TryParse(strport, out port);
                if (!res)
                {
                    throw new ArgumentException("服务层监听端口配置错误");
                }
                return string.Format("http://{0}:{1}/", ip, port);
            }
        }
        /// <summary>
        /// 是否是开发环境
        /// </summary>
        public static bool IsDev = Convert.ToBoolean(ConfigurationManager.AppSettings["IsDev"]);

        /// <summary>
        /// 短信服务地址
        /// </summary>
        public static string SMSBaseUri
        {
            get
            {
                return ConfigurationManager.AppSettings["SMSBaseUri"];
            }
        }

        public static ArrayList ApiAssemblies
        {
            get
            {
                var assemblies = new ArrayList();
                string assemblie = ConfigurationManager.AppSettings["ApiAssemblies"];
                if (string.IsNullOrEmpty(assemblie))
                {
                    assemblies.Add("HKSJ.WBVV.Api.dll");
                    return assemblies;
                }
                assemblies.AddRange(assemblie.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                return assemblies;
            }
        }
    }
}
