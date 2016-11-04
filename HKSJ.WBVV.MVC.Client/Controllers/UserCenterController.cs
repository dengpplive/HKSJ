using System.Web.Mvc;
using HKSJ.Utilities;
using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Client.Attribute;
using HKSJ.WBVV.MVC.Client.GlobalVariable;
using HKSJ.WBVV.MVC.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HKSJ.WBVV.MVC.Client.Controllers
{
    [Member]
    public class UserCenterController : BaseController
    {
        /// <summary>
        /// 用户收藏
        /// Author : AxOne
        /// </summary>
        /// <returns></returns>
        public ActionResult UserCollect()
        {
            ViewBag.Uid = GlobalMemberInfo.UserId;
            return View();
        }

        /// <summary>
        /// 账户中心
        /// Author : AxOne
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountSet()
        {
            ViewBag.Uid = GlobalMemberInfo.UserId;
            ViewBag.Phone = GlobalMemberInfo.Phone;
            ViewBag.SrcUri = GlobalMemberInfo.Picture;
            ViewBag.UserPictures = XMLHelper.GetAttributesValue("Configs/UserPictures.xml", "path");
            var model = new AccountView();
            if (GlobalMemberInfo.UserId <= 0)
            {
                return View(model);
            }
            var result = AccountApi();
            if (result == null || result.Data == null || !result.Success)
            {
                return View(model);
            }
            model = result.Data;
            return View(model);
        }

        /// <summary>
        ///保存账户资料
        /// Author : AxOne
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult SaveAccount(AccountView para)
        {
            var result = AccountSetApi(para);
            if (result == null || result.Data == null || !result.Success)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            result.Data.Picture = GlobalMemberInfo.Picture;
            GlobalMemberInfo.SetUserCookie(result.Data, GlobalMemberInfo.RemberMe);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存用户选择的系统预定义头像
        /// Author : AxOne
        /// </summary>
        /// <param name="key">qiniu key/系统图片路径</param>
        /// <returns></returns>
        public JsonResult UpdateUserPic(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) { return Json(new { Success = false }, JsonRequestBehavior.AllowGet); }
            var result = UpdateUserPicApi(key);
            if (result == null || result.Data == null || !result.Success)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            result.Data.Picture = GlobalMemberInfo.GetQiniuUserPicture(result.Data.Id);
            GlobalMemberInfo.SetUserCookie(result.Data, GlobalMemberInfo.RemberMe);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查昵称是否唯一
        /// Author : AxOne
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult CheckNickName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { return Json(new { Success = false }, JsonRequestBehavior.AllowGet); }
            var result = CheckNickNameApi(name);
            if (result == null || !result.Data || !result.Success)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 重置密码.
        /// Author : AxOne
        /// </summary>
        /// <param name="pwd">新密码</param>
        /// <returns></returns>
        public JsonResult SetPassword(string pwd)
        {
            if (string.IsNullOrWhiteSpace(pwd)) { return Json(new { Success = false }, JsonRequestBehavior.AllowGet); }
            var result = SetPasswordApi(pwd);
            if (result == null || result.Data == null || !result.Success)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            result.Data.Picture = GlobalMemberInfo.Picture;
            GlobalMemberInfo.SetUserCookie(result.Data, GlobalMemberInfo.RemberMe);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 重置手机号
        /// Author : AxOne
        /// </summary>
        /// <param name="phone">新手机号</param>
        /// <returns></returns>
        public JsonResult SetPhone(string phone, string pwd = "")
        {
            if (string.IsNullOrWhiteSpace(phone)) { return Json(new { Success = false }, JsonRequestBehavior.AllowGet); }
            var result = SetPhoneApi(phone, pwd);
            if (result == null || result.Data == null || !result.Success)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            result.Data.Picture = GlobalMemberInfo.Picture;
            GlobalMemberInfo.SetUserCookie(result.Data, GlobalMemberInfo.RemberMe);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 重置邮箱
        /// Author : AxOne
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        public JsonResult SetEmail(string email,string pwd="")
        {
            if (string.IsNullOrWhiteSpace(email)) { return Json(new { Success = false }, JsonRequestBehavior.AllowGet); }
            var result = SetEmailApi(email, pwd);
            if (result == null || result.Data == null || !result.Success)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            result.Data.Picture = GlobalMemberInfo.Picture;
            GlobalMemberInfo.SetUserCookie(result.Data, GlobalMemberInfo.RemberMe);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户头像
        /// Author : AxOne
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserPicUrl()
        {
            return Json(new { picUrl = GlobalMemberInfo.Picture }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUploadToken(string imgPara)
        {
            string url = string.Format(WebConfig.BaseAddress + "/QiniuUpload/GetToken?type=pic&imgPara={0}&uid={1}",
                imgPara, GlobalMemberInfo.UserId);
            var token = WebApiHelper.InvokeApi<JObject>(url).Value<string>("token");

            return Json(new
            {
                Success = !string.IsNullOrEmpty(token),
                token

            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserHistory()
        {
            var uid = GlobalMemberInfo.UserId;
            return View(uid);
        }


        public ActionResult UserVideo(int page=1,string mysearchKey="")
        {
            var uid = GlobalMemberInfo.UserId;
            ViewBag.page = page;
            ViewBag.searchKey = mysearchKey;
            return View(uid);
        }
        /// <summary>
        /// 删除视频
        /// </summary>
        /// <param name="vid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAVideo(int vid)
        {
            var result = WebApiHelper.InvokeApi<Result>("Video/DeleteAVideo", new { vid });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UserAlbums()
        {
            return View();
        }

        public ActionResult AlbumVideos(int albumId)
        {
            return View();
        }

        public ActionResult EditAlbum(int albumId)
        {
            return View();
        }

        public ActionResult CreateAlbum()
        {
            return View("EditAlbum");
        }

        public ActionResult MyFans()
        {
            return View();
        }

        public ActionResult Messager(int? t)
        {
            return View(t);
        }

        #region PrivateMethod  Author:AxOne
        private static ResultView<AccountView> AccountApi()
        {
            var result = WebApiHelper.InvokeApi<string>("User/GetAccountInfo?uid=" + GlobalMemberInfo.UserId);
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<AccountView>)) as ResultView<AccountView>;
            }
            return new ResultView<AccountView>();
        }
        private static ResultView<UserView> UpdateUserPicApi(string key)
        {
            var result = WebApiHelper.InvokeApi<string>(string.Format("User/UpdateUserPic?uid={0}&type=2", GlobalMemberInfo.UserId), new { uid = GlobalMemberInfo.UserId, key }, GlobalMemberInfo.GetToken);
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<UserView>)) as ResultView<UserView>;
            }
            return new ResultView<UserView>();
        }
        private static ResultView<UserView> SetPasswordApi(string pwd)
        {
            pwd = Md5Helper.MD5(pwd, 32); //TODO 用户密码 MD5加密
            var result = WebApiHelper.InvokeApi<string>(string.Format("User/SetPassword?uid={0}&type=2", GlobalMemberInfo.UserId), new { uid = GlobalMemberInfo.UserId, pwd }, GlobalMemberInfo.GetToken);
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<UserView>)) as ResultView<UserView>;
            }
            return new ResultView<UserView>();
        }
        private static ResultView<UserView> SetPhoneApi(string phone, string pwd)
        {
            var result = WebApiHelper.InvokeApi<string>(string.Format("User/SetPhone?uid={0}&type=2", GlobalMemberInfo.UserId), new { uid = GlobalMemberInfo.UserId, phone, pwd }, GlobalMemberInfo.GetToken);
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<UserView>)) as ResultView<UserView>;
            }
            return new ResultView<UserView>();
        }
        private static ResultView<UserView> SetEmailApi(string email, string pwd)
        {
            var result = WebApiHelper.InvokeApi<string>(string.Format("User/SetEmail?uid={0}&type=2", GlobalMemberInfo.UserId), new { uid = GlobalMemberInfo.UserId, email,pwd }, GlobalMemberInfo.GetToken);
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<UserView>)) as ResultView<UserView>;
            }
            return new ResultView<UserView>();
        }
        private static ResultView<bool> CheckNickNameApi(string name)
        {
            var result = WebApiHelper.InvokeApi<string>(string.Format("Login/CheckNickName?uid={0}&type=2", GlobalMemberInfo.UserId), new { uid = GlobalMemberInfo.UserId, nickName = name }, GlobalMemberInfo.GetToken);
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<bool>)) as ResultView<bool>;
            }
            return new ResultView<bool>();
        }
        private static ResultView<UserView> AccountSetApi(AccountView para)
        {
            var result = WebApiHelper.InvokeApi<string>(string.Format("User/AccountSet?uid={0}&type=2", GlobalMemberInfo.UserId), new { para = para.ToJSON() }, GlobalMemberInfo.GetToken);
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<UserView>)) as ResultView<UserView>;
            }
            return new ResultView<UserView>();
        }
        #endregion


        //------------------------------------------------------------------------
        /// <summary>
        /// 获取系统消息
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSystemMessages(int pageindex, int pagesize)
        {
            var result = WebApiHelper.InvokeApi<MessageView>("Message/GetSystemMessages", new { pageindex, pagesize, loginUserId = GlobalMemberInfo.UserId });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户评论信息
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="commmentPageSize"></param>
        /// <param name="commmentSize"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetComments(int pageindex, int pagesize, int commmentPageSize, int commmentSize)
        {
            var result = WebApiHelper.InvokeApi<MessageView>("Message/GetComments", new { pageindex, pagesize, commmentPageSize = commmentPageSize, commmentSize = commmentSize, loginUserId = GlobalMemberInfo.UserId });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户留言信息
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="messagePageSize"></param>
        /// <param name="commmentSize"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserMessages(int pageindex, int pagesize, int messagePageSize, int commmentSize)
        {
            var result = WebApiHelper.InvokeApi<MessageView>("Message/GetUserMessages", new { pageindex, pagesize, messagePageSize = messagePageSize, commmentSize = commmentSize, loginUserId = GlobalMemberInfo.UserId });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取页面头部消息状态信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetHeaderMessage()
        {
            var result = WebApiHelper.InvokeApi<MessageType>("Message/GetHeaderMessage", new { loginUserId = GlobalMemberInfo.UserId });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除系统消息
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteSystemMessage(int messageId)
        {
            var result = WebApiHelper.InvokeApi<Result>("Message/DeleteSystemMessage", new { messageId = messageId, loginUserId = GlobalMemberInfo.UserId });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除用户评论
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteComment(int messageId)
        {
            var result = WebApiHelper.InvokeApi<Result>("Message/DeleteComment", new { messageId = messageId, loginUserId = GlobalMemberInfo.UserId });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除用户留言数据
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteUserMessageById(int messageId)
        {
            var result = WebApiHelper.InvokeApi<Result>("Message/DeleteUserMessage", new { messageId = messageId, loginUserId = GlobalMemberInfo.UserId });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserVideoEdit(int vId,int page,string mysearchKey="")
        {
            ViewBag.vId = vId;
            ViewBag.page = page;
            ViewBag.searchKey = mysearchKey;
            return View();
        }
    }
}
