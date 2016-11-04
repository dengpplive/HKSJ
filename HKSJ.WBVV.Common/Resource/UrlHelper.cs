using HKSJ.Utilities;
using System;
using System.Web;

namespace HKSJ.WBVV.Common.Resource
{
    public static class UrlHelper
    {
        /// <summary>
        /// 读取App.Config中的QiniuPublicDomain，合并resourceName生成完整的Url
        /// </summary>
        /// <param name="resourceName">七牛Resource Key</param>
        /// <returns>以http://开头的Url或者null</returns>
        public static string QiniuPublicCombine(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName) || resourceName.StartsWith("http://") || resourceName.StartsWith("https://"))
            {
                return resourceName;
            }
            else
            {
                return "http://" + VirtualPathUtility.AppendTrailingSlash(ConfigHelper.AppSettings("QiniuPublicDomain")) + resourceName;
            }
        }

        /// <summary>
        /// Url路径合并
        /// </summary>
        /// <param name="paths">路径列表</param>
        /// <returns>合并后的返回值</returns>
        public static string Combine(params string[] paths)
        {
            string returnStr = string.Empty;

            if (paths != null && paths.Length > 0)
            {
                returnStr = paths[0];

                for (int i = 1; i < paths.Length; i++)
                {
                    if (returnStr.EndsWith("/") || paths[i].StartsWith("/"))
                        returnStr += paths[i];
                    else
                        returnStr += "/" + paths[i];
                }
            }

            return returnStr;
        }
    }
}
