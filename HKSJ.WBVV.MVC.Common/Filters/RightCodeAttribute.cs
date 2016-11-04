using HKSJ.WBVV.Common.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ActionFilterAttribute = System.Web.Mvc.ActionFilterAttribute;

namespace HKSJ.WBVV.MVC.Common.Filters
{
    /// <summary>
    /// 权限验证过滤器
    /// </summary>
    public class RightCodeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);


            var isRight = new List<string>();

            //存在权限
            if (!isRight.Any())
            {
                // 判断是Ajax请求
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult()
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new
                        {
                            Success = false,
                            ExceptionMessage = LanguageUtil.Translate("com_Filters_RightCodeAttribute_OnActionExecuting_ExceptionMessage"),
                            JSON = ""
                        }
                    };
                }
                else
                {
                    //没有权限返回 NoRight错误页面
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "NotAuth"
                    };
                }
            }
        }
    }
}