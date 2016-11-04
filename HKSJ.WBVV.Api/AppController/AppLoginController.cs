using System;
using System.Web.Http;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.Response;
using HKSJ.WBVV.Entity.RequestPara;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Api.AppController.Base;
using HKSJ.WBVV.Entity.ViewModel.Client;

namespace HKSJ.WBVV.Api
{
    /// <summary>
    /// app×¢²áµÇÂ¼½Ó¿Ú
    /// Author : AxOne
    /// </summary>
    [RoutePrefix("api/app")]
    public class AppLoginController : AppApiControllerBase
    {
        private readonly IUserBusiness _userBusiness;

        /// <summary>
        /// AppLoginController
        /// </summary>
        /// <param name="iuserBusiness"></param>
        public AppLoginController(IUserBusiness iuserBusiness)
        {
            _userBusiness = iuserBusiness;
        }

        #region ×¢²á,µÇÂ¼,ÕÒ»ØÃÜÂë
        /// <summary>
        /// Ê×Ò³--×¢²á
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost, Route("regist")]
        public ResponsePackage<UserView> UserRegist([FromBody]RegisterPara para)
        {
            _userBusiness.IpAddress = IpAddress;
            var accountType = (int)para.AccountType;
            return CommonResult(() => _userBusiness.RegisterUser(para.Account, para.Pwd, accountType), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// Ê×Ò³--µÇÂ¼
        /// </summary>
        /// <param name="para"></param>
        [HttpPost, Route("login")]
        public ResponsePackage<UserView> UserLogin([FromBody]LoginPara para)
        {
            _userBusiness.IpAddress = IpAddress;
            return CommonResult(() => _userBusiness.LoginUser(para.Account, para.Pwd), r => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// Ê×Ò³--ÕÒ»ØÃÜÂë
        /// </summary>
        /// <param name="para"></param>
        [HttpPost, Route("resetpwd")]
        public ResponsePackage<bool> ResetPwd([FromBody]LoginPara para)
        {
            _userBusiness.IpAddress = IpAddress;
            return CommonResult(() => ResetPwd(para.Account, para.Pwd), r => Console.WriteLine(r.ToJSON()));
        } 
        #endregion

        #region private method

        private bool ResetPwd(string account, string pwd)
        {
            var viewModel = _userBusiness.UpdatePwdByPhone(account, pwd);
            return viewModel != null && viewModel.Id > 0;
        }

        #endregion

    }
}