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
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Common.Http;
using HKSJ.WBVV.MVC.Client.GlobalVariable;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Client.Models;
using Newtonsoft.Json;
using HKSJ.WBVV.Common.Extender.LinqExtender;

namespace HKSJ.WBVV.MVC.Client.Controllers
{
    public class SSOController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }


    }
}