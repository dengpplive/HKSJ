using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using HKSJ.WBVV.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using HKSJ.WBVV.Common.Extender;
using HKSJ.Utilities;
using HKSJ.Utilities.Base.Security;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{
    public class ManageBusiness : BaseBusiness, IManageBusiness
    {
        private readonly IManageRepository _manageRepository;
        private IAuthKeysBusiness _authKeysBusiness;
        private readonly IAuthKeysRepository _authKeysRepository;
        public ManageBusiness(IManageRepository manageRepository, IAuthKeysBusiness authKeysBusiness, IAuthKeysRepository authKeysRepository)
        {
            this._manageRepository = manageRepository;
            _authKeysBusiness = authKeysBusiness;
            _authKeysRepository = authKeysRepository;
        }

        #region manage

        #region 登录
        /// <summary>
        /// 管理员登陆
        /// </summary>
        /// <param name="loginName">登陆用户名</param>
        /// <param name="password">登陆密码</param>
        /// <returns></returns>
        public ManageView GetManageView(string loginName, string password)
        {
            CheckLoginName(loginName);
            CheckPassword(password);
            Manage manage;
            CheckLoginName(loginName, out manage);
            CheckPassword(password, manage.Password);
            var token = "";
            if (manage.Id > 0)
            {
                _authKeysBusiness = ((IContainer)HttpRuntime.Cache["containerKey"]).Resolve<IAuthKeysBusiness>();
                token = _authKeysRepository.CreatePublicKey(manage.Id, AuthUserType.General);
            }
            return new ManageView
            {
                Token = token,
                Email = manage.Email,
                Id = manage.Id,
                LoginName = manage.LoginName,
                NickName = manage.NickName
            };
        }
        #endregion

        #region 注册


        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPwd">旧密码</param>
        /// <param name="newPwd">新密码</param>
        /// <param name="confirmPwd">确认新密码</param>
        /// <returns></returns>
        public bool ChangePwd(string oldPwd, string newPwd, string confirmPwd)
        {
            CheckId();
            CheckOldPassword(oldPwd);
            CheckNewPassword(newPwd);
            Manage manage;
            CheckId(out manage);
            CheckOldPassword(oldPwd.Trim(), manage.Password);
            CheckNewPassword(manage.Password, newPwd.Trim());
            CheckConfirmPassword(newPwd.Trim(), confirmPwd.Trim());
            manage.Password = Md5Helper.MD5(newPwd.Trim(), 32);
            manage.UpdateTime = DateTime.Now;
            return this._manageRepository.UpdateEntity(manage);
        }

        #endregion

        #region 管理员信息
        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public ManageView GetManageView(string loginName)
        {
            var manageView = new ManageView();
            if (!string.IsNullOrEmpty(loginName))
            {
                IList<Condtion> condtions = new List<Condtion>() 
                { 
                    CondtionEqualState(),
                    CondtionLoginName(loginName)
                };
                var manage = this._manageRepository.GetEntity(condtions);
                if (manage != null)
                {
                    manageView = new ManageView()
                    {
                        Id = manage.Id,
                        NickName = manage.NickName,
                        Email = manage.Email,
                        LoginName = manage.LoginName
                    };
                }
            }
            return manageView;
        }
        #endregion

        #endregion

        #region 传入参数检测
        /// <summary>
        /// 检测LoginName不为空
        /// </summary>
        /// <param name="loginName"></param>
        private void CheckLoginName(string loginName)
        {
            AssertUtil.NotNullOrWhiteSpace(loginName, LanguageUtil.Translate("api_Business_Manage_CheckLoginName"));
        }
        /// <summary>
        /// 检测LoginName是否存在
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="manage"></param>
        private void CheckLoginName(string loginName, out Manage manage)
        {
            IList<Condtion> condtions = new List<Condtion>() { 
            CondtionEqualState(),
            CondtionLoginName(loginName.Trim())
            };
            manage = this._manageRepository.GetEntity(condtions);
            AssertUtil.IsNotNull(manage, LanguageUtil.Translate("api_Business_Manage_CheckLoginName_manage"));
        }
        /// <summary>
        /// 检测Password不为空
        /// </summary>
        /// <param name="password"></param>
        private void CheckPassword(string password)
        {
            AssertUtil.NotNullOrWhiteSpace(password, LanguageUtil.Translate("api_Business_Manage_CheckPassword"));
        }
        /// <summary>
        /// 检测旧密码Password不为空
        /// </summary>
        /// <param name="password"></param>
        private void CheckOldPassword(string password)
        {
            AssertUtil.NotNullOrWhiteSpace(password, LanguageUtil.Translate("api_Business_Manage_CheckOldPassword"));
        }
        /// <summary>
        /// 检测旧密码Password输入是否正确
        /// </summary>
        /// <param name="inputPassword"></param>
        /// <param name="oldPwd"></param>
        private void CheckOldPassword(string inputPassword, string oldPwd)
        {
            AssertUtil.AreEqual(Md5Helper.MD5(inputPassword.Trim(), 32), oldPwd, LanguageUtil.Translate("api_Business_Manage_CheckOldPassword_error"));
        }
        /// <summary>
        /// 检测新密码Password不为空
        /// </summary>
        /// <param name="password"></param>
        private void CheckNewPassword(string password)
        {
            AssertUtil.NotNullOrWhiteSpace(password, LanguageUtil.Translate("api_Business_Manage_CheckNewPassword"));
        }

        /// <summary>
        /// 检测新密码Password不为空
        /// </summary>
        /// <param name="password"></param>
        /// <param name="newPwd"></param>
        private void CheckNewPassword(string password, string newPwd)
        {
            AssertUtil.AreNotEqual(password, Md5Helper.MD5(newPwd.Trim(), 32), LanguageUtil.Translate("api_Business_Manage_CheckNewPassword_notSame"));
        }
        /// <summary>
        /// 检测确认密码Password不为空
        /// </summary>
        /// <param name="newPwd"></param>
        /// <param name="confirmPassword"></param>
        private void CheckConfirmPassword(string newPwd, string confirmPassword)
        {
            AssertUtil.AreEqual(newPwd, confirmPassword, LanguageUtil.Translate("api_Business_Manage_CheckConfirmPassword"));
        }
        /// <summary>
        /// 检测编号不能小于0
        /// </summary>
        private void CheckId()
        {
            AssertUtil.AreBigger(UserId, 0, LanguageUtil.Translate("api_Business_Manage_CheckId"));
        }
        /// <summary>
        /// 检测管理员是否存在
        /// </summary>
        /// <param name="manage"></param>
        private void CheckId(out Manage manage)
        {
            IList<Condtion> condtions = new List<Condtion>()
            {
                CondtionEqualState(),
                ConditionEqualId(UserId)
            };
            manage = this._manageRepository.GetEntity(condtions);
            AssertUtil.IsNotNull(manage, LanguageUtil.Translate("api_Business_Manage_CheckId_manage"));
        }
        /// <summary>
        /// 检测Password输入是否正确
        /// </summary>
        /// <param name="inputPassword"></param>
        /// <param name="password"></param>
        private void CheckPassword(string inputPassword, string password)
        {
            AssertUtil.AreEqual(Md5Helper.MD5(inputPassword.Trim(), 32), password, LanguageUtil.Translate("api_Business_Manage_CheckPassword_Same"));
        }
        #endregion

        #region 传入参数
        /// <summary>
        /// 比较LoginName相等
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        private Condtion CondtionLoginName(string loginName)
        {
            return new Condtion()
            {
                FiledName = "LoginName",
                FiledValue = loginName,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
        }


        #endregion

        #region 排序参数


        #endregion
    }
}
