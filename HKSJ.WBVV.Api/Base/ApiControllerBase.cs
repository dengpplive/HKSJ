using System.Linq;
using System.Threading;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Common.Logger;
using HKSJ.WBVV.Entity.ApiModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using HKSJ.WBVV.Common.Language;
using System.Globalization;

namespace HKSJ.WBVV.Api.Base
{
    public class ApiControllerBase : ApiController
    {
        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 显示行数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// post提交的json数据
        /// </summary>
        public JObject JObject { get; set; }
        /// <summary>
        /// 查询参数
        /// </summary>
        public IList<Condtion> Condtions { get; set; }
        /// <summary>
        /// 排序参数
        /// </summary>
        public IList<OrderCondtion> OrderCondtions { get; set; }

        #region 接收GET请求的参数
        /// <summary>
        /// 接收GET请求的参数
        /// </summary>
        /// <param name="controllerContext"></param>
        private void GetParams(HttpControllerContext controllerContext)
        {
            var url = controllerContext.Request.RequestUri.ToString();
            if (url.IndexOf("?", System.StringComparison.Ordinal) != -1)
            {
                var urlArr = url.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
                var parameter = urlArr[1]; //取？后面的参数
                var parameters = parameter.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var paramter in parameters)
                {
                    //找到pagesize=1的字符串
                    if (paramter.ToLower().Contains("pagesize"))
                    {
                        int pageSize = 0;
                        int.TryParse(
                            paramter.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1],
                            out pageSize);
                        PageSize = pageSize;
                    }
                    if (paramter.ToLower().Contains("pageindex"))
                    {
                        int pageindex = 1;
                        int.TryParse(
                            paramter.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1],
                            out pageindex);
                        PageIndex = pageindex;
                    }
                }
            }
        }
        #endregion

        #region 接受POST请求的参数
        /// <summary>
        /// 接受POST请求的参数
        /// </summary>
        /// <param name="controllerContext"></param>
        private void PostParams(HttpControllerContext controllerContext)
        {
            HttpContent content = controllerContext.Request.Content;
            if (content.IsFormData())
            {
            }
            Task<JObject> task = content.ReadAsAsync<JObject>();
            if (task != null)
            {
                JObject = task.Result;
                if (JObject != null)
                {
                    IpAddress = JObject.Value<string>("IpAddress");
                    PageSize = JObject.Value<int>("pagesize");
                    PageIndex = JObject.Value<int>("pageindex");
                    var condtions = JObject.Value<JArray>("condtions");
                    if (condtions != null)
                    {
                        Condtions = condtions.ToObject<IList<Condtion>>();
                    }
                    var ordercondtions = JObject.Value<JArray>("ordercondtions");
                    if (ordercondtions != null)
                    {
                        OrderCondtions = ordercondtions.ToObject<IList<OrderCondtion>>();
                    }
                }
                else
                {
                    JObject = new JObject();
                }
            }
        }
        #endregion

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var method = controllerContext.Request.Method;
            //获取头部--语言信息
            var headers = controllerContext.Request.Headers;

            //接口未统一，无传递使用默认"中文"
            string activeLanguage = LanguageType.zh_cn;

            if (headers.Contains("ActiveLanguage"))
                activeLanguage = headers.GetValues("ActiveLanguage").First();

            //写入语言区域线程
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(activeLanguage);

            if (method != null)
            {
                if (method.ToString().ToUpper().Equals("GET"))
                {
                    GetParams(controllerContext);
                }
                else//POST提交的数据
                {
                    PostParams(controllerContext);
                }

            }
        }
        /// <summary>
        /// 增删改调用的函数
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="infoLog"></param>
        /// <returns></returns>
        protected Result CommonResult(Func<object> handler, Action<Result> infoLog = null)
        {
            var result = new Result();
            try
            {
                AssertUtil.IsNotNull(handler, LanguageUtil.Translate("api_Controller_Base_CommonResult_handler"));
                var o = handler();
                result.Data = o;
                if (infoLog != null)
                {
                    infoLog(result);
                }
            }
            catch (AssertException ex) //断言异常
            {
                result.Success = false;
                result.ExceptionMessage = ex.MostInnerException().Message;
                LogBuilder.Log4Net.Info(result.ExceptionMessage);
            }
            return result;
        }

        protected T CreateBusiness<T>() where T : class
        {
            string interfaceName = typeof(T).Name;
            return (T)Assembly.Load("HKSJ.WBVV.Business").CreateInstance("HKSJ.WBVV.Business.{0}".F(interfaceName.Substring(1, interfaceName.Length - 1)));
        }
    }
}
