using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;
using System.Linq;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using HKSJ.WBVV.MVC.Common;

namespace HKSJ.WBVV.MVC.Manage.Common
{
    /// <summary>
    /// 网站上下文信息
    /// </summary>
    public static class WebContext
    {
        public const string SessionKey = "WBVV_MANAGE";
        public static ManageView Manage
        {
            get
            {
                var manage = HttpContext.Current.Session[SessionKey] as ManageView;
                if (manage == null)
                {
                    var loginName = HttpContext.Current.User.Identity.Name;
                    manage = WebApiHelper.InvokeApi<ManageView>("Manage/GetManage?loginName={0}".F(loginName));
                    HttpContext.Current.Session[SessionKey] = manage;
                }
                return manage;
            }
        }

        public static string LoginName
        {
            get
            {
                return Manage.LoginName;
            }

        }
    }
}