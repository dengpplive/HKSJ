using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HKSJ.WBVV.Common.Logger;
using HKSJ.WBVV.Common.Resource;
using Microsoft.Owin;

namespace HKSJ.WBVV.Api.Host.ConsoleLog
{
    public class MessageDispatcher : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var result = new Task<HttpResponseMessage>(() =>
            {
                string wcfRequestID = Guid.NewGuid().ToString("N");
                Thread.CurrentThread.Name = wcfRequestID;

                #region 请求参数
                var refererUrl = request.Headers.Referrer == null ? "" : request.Headers.Referrer.ToString();
                var owinContext = request.Properties["MS_OwinContext"] as OwinContext;
                var ipAddress = "";
                if (owinContext != null)
                {
                    ipAddress = owinContext.Request.RemoteIpAddress;
                }
                var userAgent = request.Headers.UserAgent == null ? "" : request.Headers.UserAgent.ToString();
                var contentType = request.Content.Headers.ContentType == null ? "" : request.Content.Headers.ContentType.ToString();
                var data = request.Content.ReadAsStringAsync().Result;
                var method = request.Method.ToString();
                var requestUrl = request.RequestUri.AbsoluteUri;
                var requestPath = request.RequestUri.AbsolutePath;
                if (!string.IsNullOrEmpty(requestPath))
                {
                    var pathName = requestPath.Substring(1, requestPath.Length - 1);

                    switch (pathName.Trim().ToLower())
                    {
                        case "robots.txt":
                            return new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = new StringContent("User-agent: *\nDisallow: /", Encoding.GetEncoding("UTF-8"), "text/plain")
                            };
                        case "favicon.ico":
                            var favResponse = new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.Redirect
                            };
                            favResponse.Headers.Location = new Uri(UrlHelper.Combine(ConfigurationManager.AppSettings["WebServerUrl"], "favicon.ico"));
                            return favResponse;
                        case "":
                        case "/":
                        case "api":
                        case "api/":
                            return new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = new StringContent("API host is running...", Encoding.GetEncoding("UTF-8"), "text/plain")
                            };
                        case "api/doc":
                        case "api/doc/":
                            var docResponse = new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.Redirect
                            };
                            docResponse.Headers.Location =
                                new Uri(UrlHelper.Combine(request.RequestUri.Scheme + "://" + request.RequestUri.Authority, "/api/doc/getdoc"));
                            return docResponse;
                    }
                }
                #endregion

                Task<HttpResponseMessage> sendTask = base.SendAsync(request, cancellationToken);
                var timer = Stopwatch.StartNew();
                double duration = 0.0;
                try
                {
                    sendTask.Wait(cancellationToken);
                    if (sendTask.Status == TaskStatus.Canceled)
                    {
                        throw new OperationCanceledException();
                    }
                    duration = timer.Elapsed.TotalMilliseconds;
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    timer.Stop();
                    timer = null;
                    Console.WriteLine("{0,7}:{1,6} {2}>{3}", duration, request.Method, sendTask.Result.StatusCode, request.RequestUri.AbsoluteUri);
                }
                var content = sendTask.Result.Content;
                var reslt = string.Empty;
                if (content != null)
                {
                    reslt = content.ReadAsStringAsync().Result;
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("IpAddress:{0}\t\n", ipAddress);
                sb.AppendFormat("UserAgent:{0}\t\n", userAgent);
                sb.AppendFormat("Referer:{0}\t\n", refererUrl);
                sb.AppendFormat("Url:{0}\t\n", requestUrl);
                sb.AppendFormat("ContentType:{0}\t\n", contentType);
                sb.AppendFormat("Method:{0}\t\n", method);
                sb.AppendFormat("Data:{0}\t\n", data);
                sb.AppendFormat("Exception:{0}\t\n", reslt);
                if (sendTask.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    LogBuilder.Log4Net.Info(sb.ToString());
                }
                else
                {
#if !DEBUG
                         LogBuilder.Log4Net.Error(sb.ToString());
#else
                    Console.WriteLine(sb.ToString());
#endif
                }
                sendTask.Result.Headers.Add("Access-Control-Allow-Origin", "*");
                //sendTask.Result.Content = new StringContent(userName, Encoding.GetEncoding("UTF-8"), "application/json");
                return sendTask.Result;
            }, cancellationToken);
            result.Start();
            return result;
        }
    }
}
