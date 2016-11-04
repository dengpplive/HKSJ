using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using HKSJ.WBVV.Api.AppController.Base;
using HKSJ.WBVV.Entity.Response;

namespace HKSJ.WBVV.Api.Extend
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpResponseMessage CreateResponse<T>(this HttpRequestMessage request, T value, HttpControllerContext controllerContext, HttpStatusCode statusCode = HttpStatusCode.OK, ResponseExtensionData responseExtensionData = null)
        {
            var baseController = controllerContext.Controller as AppApiControllerBase;
            if (baseController == null)
            {
                return request.CreateResponse(statusCode, value, controllerContext.Configuration);
            }
            if (responseExtensionData == null && typeof(T).Name.Contains("ResponsePackage"))
            {
                return request.CreateResponse(statusCode, baseController.PackageResult(value), controllerContext.Configuration);
            }
            return request.CreateResponse(statusCode, baseController.PackageActionResult(value, responseExtensionData), controllerContext.Configuration);
        }

        /// <summary>
        /// 根据Action的返回类型创建对应的错误响应信息
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="actionContext">actionContext</param>
        /// <param name="extensionData">extensionData</param>
        /// <param name="statusCode">statusCode</param>
        /// <returns></returns>
        public static HttpResponseMessage CreateErrorResponseByReturnType(this HttpRequestMessage request
            , HttpActionContext actionContext, ResponseExtensionData extensionData, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (actionContext.ControllerContext.Controller is AppApiControllerBase)
            {
                return CreateResponse(request, (object)null, actionContext.ControllerContext, statusCode, extensionData);
            }
            var returnType = actionContext.ActionDescriptor.ReturnType;
            if (returnType.IsValueType || returnType == typeof(string))
            {
                return request.CreateResponse(statusCode, (object)null);
            }
            if (!returnType.IsGenericType) return request.CreateResponse(statusCode, (object)null);
            var genericType = returnType.GetGenericTypeDefinition();
            if (genericType == typeof(ResponsePackage<>))
            {
                return request.CreateResponse(statusCode
                    , new ResponsePackage<object> { Data = null, ExtensionData = extensionData });
            }
            return request.CreateResponse(statusCode, (object)null);
        }

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="request">request</param>
        /// <returns>服务实例</returns>
        public static TService GetService<TService>(this HttpRequestMessage request)
        {
            System.Web.Http.Dependencies.IDependencyScope dependencyScope = request.GetDependencyScope();
            TService service = (TService)dependencyScope.GetService(typeof(TService));
            return service;
        }

    }
}
