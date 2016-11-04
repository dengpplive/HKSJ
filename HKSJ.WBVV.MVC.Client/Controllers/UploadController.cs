using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Http;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.MVC.Client.Attribute;
using HKSJ.WBVV.MVC.Client.GlobalVariable;
using HKSJ.WBVV.MVC.Common;
using Newtonsoft.Json.Linq;

namespace HKSJ.WBVV.MVC.Client.Controllers
{
    public class UploadController : BaseController
    {
        //
        // GET: /Upload/
        //VideoBusiness

        [Member]
        public  ActionResult Index()
        {
            string url = WebConfig.BaseAddress + "QiniuUpload/GetToken?type=video&uid=" + GlobalMemberInfo.UserId;
            var token = WebApiHelper.InvokeApi<JObject>(url).Value<string>("token");
            ViewBag.VideoToken = token;
            return View();
        }


        [Member]
        public ActionResult CompletedLoad()
        {
            return View();
        }

        //load2
        public PartialViewResult load2(string fileId,int percent)
        {
            ViewBag.fileId = fileId;
            ViewBag.percent = percent + "%";
            return PartialView("load2");
        }

        public PartialViewResult UploadImage(string fileId)
        {
            ViewBag.fileId = fileId;
            ViewBag.Uid = GlobalMemberInfo.UserId;
            return PartialView("UploadImage");
        }

        [HttpGet]
        public JsonResult GetPersistentState(string id)
        {
            //http://api.qiniu.com/status/get/prefop?id=z0.564434957823de48a801b990
            string url = "http://api.qiniu.com/status/get/prefop?id="+id;
            var token = WebApiHelper.InvokeApi<JObject>(url);
            return Json(token, JsonRequestBehavior.AllowGet);
        }
    }
}
