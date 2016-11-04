using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace HKSJ.Utilities
{
    public class HttpHelper
    {
        static HttpHelper()
        {
            ServicePointManager.DefaultConnectionLimit = 512;
            UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.17 (KHTML, like Gecko) Chrome/24.0.1312.57 Safari/537.17";
            Timeout = 100000;
        }

        class MyWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address) as HttpWebRequest;
                if (request == null) return null;
                request.Timeout = Timeout;
                request.UserAgent = UserAgent;
                return request;
            }
        }

        /// <summary>
        /// 获取或设置 使用的UserAgent信息
        /// </summary>
        /// <remarks>
        /// 可以到<see cref="http://www.sum16.com/resource/user-agent-list.html"/>查看更多User-Agent
        /// </remarks>
        public static String UserAgent { get; set; }
        /// <summary>
        /// 获取或设置 请求超时时间
        /// </summary>
        public static Int32 Timeout { get; set; }
        public static String SMSBaseUri { get; set; }

        public static Boolean GetContentString(String url, out String message, Encoding encoding = null)
        {
            try
            {
                if (encoding == null) encoding = Encoding.UTF8;
                using (var wc = new MyWebClient())
                {
                    message = encoding.GetString(wc.DownloadData(url));
                    return true;
                }
            }
            catch (Exception exception)
            {
                message = exception.Message;
                return false;
            }
        }

        /// <summary>
        ///  POST数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static String Post(String url, Byte[] data, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            String str;
            using (var wc = new MyWebClient())
            {
                wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                var ret = wc.UploadData(url, "POST", data);
                str = encoding.GetString(ret);
            }
            return str;
        }

        public static Byte[] DownloadData(String address)
        {
            Byte[] data;
            using (var wc = new MyWebClient())
            {
                data = wc.DownloadData(address);
            }
            return data;
        }
        public static Int64 GetContentLength(String url)
        {
            Int64 length;
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.UserAgent = UserAgent;
            req.Method = "HEAD";
            req.Timeout = 5000;
            var res = (HttpWebResponse)req.GetResponse();
            if (res.StatusCode == HttpStatusCode.OK)
            {
                length = res.ContentLength;
            }
            else
            {
                length = -1;
            }
            res.Close();
            return length;
        }

        public static T SMSPost<T>(string absoluteUri, object value)
        {
            return SMSPost<T>(absoluteUri, value, HttpMethod.Post);
        }
        private static T SMSPost<T>(string absoluteUri, object value, HttpMethod method)
        {
            var responseResult = new HttpResponseMessage(HttpStatusCode.OK);
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(SMSBaseUri);
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
                    return (T)Convert.ChangeType(responseResult.Content.ReadAsStringAsync().Result, typeof(T));
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

        #region  添加请求方法
        private static HttpHelper model = null;
        public static HttpHelper CreatHelper()
        {
            if (model == null)
            {
                model = new HttpHelper();
            }
            return model;
        }
        /// <summary>
        /// 通过GET方法调用URL
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="ErrInfo">如果有错误，则返回错误信息,如果没有则返回控制符传</param>
        /// <param name="ResponseCode">状态,正常是200</param>
        /// <param name="TimeOut">超时时间(单位：秒)</param>
        /// <returns>返回字符串</returns>
        public string DoGet(string url, out string ErrInfo, out int ResponseCode, int TimeOut = 30)
        {
            ResponseCode = 400;
            ErrInfo = "";
            StreamReader sr = null;
            HttpWebResponse wr = null;
            HttpWebRequest hp = null;
            try
            {
                hp = (HttpWebRequest)WebRequest.Create(url);

                hp.Timeout = TimeOut * 1000;
                System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("UTF-8");

                wr = (HttpWebResponse)hp.GetResponse();
                sr = new StreamReader(wr.GetResponseStream(), encoding);
                ResponseCode = Convert.ToInt32(wr.StatusCode);
                string strData = sr.ReadToEnd();
                sr.Close();
                wr.Close();
                return strData;
            }
            catch (Exception exp)
            {
                ErrInfo += exp.Message;
                if (wr != null)
                {
                    ResponseCode = Convert.ToInt32(wr.StatusCode);
                }
                return "";
            }
        }

        /// <summary>
        /// 通过GET方法调用URL
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="TimeOut">超时时间(单位：秒)</param>
        /// <returns>返回字符串</returns>
        public string DoGet(string url, int TimeOut = 30)
        {
            int ResponseCode = 400;
            string ErrInfo = "";
            return DoGet(url, out ErrInfo, out ResponseCode, TimeOut);
        }
        /// <summary>
        /// 通过GET方法调用URL
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="TimeOut">超时时间(单位：秒)</param>
        /// <returns>返回实体</returns>
        public T DoGetObject<T>(string url, int TimeOut = 30)
        {
            string jsonString = DoGet(url, TimeOut);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// 通过POST方法调用URL
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="postData">POST数据</param>
        /// <param name="ErrInfo">如果有错误，则返回错误信息,如果没有则返回控制符传</param>
        /// <param name="ResponseCode">状态,正常是200</param>
        /// <param name="TimeOut">超时时间(单位：秒)</param>
        /// <returns>返回字符串</returns>
        public string DoPost(string url, string postData, out string ErrInfo, out int ResponseCode, int TimeOut = 30)
        {
            ResponseCode = 400;
            ErrInfo = "";
            StreamReader sr = null;
            HttpWebResponse wr = null;
            HttpWebRequest hp = null;
            try
            {
                hp = (HttpWebRequest)WebRequest.Create(url);
                hp.Timeout = TimeOut * 1000;
                if (postData != null)
                {
                    byte[] data = Encoding.UTF8.GetBytes(postData);
                    hp.Method = "POST";
                    hp.ContentType = "application/Json";
                    hp.ContentLength = data.Length;
                    Stream ws = hp.GetRequestStream();
                    // 发送数据
                    ws.Write(data, 0, data.Length);
                    ws.Close();
                }

                wr = (HttpWebResponse)hp.GetResponse();
                sr = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                ResponseCode = Convert.ToInt32(wr.StatusCode);
                string result = sr.ReadToEnd(); ;
                return result;
            }
            catch (Exception exp)
            {
                ErrInfo += exp.Message;
                if (wr != null)
                {
                    ResponseCode = Convert.ToInt32(wr.StatusCode);
                }
                return "";
            }
            finally
            {
                try
                {
                    if (hp != null)
                    {
                        hp.Abort();
                        hp = null;
                    }
                    if (sr != null)
                    {
                        sr.Close();
                        sr = null;
                    }
                    if (wr != null)
                    {
                        wr.Close();
                        wr = null;
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 通过POST方法调用URL
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="postData">POST数据</param>
        /// <param name="TimeOut">超时时间(单位：秒)</param>
        /// <returns>返回字符串</returns>
        public string DoPost(string url, string postData, int TimeOut = 30)
        {
            int ResponseCode = 400;
            string ErrInfo = "";
            return DoPost(url, postData, out ErrInfo, out ResponseCode, TimeOut);
        }

        /// <summary>
        /// PostXML数据到服务器及获取返回的xml值
        /// </summary>
        public string HttpPost(string url, string data)
        {
            string res = "";
            string postData = data;	//xml数据
            string Web = url;	//网关地址

            try
            {
                //将数据提交到快钱服务器
                WebRequest myWebRequest = WebRequest.Create(url);
                myWebRequest.Method = "POST";
                myWebRequest.ContentType = "application/x-www-form-urlencoded";
                Stream streamReq = myWebRequest.GetRequestStream();
                byte[] byteArray = Encoding.GetEncoding("utf-8").GetBytes(postData);
                streamReq.Write(byteArray, 0, byteArray.Length);
                streamReq.Close();

                //获取服务器返回的XML数据
                WebResponse myWebResponse = myWebRequest.GetResponse();
                StreamReader sr = new StreamReader(myWebResponse.GetResponseStream());
                res = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception e)
            {
                res = e.Message.ToString();
            }

            return res; //返回数据
        }

        /// <summary>
        /// 通过POST方法调用URL
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="postBody">POST数据</param>
        /// <param name="TimeOut">超时时间(单位：秒)</param>
        /// <returns>返回字符串</returns>
        public string DoPost(string url, object postBody, int TimeOut = 30)
        {
            return DoPost(url, postBody.ToJson(), TimeOut);
        }
        /// <summary>
        /// 通过POST方法调用URL
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="postData">POST数据</param>
        /// <param name="TimeOut">超时时间(单位：秒)</param>
        /// <returns>返回实体</returns>
        public T DoPostObject<T>(string url, string postData, int TimeOut = 30)
        {
            return DoPost(url, postData, TimeOut).JsonToEntity<T>();
        }
        /// <summary>
        /// 通过POST方法调用URL
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="postBody">POST数据</param>
        /// <param name="TimeOut">超时时间(单位：秒)</param>
        /// <returns>返回实体</returns>
        public T DoPostObject<T>(string url, object postBody, int TimeOut = 30)
        {
            return DoPost(url, postBody, TimeOut).JsonToEntity<T>();
        }
        #endregion
    }
}
