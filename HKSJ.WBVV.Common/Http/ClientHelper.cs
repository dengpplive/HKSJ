using HKSJ.WBVV.Common.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace HKSJ.WBVV.Common.Http
{
    public static class ClientHelper
    {

        #region 获取浏览器版本号

        /// <summary>  
        /// 获取浏览器版本号  
        /// </summary>  
        /// <returns></returns>  
        public static string GetBrowser()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.Browser + bc.Version;
        }

        #endregion

        #region 获取操作系统版本号

        /// <summary>  
        /// 获取操作系统版本号  
        /// </summary>  
        /// <returns></returns>  
        public static string GetOSVersion()
        {
            //UserAgent   
            var userAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];

            var osVersion = LanguageUtil.Translate("com_ClientHelper_GetOSVersion");

            if (userAgent.Contains("NT 6.1"))
            {
                osVersion = "Windows 7";
            }
            else if (userAgent.Contains("NT 6.0"))
            {
                osVersion = "Windows Vista/Server 2008";
            }
            else if (userAgent.Contains("NT 5.2"))
            {
                osVersion = "Windows Server 2003";
            }
            else if (userAgent.Contains("NT 5.1"))
            {
                osVersion = "Windows XP";
            }
            else if (userAgent.Contains("NT 5"))
            {
                osVersion = "Windows 2000";
            }
            else if (userAgent.Contains("NT 4"))
            {
                osVersion = "Windows NT4";
            }
            else if (userAgent.Contains("Me"))
            {
                osVersion = "Windows Me";
            }
            else if (userAgent.Contains("98"))
            {
                osVersion = "Windows 98";
            }
            else if (userAgent.Contains("95"))
            {
                osVersion = "Windows 95";
            }
            else if (userAgent.Contains("Mac"))
            {
                osVersion = "Mac";
            }
            else if (userAgent.Contains("Unix"))
            {
                osVersion = "UNIX";
            }
            else if (userAgent.Contains("Linux"))
            {
                osVersion = "Linux";
            }
            else if (userAgent.Contains("SunOS"))
            {
                osVersion = "SunOS";
            }
            return osVersion;
        }
        #endregion

        #region 获取客户端IP地址

        /// <summary>  
        /// 获取客户端IP地址  
        /// </summary>  
        /// <returns></returns>  
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                return "0.0.0.0";
            }
            return result;
        }

        #endregion

        #region 取客户端真实IP

        ///  <summary>    
        ///  取得客户端真实IP。如果有代理则取第一个非内网地址    
        ///  </summary>    
        public static string GetIPAddress
        {
            get
            {
                var result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(result))
                {
                    //可能有代理    
                    if (result.IndexOf(".") == -1)        //没有“.”肯定是非IPv4格式    
                        result = null;
                    else
                    {
                        if (result.IndexOf(",") != -1)
                        {
                            //有“,”，估计多个代理。取第一个不是内网的IP。    
                            result = result.Replace("  ", "").Replace("'", "");
                            string[] temparyip = result.Split(",;".ToCharArray());
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                if (IsIPAddress(temparyip[i])
                                        && temparyip[i].Substring(0, 3) != "10."
                                        && temparyip[i].Substring(0, 7) != "192.168"
                                        && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i];        //找到不是内网的地址    
                                }
                            }
                        }
                        else if (IsIPAddress(result))  //代理即是IP格式    
                            return result;
                        else
                            result = null;        //代理中的内容  非IP，取IP    
                    }

                }

                string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];

                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];

                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.UserHostAddress;

                return result;
            }
        }

        #endregion

        #region  判断是否是IP格式

        ///  <summary>  
        ///  判断是否是IP地址格式  0.0.0.0  
        ///  </summary>  
        ///  <param  name="str1">待判断的IP地址</param>  
        ///  <returns>true  or  false</returns>  
        public static bool IsIPAddress(string str1)
        {
            if (string.IsNullOrEmpty(str1) || str1.Length < 7 || str1.Length > 15) return false;

            const string regFormat = @"^d{1,3}[.]d{1,3}[.]d{1,3}[.]d{1,3}$";

            var regex = new Regex(regFormat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }

        #endregion

        #region 获取公网IP及省份信息
        /// <summary>  
        /// 获取公网IP及省份信息  
        /// </summary>  
        /// <returns></returns>  
        public static string GetNetIpAndCity(bool isNetIp = false)
        {
            var tempIp = "";
            var city = "";
            try
            {
                var wr = System.Net.WebRequest.Create("http://1111.ip138.com/ic.asp");
                var s = wr.GetResponse().GetResponseStream();
                var sr = new System.IO.StreamReader(s, Encoding.GetEncoding("gb2312"));
                var all = sr.ReadToEnd();

                var start = all.IndexOf("[", StringComparison.Ordinal) + 1;
                var end = all.IndexOf("]", start, StringComparison.Ordinal);
                tempIp = all.Substring(start, end - start);
                start = all.IndexOf("来自：", StringComparison.Ordinal) + 3;
                end = all.IndexOf("市", start, StringComparison.Ordinal) + 1;
                city = all.Substring(start, end - start);
                sr.Close();
                s.Close();
                if (!isNetIp) tempIp = GetIP();
            }
            catch
            {
                if (System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.Length > 1)
                    tempIp = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[1].ToString();
                if (string.IsNullOrEmpty(tempIp)) return GetIP() + " " + city;
            }
            return tempIp + " " + city;
        }
        #endregion

    }
}
