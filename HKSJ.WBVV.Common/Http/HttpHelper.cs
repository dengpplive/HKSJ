using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Mime;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Common.Http
{
    public class ResponseModel
    {
        /// <summary>
        /// 响应状态
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// 响应是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 响应内容
        /// </summary>
        public string Body { get; set; }
    }

    public class HttpHelper
    {

        private const string DefaultAccept = "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        private const string DefaultAcceptEncoding = "gzip, deflate";
        private const string DefaultAcceptLanguage = "zh-CN,zh;q=0.8";
        private const string DefaultUserAgent = "User-Agent: Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36";
        private const string DefaultContentType = "application/x-www-form-urlencoded";

        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="requestHeader">请求报头</param>
        /// <param name="requestBody">POST的数据</param>
        /// <param name="refererUrl">客户端请求地址url</param>
        /// <param name="host">客户端主机:端口</param>
        /// <param name="contentType">请求内容类型</param>
        /// <returns></returns>
        public static string HttpPost(string requestUrl, string requestHeader, string requestBody, string refererUrl, string host, string contentType)
        {
            return HttpRequest(requestUrl, "POST", requestHeader, requestBody, null, contentType, refererUrl, null, null, host, null, Encoding.UTF8);
        }

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="requestHeader">请求报头</param>
        /// <param name="refererUrl">客户端请求地址url</param>
        /// <param name="host">客户端主机:端口</param>
        /// <returns></returns>
        public static string HttpGet(string requestUrl, string requestHeader, string refererUrl, string host)
        {
            return HttpRequest(requestUrl, "GET", requestHeader, null, null, null, refererUrl, null, null, host, null, Encoding.UTF8);
        }


        /// <summary>
        /// HTTP 请求数据.
        /// </summary>
        /// <param name="requestUrl">请求url地址</param>
        /// <param name="requestMethod">请求方式</param>
        /// <param name="requestHeader">请求头部参数</param>
        /// <param name="requestBody">请求内容</param>
        /// <param name="timeout">请求超时时间</param>
        /// <param name="requestEncoding">请求时所用的编码</param>
        /// <param name="requestCacheLevel">请求的缓存级别</param>
        /// <param name="contentType">请求内容类型</param>
        /// <param name="refererUrl">客户端请求地址url</param>
        /// <param name="accept">客户端可以接受任何数据类型</param>
        /// <param name="userAgent">客户端浏览器类型</param>
        /// <param name="host">客户端主机:端口</param>
        /// <param name="cookies">cookies</param>
        /// <returns></returns>
        public static string HttpRequest(string requestUrl,
                                        string requestMethod,
                                        string requestHeader,
                                        string requestBody,
                                        int? timeout,
                                        string contentType,
                                        string refererUrl,
                                        string accept,
                                        string userAgent,
                                        string host,
                                        CookieCollection cookies,
                                        Encoding requestEncoding,
                                        RequestCacheLevel requestCacheLevel = RequestCacheLevel.Default)
        {
            AssertUtil.NotNullOrWhiteSpace(requestUrl, LanguageUtil.Translate("com_HttpHelper_HttpRequest_check_url"));
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(requestUrl);
                request.Method = requestMethod;
                request.Accept = string.IsNullOrEmpty(accept) ? DefaultAccept : accept;
                request.UserAgent = string.IsNullOrEmpty(userAgent) ? DefaultUserAgent : userAgent;
                request.ContentType = string.IsNullOrEmpty(contentType)
                    ? DefaultContentType
                    : contentType;
                request.Timeout = timeout.HasValue ? timeout.Value : 100000;
                request.CachePolicy = new RequestCachePolicy(requestCacheLevel);
                if (!string.IsNullOrEmpty(refererUrl))
                {
                    request.Referer = refererUrl;
                }
                if (!string.IsNullOrEmpty(host))
                {
                    request.Host = host;
                }
                if (!string.IsNullOrEmpty(requestHeader))
                {
                    if (requestHeader.IndexOf('&') != -1)
                    {
                        var compareArr = requestHeader.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                        if (compareArr.Length > 0)
                        {
                            foreach (var s in compareArr)
                            {
                                if (s.IndexOf('=') != -1)
                                {
                                    var dic = s.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (dic.Length == 2)
                                    {
                                        request.Headers.Add(dic[0], dic[1]);
                                    }
                                }
                            }
                        }
                    }
                }
                if (!request.Headers.AllKeys.Contains("Accept-Encoding"))
                {
                    request.Headers.Add("Accept-Encoding", DefaultAcceptEncoding);
                }
                if (!request.Headers.AllKeys.Contains("Accept-Language"))
                {
                    request.Headers.Add("Accept-Language", DefaultAcceptLanguage);
                }
                if (cookies != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }
                if ("post" == requestMethod.Trim().ToLower())
                {
                    byte[] data = requestEncoding.GetBytes(requestBody);
                    request.ContentLength = data.Length;
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                return ResponseBody(request);
            }
            catch (HttpException httpException)
            {
                var responseModel = new ResponseModel()
                {
                    StatusCode = httpException.GetHttpCode(),
                    Success = false,
                    Body = httpException.GetHtmlErrorMessage()
                };
                return responseModel.ToJSON();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                request.Abort();
                request = null;
            }
        }
        /// <summary>
        /// 响应内容
        /// </summary>
        /// <returns></returns>
        private static string ResponseBody(HttpWebRequest request)
        {
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return ResponseBody(response);
                }
            }
            catch (WebException ex)
            {
                using (HttpWebResponse response = (HttpWebResponse)ex.Response)
                {
                    return ResponseBody(response);
                }
            }
        }
        /// <summary>
        /// 响应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string ResponseBody(HttpWebResponse response)
        {
            int statusCode = Convert.ToInt32(response.StatusCode);
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string responseBody = reader.ReadToEnd();
                    var responseModel = new ResponseModel()
                    {
                        StatusCode = statusCode,
                        Success = statusCode == 200,
                        Body = responseBody
                    };
                    return responseModel.ToJSON();
                }
            }

        }
    }
}
