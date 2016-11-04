using System;
using System.Web.Mvc;
using HKSJ.Cache;
using HKSJ.Utilities;
using HKSJ.WBVV.Common.Http;
using HKSJ.WBVV.Common.Language;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Client.GlobalVariable;
using HKSJ.WBVV.MVC.Common;
using HKSJ.WBVV.MVC.Common.Code;
using Newtonsoft.Json;

namespace HKSJ.WBVV.MVC.Client.Controllers
{
    /// <summary>
    /// 用户登录控制器
    /// Author : AxOne
    /// </summary>
    public class LoginController : BaseController
    {
        /// <summary>
        /// 获取验证码
        /// Author : AxOne
        /// </summary>
        /// <returns></returns>
        public ActionResult GetValidateCode()
        {
            var vCode = new ValidateCode();
            var code = vCode.CreateValidateCode(4);
            CacheHelper.Insert(GlobalMemberInfo.Account + "_Code", code, null, DateTime.Now.AddSeconds(3600));
            var bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        /// <summary>
        /// 校验验证码
        /// Author : AxOne
        /// </summary>
        /// <param name="account"></param>
        /// <param name="vcode"></param>
        /// <returns></returns>
        public JsonResult CheckValidateCode(string vcode)
        {
            var code = CacheHelper.Get(GlobalMemberInfo.Account + "_Code") ?? "";
            var result = string.Equals(code, vcode);
            return Json(new { Success = result }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 检测登录(以弃用)
        /// Author : AxOne
        /// </summary>
        /// <returns></returns>
        public JsonResult CheckLogin()
        {
            if (string.IsNullOrWhiteSpace(GlobalMemberInfo.Account) || GlobalMemberInfo.UserId < 1)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            var data = new
            {
                Id = GlobalMemberInfo.UserId,
                GlobalMemberInfo.Account,
                Pwd = GlobalMemberInfo.PassWord,
                GlobalMemberInfo.NickName,
                GlobalMemberInfo.BB
            };
            return Json(new { Success = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 登录
        /// Author : AxOne
        /// </summary>
        /// <param name="account">用户名/手机号</param>
        /// <param name="pwd">密码</param>
        /// <param name="remb">是否记住(0:不记住1:记住)</param>
        /// <param name="type">账户类型(0:手机1:邮箱)</param>
        /// <returns></returns>
        public JsonResult UserLogin(string account, string pwd, int remb = 0)
        {
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(pwd))
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            var result = LoginApi(account, pwd);
            if (result == null || result.Data == null || !result.Success || result.Data.Id <= 0)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            result.Data.Picture = GlobalMemberInfo.GetQiniuUserPicture(result.Data.Id);
            GlobalMemberInfo.SetUserCookie(result.Data, remb);
            return Json(new { Success = true, result.Data, remb });
        }

        /// <summary>
        /// 登录SSO
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public JsonResult UserLoginById(int userId)
        {
            if (userId <= 0)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            var result = LoginApi(userId);
            if (result == null || result.Data == null || !result.Success || result.Data.Id <= 0)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            result.Data.Picture = GlobalMemberInfo.GetQiniuUserPicture(result.Data.Id);
            GlobalMemberInfo.SetUserCookie(result.Data);
            return Json(new { Success = true, result.Data });
        }



        /// <summary>
        /// 找回密码
        /// Author : AxOne
        /// </summary>
        /// <param name="account">用户名/手机号</param>
        /// <param name="pwd">新密码</param>
        /// <returns></returns>
        public JsonResult UpdatePwdByPhone(string account, string pwd)
        {
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(pwd))
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            var result = UpdatePwdApi(account, pwd);
            if (result == null || result.Data == null || !result.Success || result.Data.Id <= 0)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            result.Data.Picture = GlobalMemberInfo.GetQiniuUserPicture(result.Data.Id);
            GlobalMemberInfo.SetUserCookie(result.Data, GlobalMemberInfo.RemberMe);
            return Json(new { Success = true, result.Data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 注销登录
        /// Author : AxOne
        /// </summary>
        /// <returns></returns>
        public JsonResult LogOut()
        {
            CookieHelper.DelCookie("ck5bvv");
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证已登录用户密码
        /// Author : AxOne
        /// </summary>
        /// <param name="pwd">旧密码</param>
        /// <returns></returns>
        public JsonResult LoginUserPwdValid(string pwd)
        {
            if (string.IsNullOrWhiteSpace(pwd))
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            var result = PwdValidApi(pwd);
            if (result == null || !result.Success || !result.Data)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证已登录用户手机号码
        /// Author : AxOne
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        public JsonResult LoginUserPhoneValid(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            var result = PhoneValid(phone);
            if (result == null || !result.Success || !result.Data)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        #region PrivateMethod  Author:AxOne
        private static ResultView<UserView> LoginApi(string account, string pwd)
        {
            pwd = Md5Helper.MD5(pwd, 32); //TODO 用户密码 MD5加密
            var result = WebApiHelper.InvokeApi<string>("Login/UserLogin", new { account, pwd, IpAddress = ClientHelper.GetIPAddress });
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<UserView>)) as ResultView<UserView>;
            }
            return new ResultView<UserView>();
        }
        /// <summary>
        /// SSO,Author:xuzhoujie
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static ResultView<UserView> LoginApi(int userId)
        {
            var result = WebApiHelper.InvokeApi<string>("Login/UserLoginById", new { userId, IpAddress = ClientHelper.GetIPAddress });
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<UserView>)) as ResultView<UserView>;
            }
            return new ResultView<UserView>();
        }

        private static ResultView<UserView> UpdatePwdApi(string account, string pwd)
        {
            pwd = Md5Helper.MD5(pwd, 32); //TODO 用户密码 MD5加密
            var result = WebApiHelper.InvokeApi<string>("User/UpdatePwdByPhone", new { account, pwd });
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<UserView>)) as ResultView<UserView>;
            }
            return new ResultView<UserView>();
        }

        private static ResultView<bool> PwdValidApi(string pwd)
        {
            pwd = Md5Helper.MD5(pwd, 32); //TODO 用户密码 MD5加密
            var result = WebApiHelper.InvokeApi<string>("Login/CheckPwd", new { uid = GlobalMemberInfo.UserId, opwd = pwd });
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<bool>)) as ResultView<bool>;
            }
            return new ResultView<bool>();
        }

        private static ResultView<bool> PhoneValid(string phone)
        {
            var result = WebApiHelper.InvokeApi<string>("Login/CheckPhone", new { uid = GlobalMemberInfo.UserId, ophone = phone });
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<bool>)) as ResultView<bool>;
            }
            return new ResultView<bool>();
        }

        #endregion


        #region 第三方绑定、登录处理
        /// <summary>
        /// 绑定已有帐号并登录API请求
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="pwd">密码</param>
        /// <param name="typeCode">第三方类型</param>
        /// <param name="relatedId">第三方唯一标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="figureURL">头像路径</param>
        /// <returns></returns>
        private static ResultView<UserView> ThirdPartyBindAndLoginAPI(string account, string pwd, string typeCode, string relatedId, string nickName, string figureURL)
        {
            pwd = Md5Helper.MD5(pwd, 32); //TODO 用户密码 MD5加密
            var result = WebApiHelper.InvokeApi<string>("Login/ThirdPartyBindAndLogin", new { account, pwd, typeCode, relatedId, nickName, figureURL, IpAddress = ClientHelper.GetIPAddress });
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<UserView>)) as ResultView<UserView>;
            }
            return new ResultView<UserView>();
        }

        /// <summary>
        /// 绑定已有帐号并登录
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="pwd">密码</param>
        /// <param name="typeCode">第三方类型</param>
        /// <param name="relatedId">第三方唯一标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="figureURL">头像路径</param>
        /// <returns></returns>
        public JsonResult ThirdPartyBindAndLogin(string account, string pwd, string typeCode, string relatedId, string nickName, string figureURL)
        {
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(pwd) || string.IsNullOrWhiteSpace(typeCode) || string.IsNullOrWhiteSpace(relatedId))
            {

                return Json(new { Success = false, Message = LanguageUtil.Translate("web_Controllers_Login_ThirdPartyBindAndLogin_messageone") }, JsonRequestBehavior.AllowGet);
            }
            var result = ThirdPartyBindAndLoginAPI(account, pwd, typeCode, relatedId, nickName, figureURL);
            if (result == null || result.Data == null || !result.Success || result.Data.Id <= 0)
            {
                return Json(new { Success = false, Message = LanguageUtil.Translate("web_Controllers_Login_ThirdPartyBindAndLogin_messagetwo") }, JsonRequestBehavior.AllowGet);
            }
            result.Data.Picture = GlobalMemberInfo.GetQiniuUserPicture(result.Data.Id);

            GlobalMemberInfo.SetUserCookie(result.Data);
            return Json(new { Success = true, result.Data });
        }

        #endregion
    }

}
