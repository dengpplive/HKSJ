using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Security.Cryptography;
using HKSJ.WBVV.Common.Assert;
using System.Text.RegularExpressions;
using System.Web;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Common.Extender
{
    /// <summary>
    /// 字符串拓展类
    /// </summary>
    public static class StringExtender
    {
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UrlEncode(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            return HttpUtility.UrlEncode(input);
        }
        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UrlDecode(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            return HttpUtility.UrlDecode(input);
        }
        /// <summary>
        /// 拼接URL
        /// </summary>
        /// <param name="baseUrlPath"></param>
        /// <param name="additionalNode"></param>
        /// <returns></returns>
        public static string Link(this string baseUrlPath, object additionalNode)
        {
            if (additionalNode == null)
            {
                return baseUrlPath;
            }
            if (baseUrlPath == null)
            {
                return additionalNode.ToString();
            }
            return Link(baseUrlPath, additionalNode.ToString());
        }
        /// <summary>
        /// 拼接URL
        /// </summary>
        /// <param name="baseUrlPath"></param>
        /// <param name="additionalNode"></param>
        /// <returns></returns>
        public static string Link(this string baseUrlPath, string additionalNode)
        {
            if (baseUrlPath == null)
            {
                return additionalNode;
            }
            if (additionalNode == null)
            {
                return baseUrlPath;
            }
            if (baseUrlPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                if (additionalNode.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    baseUrlPath = baseUrlPath.TrimEnd(new char[] { '/' });
                }
                return baseUrlPath + additionalNode;
            }
            if (additionalNode.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                return baseUrlPath + additionalNode;
            }
            return baseUrlPath + "/" + additionalNode;
        }
        /// <summary>
        /// 拼接URL
        /// </summary>
        /// <param name="baseUrlPath"></param>
        /// <param name="additionalNodes"></param>
        /// <returns></returns>
        public static string Link(this string baseUrlPath, params object[] additionalNodes)
        {
            var temp = baseUrlPath;
            foreach (var item in additionalNodes)
            {
                temp = temp.Link(item);
            }
            return temp;
        }
        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        /// <summary>
        /// 格式化输出
        /// </summary>
        /// <param name="format"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Format(this string format, params object[] values)
        {
            return string.Format(format, values);
        }

        public static string EnSure(this string input, string defaultValue)
        {
            return string.IsNullOrEmpty(input) ? defaultValue : input;
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="normalString">普通的字符串</param>
        /// <returns>加密后的二进制字符串</returns>
        /// <remarks>不可逆加密</remarks>
        public static string Encrypt(this string normalString)
        {
            AssertUtil.NotNullOrWhiteSpace(normalString, LanguageUtil.Translate("com_StringExtender_check_encryption_string_null"));
            UnicodeEncoding encoding = new UnicodeEncoding();
            HashAlgorithm algroithm = new MD5CryptoServiceProvider();
            byte[] buffer = algroithm.ComputeHash(encoding.GetBytes(normalString));
            return BitConverter.ToString(buffer);
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="template">字符串模板</param>
        /// <param name="args">参数列表</param>
        /// <returns>格式化后的字符串</returns>
        public static string F(this string template, params object[] args)
        {
            return string.Format(template, args);
        }

        const string RTemplate = "|{0}|";
        /// <summary>
        /// 代替字符串
        /// </summary>
        /// <param name="template">模版</param>
        /// <param name="oldValue">模版字符串</param>
        /// <param name="newValue">实际数据</param>
        /// <returns></returns>
        public static string R(this string template, string oldValue, string newValue)
        {
            return template.Replace(RTemplate.F(oldValue), newValue);
        }

        /// <summary>
        /// 测试字符串是否为空或者空字符组成
        /// </summary>
        /// <param name="msg">字符串</param>
        /// <returns>为空返回true</returns>
        public static bool IsNullOrWhiteSpace(this string msg)
        {
            return msg == null || msg == string.Empty;
        }

        /// <summary>
        /// 测试字符串不是为空或者空字符组成
        /// </summary>
        /// <param name="msg">字符串</param>
        /// <returns>为空返回true</returns>
        public static bool IsNotNullOrWhiteSpace(this string msg)
        {
            return !(msg == null || msg == string.Empty);
        }


        public static string FormatNull(this string msg)
        {
            if (msg.IsNullOrWhiteSpace())
                return msg;
            else
                return msg.Trim();
        }

        /// <summary>
        /// 字符串略缩显示
        /// </summary>
        /// <param name="msg">原字符串</param>
        /// <param name="length">长度</param>
        /// <returns>略缩后的字符串</returns>
        public static string Abbreviation(this string msg, int length)
        {
            if (msg.Length > length)
                return msg.Substring(0, length) + "...";
            else
                return msg;
        }

        /// <summary>
        /// 重复字符串
        /// </summary>
        /// <param name="msg">原字符串</param>
        /// <param name="count">重复次数</param>
        /// <returns>重复字符串</returns>
        public static string Duplication(this string msg, int count)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                builder.Append(msg);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 判断是否包含子字符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="tmp">包含的子字符串</param>
        /// <returns>是否</returns>
        public static bool ContainsIgnoreCase(this string str, string tmp)
        {
            return str.ToLower().Contains(tmp.ToLower());
        }

        /// <summary>
        /// 比较字符串（不区分大小写）
        /// </summary>
        /// <param name="str">字符串A</param>
        /// <param name="str2">字符串B</param>
        /// <returns>是否相等</returns>
        public static bool EqualsIgnoreCase(this string str, string str2)
        {
            return str.Equals(str2, StringComparison.CurrentCultureIgnoreCase);
        }



        /// <summary>
        /// 反转字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string ReverseFormat(this string str, char split = ',')
        {
            var tmps = str.Split(split).Reverse().ToArray();
            return string.Join(split.ToString(), tmps);
        }

        /// <summary>
        /// 清除无效的xml字符信息
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string CleanInvalidXmlChars(this string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }

        /// <summary>
        /// 特殊字符文本替换为XML格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToXMLContent(this string s)
        {
            if (s == null) return "";
            return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }


        /// <summary>
        /// 获取字符串哈希值
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SHA1(this string text)
        {
            byte[] cleanbytes = Encoding.Default.GetBytes(text);
            byte[] hashbytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanbytes);
            return BitConverter.ToString(hashbytes);
        }
    }
}
