using System.Web.Mvc;
using HKSJ.Utilities;
using HKSJ.WBVV.Common.Language;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Client.GlobalVariable;
using HKSJ.WBVV.MVC.Common;
using Newtonsoft.Json;

namespace HKSJ.WBVV.MVC.Client.Controllers
{
    /// <summary>
    /// 用户注册控制器
    /// Author : AxOne
    /// </summary>
    public class RegistController : BaseController
    {
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="raccount">帐号/手机号码</param>
        /// <param name="rpwd">密码</param>
        /// <param name="type">账户类型(0:手机1:邮箱)</param>
        /// <returns></returns>
        public JsonResult UserRegist(string raccount, string rpwd, int type = 0)
        {
            if (string.IsNullOrWhiteSpace(raccount) || string.IsNullOrWhiteSpace(rpwd))
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            var result = RegistApi(raccount, rpwd, type);
            if (result == null || result.Data == null || result.Data.Id <= 0 || !result.Success)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            result.Data.Picture = GlobalMemberInfo.GetQiniuUserPicture(result.Data.Id);
            GlobalMemberInfo.SetUserCookie(result.Data, 1);
            return Json(new { Success = true, result.Data });
        }

        #region PrivateMethod  Author:AxOne
        private static ResultView<UserView> RegistApi(string account, string pwd, int type)
        {
            var result = WebApiHelper.InvokeApi<string>("Register/UserRegist", new { account, pwd, type });
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<UserView>)) as ResultView<UserView>;
            }
            return new ResultView<UserView>();
        }
        #endregion

        #region 第三方注册并绑定
        /// <summary>
        /// 第三方注册并绑定
        /// </summary>
        /// <param name="tpaccount">账户</param>
        /// <param name="tppwd">密码</param>
        /// <param name="tptypeCode">第三方类型编码</param>
        /// <param name="tprelatedId">第三方唯一身份标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="figureURL">头像路径</param>
        /// <param name="type">手机、邮箱类型</param>
        /// <returns></returns>
        public JsonResult ThirdPartyBindAndRegister(string tpaccount, string tppwd, string tptypeCode, string tprelatedId, string tpnickName, string tpfigureURL, int type = 0)
        {
            if (string.IsNullOrWhiteSpace(tpaccount) || string.IsNullOrWhiteSpace(tppwd) || string.IsNullOrWhiteSpace(tptypeCode) || string.IsNullOrWhiteSpace(tprelatedId))
            {
                return Json(new { Success = false, Message = LanguageUtil.Translate("web_Controllers_Regist_ThirdPartyBindAndRegister_messageone") }, JsonRequestBehavior.AllowGet);
            }
            var result = ThirdPartyBindAndRegisterAPI(tpaccount, tppwd, tptypeCode, tprelatedId, tpnickName, tpfigureURL, type);
            if (result == null || result.Data == null || result.Data.Id <= 0 || !result.Success)
            {
                return Json(new { Success = false, Message =LanguageUtil.Translate("web_Controllers_Regist_ThirdPartyBindAndRegister_messagetwo") }, JsonRequestBehavior.AllowGet);
            }
            result.Data.Picture = GlobalMemberInfo.GetQiniuUserPicture(result.Data.Id);
            GlobalMemberInfo.SetUserCookie(result.Data);
            return Json(new { Success = true, result.Data });
        }

        private static ResultView<UserView> ThirdPartyBindAndRegisterAPI(string account, string pwd, string typeCode, string relatedId, string nickName, string figureURL, int type)
        {
            var result = WebApiHelper.InvokeApi<string>("Register/ThirdPartyBindAndRegister", new { account, pwd, typeCode, relatedId, nickName, figureURL, type });
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<UserView>)) as ResultView<UserView>;
            }
            return new ResultView<UserView>();
        }

        #endregion


        #region 第三方自动注册并绑定（跳过）
        /// <summary>
        /// 第三方自动注册并绑定
        /// </summary>
        /// <param name="typeCode">第三方类型编码</param>
        /// <param name="relatedId">第三方唯一身份标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="figureURL">头像路径</param>
        /// <returns></returns>
        public JsonResult AutoRegisterAndBindThirdParty(string typeCode, string relatedId, string nickName, string figureURL)
        {
            if (string.IsNullOrWhiteSpace(typeCode) || string.IsNullOrWhiteSpace(relatedId))
            {
                return Json(new { Success = false, Message = LanguageUtil.Translate("web_Controllers_Regist_AutoRegisterAndBindThirdParty_messageone") }, JsonRequestBehavior.AllowGet);
            }
            var result = AutoRegisterAndBindThirdPartyAPI(typeCode, relatedId, nickName, figureURL);
            if (result == null || result.Data == null || result.Data.Id <= 0 || !result.Success)
            {
                return Json(new { Success = false, Message = LanguageUtil.Translate("web_Controllers_Regist_AutoRegisterAndBindThirdParty_messagetwo") }, JsonRequestBehavior.AllowGet);
            }
            result.Data.Picture = GlobalMemberInfo.GetQiniuUserPicture(result.Data.Id);
            GlobalMemberInfo.SetUserCookie(result.Data);
            return Json(new { Success = true, result.Data });
        }

        private static ResultView<UserView> AutoRegisterAndBindThirdPartyAPI(string typeCode, string relatedId, string nickName, string figureURL)
        {
            var result = WebApiHelper.InvokeApi<string>("Register/AutoRegisterAndBindThirdParty", new { typeCode, relatedId, nickName, figureURL });
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<UserView>)) as ResultView<UserView>;
            }
            return new ResultView<UserView>();
        }

        #endregion
    }
}
