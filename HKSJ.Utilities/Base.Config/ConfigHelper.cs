using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace HKSJ.Utilities
{
    /// <summary>
    ///  Config配置文件 公共帮助类
    /// 版本：2.0
    /// <author>
    ///		<name>caoxilong</name>
    ///		<date>2013.09.27</date>
    /// </author>
    /// </summary>
    public  class ConfigHelper
    {
        /// <summary>
        /// 根据key获取配置value
        /// 没有则返回空字符串
        /// </summary>
        /// <param name="key"></param>
        public static string AppSettings(string key)
        {
            return GetSettingByKey(key, string.Empty);
        }

        /// <summary>
        /// 根据key获取配置value
        /// 没有则返回用户defaultVal
        /// </summary>
        /// <param name="configKey"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static string GetSettingByKey(string configKey, string defaultVal)
        {
            if (ConfigurationManager.AppSettings[configKey] != null)
            {
                string val = ConfigurationManager.AppSettings[configKey].Trim();
                return string.IsNullOrEmpty(val) ? defaultVal : val;
            }
            else
            {
                return defaultVal;
            }
        }

        /// <summary>
        /// 根据key获取配置value
        /// 没有则返回用户defaultVal
        /// </summary>
        /// <param name="configKey"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static string GetConnectionByKey(string configKey, string defaultVal)
        {
            if (ConfigurationManager.ConnectionStrings[configKey] != null)
            {
                string val = ConfigurationManager.ConnectionStrings[configKey].ConnectionString.Trim();
                return string.IsNullOrEmpty(val) ? defaultVal : val;
            }
            else
            {
                return defaultVal;
            }
        }
        /// <summary>
        /// 根据name取connectionString值
        /// </summary>
        /// <param name="name"></param>
        public static string ConnectionStrings(string name)
        {
            return GetConnectionByKey(name,string.Empty);
        }
        /// <summary>
        /// 根据Key修改Value
        /// </summary>
        /// <param name="key">要修改的Key</param>
        /// <param name="value">要修改为的值</param>
        public static void SetValue(string key, string value)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(HttpContext.Current.Server.MapPath("/Config/Config.xml"));
            var xNode = xDoc.SelectSingleNode("//appSettings");

            if (xNode != null)
            {
                var xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + key + "']");
                if (xElem1 != null) xElem1.SetAttribute("value", value);
                else
                {
                    var xElem2 = xDoc.CreateElement("add");
                    xElem2.SetAttribute("key", key);
                    xElem2.SetAttribute("value", value);
                    xNode.AppendChild(xElem2);
                }
            }
            xDoc.Save(HttpContext.Current.Server.MapPath("/Config/Config.xml"));
        }
    }
}
