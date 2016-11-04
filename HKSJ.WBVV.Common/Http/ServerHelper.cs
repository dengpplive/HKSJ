using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace HKSJ.WBVV.Common.Http
{
    /// <summary>
    /// ip地址帮助类
    /// </summary>
    public static class ServerHelper
    {
        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP
        {
            get
            {
                var hostName = Dns.GetHostName();
                IPHostEntry localhost = Dns.GetHostEntry(hostName);
                IPAddress localAddress = localhost.AddressList.FirstOrDefault(add => add.AddressFamily == AddressFamily.InterNetwork);
                return localAddress != null ? localAddress.ToString() : string.Empty;
            }

        }
        #region 获取网站根目录Url
        /// <summary>
        /// 获取网站根目录Url
        /// </summary>
        public static string RootPath
        {
            get
            {
                string appPath;
                HttpContext httpCurrent = HttpContext.Current;
                HttpRequest request = httpCurrent.Request;
                string urlAuthority = "//" + request.Url.Authority;
                if (request.ApplicationPath == null || request.ApplicationPath == "/")
                {
                    appPath = urlAuthority;
                }
                else //安装在虚拟子目录下
                {
                    appPath = urlAuthority + request.ApplicationPath;
                }
                return appPath;
            }
        }
        #endregion


    }

}
