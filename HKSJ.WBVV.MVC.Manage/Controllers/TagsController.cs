using System;
using System.Collections.Generic;
using System.Linq;
using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.MVC.Common;
using System.Web.Mvc;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Manage.Common.Base;

namespace HKSJ.WBVV.MVC.Manage.Controllers
{
    public class TagsController : BaseController
    {
        // GET: Tags
        public ActionResult Index()
        {
            var api = WebConfig.BaseAddress;
            var model = new { api };
            return View(model);
        }
        
    }
}