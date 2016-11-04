// All Rights Reserved , Copyright © HKSJ 2013
//=====================================================================================
using System;
using System.Collections.Specialized;
using System.Web;

namespace HKSJ.Utilities
{
    /// <summary>
    /// Cookie帮助类
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            var cookie = HttpContext.Current.Request.Cookies[strName] ?? new HttpCookie(strName);
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            var cookie = HttpContext.Current.Request.Cookies[strName] ?? new HttpCookie(strName);
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 添加为Cookie.Values集合
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="keyValues"></param>
        /// <param name="expires">>COOKIE对象有效时间（秒数），1表示永久有效，0和负数都表示不设有效时间，大于等于2表示具体有效妙数，31536000秒=1年=(60*60*24*365)</param>
        public static void AddCookie(string cookieName, NameValueCollection keyValues, int expires)
        {
            var cookie = new HttpCookie(cookieName);
            foreach (string keyValue in keyValues)
            {
                cookie[keyValue] = keyValues[keyValue];
            }
            if (expires > 0)
            {
                cookie.Expires = expires == 1 ? DateTime.MaxValue : DateTime.Now.AddSeconds(expires);
            }
            AddCookie(cookie);
        }
        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookie"></param>
        public static void AddCookie(HttpCookie cookie)
        {
            var response = HttpContext.Current.Response;
            //指定客户端脚本是否可以访问[默认为false]
            cookie.HttpOnly = true;
            //指定统一的Path:/，比便能通存通取
            cookie.Path = HttpContext.Current.Request.ApplicationPath;
            //设置跨域,这样在其它二级域名下就都可以访问到了
            //例如:cookie.Domain = "chinesecoo.com";
            response.AppendCookie(cookie);
            response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            return HttpContext.Current.Request.Cookies[strName] != null ? HttpContext.Current.Request.Cookies[strName].Value : "";
        }

        /// <summary>
        /// 获得Cookie的值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookie(string cookieName, string key)
        {
            var request = HttpContext.Current.Request;
            return GetCookieValue(request.Cookies[cookieName], key);
        }

        /// <summary>
        /// 获得Cookie的子键值
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookieValue(HttpCookie cookie, string key)
        {
            if (cookie == null) return "";
            if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                return cookie.Values[key];
            return cookie.Value;
        }

        public static string GetHostInfo()
        {
            return HttpContext.Current.Request.Headers["Host"] ?? "";
        }

        /// <summary>
        /// 删除Cookie对象
        /// </summary>
        /// <param name="cookiesName">Cookie对象名称</param>
        public static void DelCookie(string cookiesName)
        {
            var request = HttpContext.Current.Request;
            var cookie = request.Cookies[cookiesName];
            if (cookie != null)
            {
                cookie.Path = HttpContext.Current.Request.ApplicationPath;
                cookie.Expires = DateTime.Now.AddYears(-5);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                var objCookie = new HttpCookie(cookiesName.Trim())
                {
                    Expires = DateTime.Now.AddYears(-5),
                    Path = HttpContext.Current.Request.ApplicationPath
                };
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }


    }
}
