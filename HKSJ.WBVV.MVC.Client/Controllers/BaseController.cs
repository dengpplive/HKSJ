using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKSJ.WBVV.Common.Language;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Web.Security;
using HKSJ.WBVV.MVC.Client.Models;
using HKSJ.WBVV.MVC.Client.GlobalVariable;
using HKSJ.WBVV.MVC.Common;
using HKSJ.Utilities;
using HKSJ.WBVV.Common.Http;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Common.Logger;
namespace HKSJ.WBVV.MVC.Client.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 处理JSON中的时间格式
        /// </summary>       
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new NewtonJsonResult()
            {
                ContentEncoding = contentEncoding,
                ContentType = contentType,
                Data = data,
                JsonRequestBehavior = behavior
            };
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.ExceptionHandled = true;
                string errorMsg = filterContext.Exception.Message;
                //filterContext.Result = Json(new { ErrorCode = filterContext.Exception.Source, ErrorMsg = errorMsg }, filterContext.Exception.Message);
                filterContext.Result = Json(new { ErrorCode = filterContext.Exception.Source, ErrorMsg = errorMsg }, filterContext.Exception.ToString());
            }
            else
            {
                filterContext.ExceptionHandled = true;
                //var errorView = View("Error", (object)filterContext.Exception.Message);
                var errorView = View("Error", (object)filterContext.Exception.ToString());
                filterContext.Result = errorView;
            }
#if !DEBUG
                   LogBuilder.Log4Net.Error(filterContext.Exception.Message, filterContext.Exception);
#else

#endif

            base.OnException(filterContext);
        }


        //------公共数据-------------------------
        protected UserModel GetUserModel()
        {
            var model = new UserModel();
            if (string.IsNullOrWhiteSpace(GlobalMemberInfo.Account) || GlobalMemberInfo.UserId <= 0) { return model; }
            var result = WebApiHelper.InvokeApi<string>("Login/UserLogin",
                new
                {
                    account = GlobalMemberInfo.Account,
                    pwd = GlobalMemberInfo.PassWord,
                    IpAddress = ClientHelper.GetIPAddress
                });
            //TODO update 刘强 处理null异常
            if (string.IsNullOrEmpty(result))
            {
                return model;
            }
            var resultView = JsonConvert.DeserializeObject(result, typeof(ResultView<UserView>)) as ResultView<UserView>;
            if (resultView == null || resultView.Data == null || !resultView.Success) return model;
            model.Id = resultView.Data.Id;
            model.Account = resultView.Data.Account;
            model.Phone = resultView.Data.Phone;
            model.Pwd = resultView.Data.Pwd;
            model.NickName = resultView.Data.NickName;
            model.BB = resultView.Data.BB;
            model.PlayCount = resultView.Data.PlayCount;
            model.Picture = GlobalMemberInfo.GetQiniuUserPicture(model.Id);
            resultView.Data.Picture = model.Picture;
            model.SubscribeNum = resultView.Data.SubscribeNum;
            model.FansCount = resultView.Data.FansCount;
            model.State = resultView.Data.State;
            model.SkinId = resultView.Data.SkinId;
            GlobalMemberInfo.SetUserCookie(resultView.Data, GlobalMemberInfo.RemberMe);
            return model;
        }

    }
    public class NewtonJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if ((this.JsonRequestBehavior == JsonRequestBehavior.DenyGet) && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(LanguageUtil.Translate("web_Controllers_Base_NewtonJsonResult_ExecuteResult_exception"));
            }
            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "text/html";
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                //设置输出的时间格式
                timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                string strData = JsonConvert.SerializeObject(this.Data, Newtonsoft.Json.Formatting.Indented, timeFormat);
                response.Write(strData);
            }
        }
    }
}
