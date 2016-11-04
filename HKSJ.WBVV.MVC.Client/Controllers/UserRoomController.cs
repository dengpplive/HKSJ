
using HKSJ.WBVV.Common.Config;
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
using HKSJ.WBVV.MVC.Client.Attribute;
using HKSJ.WBVV.MVC.Client.GlobalVariable;
using HKSJ.WBVV.Entity.ViewModel.Client;

namespace HKSJ.WBVV.MVC.Client.Controllers
{
    public class UserRoomController : BaseController
    {
        // GET: UserRoom   

        //[Member]
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult _UserHeader()
        {
            var model = GetUserModel();
            //个人空间的模型
            var url = WebConfig.BaseAddress + "UserSkin/GetUserSkinList";
            var result = WebApiHelper.InvokeApi<string>(url).JsonToEntity<Result>();
            if (result.Success)
            {
                ViewBag.skins = result.Data;
            }
            return PartialView(model);
        }

        public JsonResult GetUserRoomInfo(int browserUserId, int loginUserId)
        {
            var url = WebConfig.BaseAddress + "User/GetUserRoomInfo?browserUserId=" + browserUserId + "&loginUserId=" + loginUserId;
            string strData = WebApiHelper.InvokeApi<string>(url);
            var result = (!string.IsNullOrEmpty(strData) && !strData.StartsWith("null")) ? strData.JsonToEntity<Result>() : null;
            if (result != null && result.Data != null)
            {
                dynamic userView = (dynamic)result.Data;
                userView.Picture = GlobalMemberInfo.GetQiniuUserPicture((int)userView.Id);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // [Member]
        public ActionResult Video()
        {
            return View();
        }
        // [Member]
        public ActionResult Message()
        {
            return View();
        }
        // [Member]
        public ActionResult Fans()
        {
            return View();
        }
        // [Member]
        public ActionResult Album()
        {
            return View();
        }
        public ActionResult SonAlbum()
        {
            return View();
        }

        // [Member]
        public ActionResult RoomManage()
        {

            return View();
        }
        /// <summary>
        /// 获取当前的登录用户id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCurrUserId()
        {
            return Json(new { Success = GlobalMemberInfo.UserId > 0 ? true : false, UserId = GlobalMemberInfo.UserId }, JsonRequestBehavior.AllowGet);
        }

    }
}