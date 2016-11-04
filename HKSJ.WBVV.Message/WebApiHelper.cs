using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Http;
using HKSJ.WBVV.Common.Logger;
using HKSJ.WBVV.Message.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace HKSJ.WBVV.Message
{
    public class WebApiHelper
    {
        private static string _baseAddress;
        static WebApiHelper()
        {
            _baseAddress = WebConfig.BaseAddress;
        }

        public static T InvokeApi<T>(string absoluteUri)
        {
            return InvokeApi<T>(absoluteUri, null, HttpMethod.Get);
        }
        public static T InvokeApi<T>(string absoluteUri, object value)
        {
            return InvokeApi<T>(absoluteUri, value, HttpMethod.Post);
        }
        private static T InvokeApi<T>(string absoluteUri, object value, HttpMethod method)
        {
            var responseResult = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_baseAddress);
                if (method == HttpMethod.Post)
                {
                    responseResult = httpClient.PostAsJsonAsync(absoluteUri, value).Result;
                }
                else if (method == HttpMethod.Get)
                {
                    responseResult = httpClient.GetAsync(absoluteUri).Result;
                }
            }
            try
            {
                if (!responseResult.IsSuccessStatusCode) return default(T);
                if (typeof(T).Name == "String")
                    return (T)Convert.ChangeType(responseResult.Content.ReadAsStringAsync().Result, typeof(T));
                return responseResult.Content.ReadAsAsync<T>().Result;
            }
            catch
            {
                return default(T);
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
#if !DEBUG
                LogBuilder.Log4Net.Error(response.ReasonPhrase, new Exception(response.RequestMessage.ToString()));
#else

#endif

                }
                else
                {
#if !DEBUG
                   LogBuilder.Log4Net.Error((int)response.StatusCode+"", new Exception(response.RequestMessage.ToString()));
#else

#endif

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
        private static void BuildLogHeaders(HttpClient client)
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
#if !DEBUG
                  LogBuilder.Log4Net.Error("BuildLogHeaders",ex.MostInnerException());
#else

#endif

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