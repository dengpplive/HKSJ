using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Http;
using HKSJ.WBVV.MVC.Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HKSJ.Utilities.Base.Security;
using Newtonsoft.Json;


namespace HKSJ.WBVV.MVC.Common
{
    public class WebApiHelper
    {
        private static string _baseAddress;

        private const string PublicKey = "AwEAAe18IfcCMADOaZA4DbBpnSQrOj7AMXmHVU7LTotRl/EicHlcOhwwieBvLlH2NrW/tcrWG2FxLQmaBxIa1cqdKSgLNDMnUrzXvdFE5uuWtD5nuFM0KDhqcnes5NMNKGuQWCu93MBch8YXVLHdOUMK09IgQcDJo5ajyVu+AmMCKeE7"; // TODO 可配置 AxOne
        private const string EncryptValue = "5bvv"; // TODO 可配置 AxOne

        static WebApiHelper()
        {
            _baseAddress = WebConfig.BaseAddress;
        }

        public static T InvokeApi<T>(string absoluteUri, string token = "")
        {
            return InvokeApi<T>(absoluteUri, null, HttpMethod.Get, token);
        }
        public static T InvokeApi<T>(string absoluteUri, object value, string token = "")
        {
            return InvokeApi<T>(absoluteUri, value, HttpMethod.Post, token);
        }

        public static T InvokeApi<T>(string absoluteUri, object value, bool isPost, string token = "")
        {
            return isPost ? InvokeApi<T>(absoluteUri, value, HttpMethod.Post, token) : InvokeApi<T>(absoluteUri, null, HttpMethod.Get, token);
        }

        public static T InvokeAxApi<T>(string url, object para, bool isPost = true, string token = "")
        {
            var response = InvokeApi<string>(url, para, isPost, token);
            if (string.IsNullOrWhiteSpace(response))
            {
                return default(T);
            }
            var result = JsonConvert.DeserializeObject(response, typeof(T));
            return (T)Convert.ChangeType(result, typeof(T));
        }

        private static T InvokeApi<T>(string absoluteUri, object value, HttpMethod method, string token = "")
        {
            var responseResult = new HttpResponseMessage(HttpStatusCode.OK);
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_baseAddress);
                var appkey = RSAHelper.EncryptString(EncryptValue, PublicKey);
                httpClient.DefaultRequestHeaders.Add("appkey", appkey);
                //将激活的语言写入header
                httpClient.DefaultRequestHeaders.Add("ActiveLanguage", WebConfig.ActiveLanguage);

                if (!string.IsNullOrWhiteSpace(token))
                {
                    httpClient.DefaultRequestHeaders.Add("token", token);//TODO uid type 添加
                }
                if (method == HttpMethod.Post)
                {
                    responseResult = PerformActionSafe(() => (httpClient.PostAsJsonAsync(absoluteUri, value)).Result);
                }
                else if (method == HttpMethod.Get)
                {
                    responseResult = PerformActionSafe(() => (httpClient.GetAsync(absoluteUri)).Result);
                }
            }
            try
            {
                if (!responseResult.IsSuccessStatusCode) return default(T);
                if (typeof(T).Name == "String")
                {
                    return (T)Convert.ChangeType(responseResult.Content.ReadAsStringAsync().Result, typeof(T));
                }
                return responseResult.Content.ReadAsAsync<T>().Result;
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 捕获异常
        /// Author : axone
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private static HttpResponseMessage PerformActionSafe(Func<HttpResponseMessage> action)
        {
            try
            {
                return action();
            }
            catch (AggregateException aex)
            {
                Exception firstException = null;
                if (aex.InnerExceptions != null && aex.InnerExceptions.Any())
                {
                    firstException = aex.InnerExceptions.First();
                    if (firstException.InnerException != null)
                    {
                        firstException = firstException.InnerException;
                    }
                }
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(firstException != null ? firstException.ToString() : "没有内部异常信息")
                };
                return response;
            }
        }

        public static async Task<T> DeleteAsync<T>(string requestUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress.Link(""));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                BuildLogHeaders(client);
                var response = await client.DeleteAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    VerifyStatus(response);
                }
                return default(T);
            }
        }
        private static void VerifyStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    WebLog.Error(new UnauthorizedAccessException(response.ReasonPhrase, new Exception(response.RequestMessage.ToString())));
                }
                else
                {
                    WebLog.Error(new HttpException((int)response.StatusCode, response.ReasonPhrase, new Exception(response.RequestMessage.ToString())));
                }
            }
        }
        public static async Task<string> GetString(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
                return await client.GetStringAsync(url);
            }
        }
        public static async Task<T> GetAsync<T>(string requestUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress.Link(""));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                BuildLogHeaders(client);
                var response = await client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    VerifyStatus(response);
                }
                return default(T);
            }
        }
        public static async Task<string> GetAsync(string requestUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress.Link(""));

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                BuildLogHeaders(client);
                var response = await client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    VerifyStatus(response);
                }
                return null;
            }
        }
        public static async Task<T> PostAsync<T>(string requestUri, object data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress.Link(""));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                BuildLogHeaders(client);
                var response = await client.PostAsJsonAsync(requestUri, data);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    VerifyStatus(response);
                }
                return default(T);
            }
        }
        public static bool PostAsyncTag(string requestUri, byte[] data)
        {
            WebClient wc = new WebClient();
            wc.Headers["content-type"] = "application/x-www-form-urlencoded";
            wc.Headers["Accept"] = "text/html, application/xhtml+xml, */*";
            wc.Headers["Accept-Encoding"] = "gzip, deflate";
            wc.Headers["Accept-Language"] = "zh-CN";
            wc.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
            wc.Headers["Pragma"] = "no-cache";
            wc.Headers["DNT"] = "1";
            wc.UploadData(_baseAddress + requestUri, data);
            return true;
        }
        public static async Task<string> PostAsync(string requestUri, object data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress.Link(""));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsJsonAsync(requestUri, data);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    VerifyStatus(response);
                }
                return null;
            }
        }
        public static async Task<T> PostFormAsync<T>(string requestUri, List<KeyValuePair<string, string>> data)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new FormUrlEncodedContent(data);
                var response = await client.PostAsync(requestUri, content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    VerifyStatus(response);
                }
                return default(T);
            }
        }
        public static async Task<string> PostFileAsync(string url, string name, string fileName, byte[] fileContent, string contentType, string ip)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent("---------------------------" + DateTime.Now.Ticks.ToString("x")))
                {
                    var file = new ByteArrayContent(fileContent);
                    file.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    var con = new ContentDispositionHeaderValue("form-data");
                    con.Parameters.Add(new NameValueHeaderValue("Name", '"' + name + '"'));
                    con.Parameters.Add(new NameValueHeaderValue("FileName", '"' + fileName + '"'));
                    file.Headers.ContentDisposition = con;

                    content.Add(file);
                    if (!string.IsNullOrEmpty(ip))
                    {
                        client.DefaultRequestHeaders.Remove("X_FORWARDED_FOR");
                        client.DefaultRequestHeaders.Add("X_FORWARDED_FOR", ip);
                    }
                    var response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        VerifyStatus(response);
                    }
                    return null;
                }
            }
        }
        public static async Task<T> PutAsync<T>(string requestUri, object data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress.Link(""));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                BuildLogHeaders(client);
                var response = await client.PutAsJsonAsync(requestUri, data);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    VerifyStatus(response);
                }
                return default(T);
            }
        }
        private static void BuildLogHeaders(HttpClient client, string token = "")
        {
            try
            {
                //登录信息
                //client.DefaultRequestHeaders.Authorization = CreateBasicCredentials(Ticket.LoginName, Ticket.Id, Ticket.ChineseName);
                //填充日志所需头
                if (HttpContext.Current != null)
                {
                    var rid = HttpContext.Current.Items["__RIO_RequestID"] as string;
                    if (rid == null)
                    {
                        rid = Guid.NewGuid().ToString("N");
                        HttpContext.Current.Items["__RIO_RequestID"] = rid;
                    }
                    client.DefaultRequestHeaders.Add("appkey", "M44NJ725jfnbT7PLkLfarvhpD2DZ7");//TODO 可配置 appkey
                    client.DefaultRequestHeaders.Add("token", token);//TODO 可配置 appkey
                    client.DefaultRequestHeaders.Add("X-RIO-RequestID", rid.UrlEncode());
                    client.DefaultRequestHeaders.Add("X-RIO-UserIP", ClientHelper.GetIP().UrlEncode());
                    client.DefaultRequestHeaders.Add("X-RIO-LoginUserName", HttpContext.Current.User.Identity.Name.UrlEncode());
                    client.DefaultRequestHeaders.Add("X-RIO-UserName", HttpContext.Current.User.Identity.Name.UrlEncode());
                    client.DefaultRequestHeaders.Add("X-RIO-Url", HttpContext.Current.Request.Url.AbsoluteUri.UrlEncode());
                    client.DefaultRequestHeaders.Add("X-RIO-UserAgent", HttpContext.Current.Request.UserAgent.UrlEncode());
                    var referrer = HttpContext.Current.Request.UrlReferrer == null ? "" : HttpContext.Current.Request.UrlReferrer.AbsoluteUri;
                    client.DefaultRequestHeaders.Add("X-RIO-Referer", referrer.UrlEncode());
                }
            }
            catch (Exception ex)
            {
                WebLog.Error(ex);
            }
        }
        private static AuthenticationHeaderValue CreateBasicCredentials(string userName, int staffId, string chineseName)
        {
            string toEncode = userName + ":" + staffId + ":" + chineseName;
            var toBase64 = Encoding.UTF8.GetBytes(toEncode);
            var base64 = Convert.ToBase64String(toBase64);
            return new AuthenticationHeaderValue("Basic", base64);
        }
    }
}