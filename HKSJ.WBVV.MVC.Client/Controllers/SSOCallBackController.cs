using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKSJ.WBVV.MVC.Client.Controllers
{
    public class SSOCallBackController : Controller
    {
        //
        // GET: /Filter/

        public ActionResult QQCallBack()
        {
            return View();
        }
    }
}
