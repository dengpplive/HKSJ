using System;
using System.Web;
using System.Web.Http;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Entity.Response;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Api.AppController.Base
{
    [ModelValidate, CheckApp]
    public class AppApiControllerBase : ApiController
    {
        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddress { get; set; }

        private ResponseExtensionData _responseExtensionData;

        /// <summary>
        /// 响应附加数据
        /// </summary>
        public virtual ResponseExtensionData ResponseExtensionData
        {
            get
            {
                return _responseExtensionData ?? (_responseExtensionData = new ResponseExtensionData
                {
                    CallResult = CallResult.Success,
                    RetMsg = LanguageUtil.Translate("api_Controller_AppApi_ResponseExtensionData_RetMsg"),
                });
            }
        }

        /// <summary>
        /// http上下文
        /// </summary>
        public HttpContextBase ContextBase
        {
            get
            {
                return Request.Properties["MS_HttpContext"] as HttpContextBase;
            }
        }

        /// <summary>
        /// 包装响应结果
        /// </summary>
        /// <param name="value"></param>
        /// <param name="extentionData"></param>
        /// <returns></returns>
        [NonAction]
        public virtual ResponsePackage<T> PackageActionResult<T>(T value, ResponseExtensionData extentionData)
        {
            if (extentionData == null)
            {
                extentionData = ResponseExtensionData;
            }
            return new ResponsePackage<T> { Data = value, ExtensionData = extentionData };
        }

        /// <summary>
        /// 结果包装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [NonAction]
        public virtual T PackageResult<T>(T value)
        {
            return value;
        }

        /// <summary>
        /// 增删改调用的函数
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="infoLog"></param>
        /// <returns></returns>
        [NonAction]
        public virtual ResponsePackage<T> CommonResult<T>(Func<T> handler, Action<ResponsePackage<T>> infoLog = null)
        {
            var result = new ResponsePackage<T>();
            var extionData = new ResponseExtensionData
            {
                ModelValidateErrors = new List<ModelValidateError>(),
                CallResult = CallResult.Success,
                RetMsg = LanguageUtil.Translate("api_Controller_AppApi_CommonResult_RetMsg")
            };
            try
            {
                AssertUtil.IsNotNull(handler, LanguageUtil.Translate("api_Controller_AppApi_CommonResult_handler"));
                var o = handler();
                result.Data = o;
                if (infoLog != null)
                {
                    infoLog(result);
                }
            }
            catch (AssertException ex)
            {
                extionData.CallResult = CallResult.BusinessError;
                extionData.RetMsg = string.Format(LanguageUtil.Translate("api_Controller_AppApi_CommonResult_AssertException"), ex.Message);
                result.Data = default(T);
            }
            catch (Exception ex)
            {
                extionData.CallResult = CallResult.BusinessError;
                extionData.RetMsg = string.Format(LanguageUtil.Translate("api_Controller_AppApi_CommonResult_Exception"), ex.Message);
                result.Data = default(T);
            }
            result.ExtensionData = extionData;
            return result;
        }


        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var request = controllerContext.Request;
            var headers = request.Headers;
            if (headers != null && headers.Contains("ipaddress"))
            {
                IpAddress = headers.GetValues("ipaddress").First();
            }
            else
            {
                IpAddress = "";
            }
        }
    }
}
