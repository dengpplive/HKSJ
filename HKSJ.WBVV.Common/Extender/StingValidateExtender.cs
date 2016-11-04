using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Common.Extender
{
    /// <summary>
    /// 字符串校验
    /// </summary>
    public static class StingValidateExtender
    {

        /// <summary>
        /// 判断一个字符串是否为合法数字(不限制长度)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNumber(this string s)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(s, @"^\d*$"); 
        }

        /// <summary>
        /// 验证url地址格式是否正确
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public static bool IsUrl(this string requestUrl)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(requestUrl, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"); 
        }
    }
}
