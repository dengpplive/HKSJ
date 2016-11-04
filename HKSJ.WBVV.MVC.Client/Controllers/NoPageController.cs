
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.MVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Newtonsoft.Json.Linq;
using HKSJ.Utilities;
using HKSJ.WBVV.Entity.ApiModel;

namespace HKSJ.WBVV.MVC.Client.Controllers
{
    public class NoPageController : BaseController
    {
        // GET: UserVip   
        public ActionResult Index()
        {
            return View();
        }
    }
}