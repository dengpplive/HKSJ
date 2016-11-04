using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Common.Logger;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Repository.Interface;
using Autofac;
using HKSJ.Utilities;
using HKSJ.WBVV.Entity.Enums;
using Manage = HKSJ.WBVV.Entity.ViewModel.Manage;
using HKSJ.Utilities.Base.Security;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{
    public class UserBusiness : BaseBusiness, IUserBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAttentionRepository _userAttention;
        private readonly IVideoRepository _videoRepository;
        private readonly IUserHistoryRepository _userHistoryRepository;
        private readonly IQiniuUploadBusiness _qiniuUploadBusiness;
        private readonly IAuthKeysRepository _authKeysRepository;
        private readonly IVideoApproveRepository _videoApproveRepository;
        private IVideoBusiness _iVideoBusiness;
        private readonly IUserBindRepository _userBindRepository;
        private readonly IUserFansRepository _userFansRepository;
        public UserBusiness(
            IUserRepository userRepository,
            IUserAttentionRepository userAttention,
            IVideoRepository videoRepository,
            IUserHistoryRepository userHistoryRepository,
            IQiniuUploadBusiness qiniuUploadBusiness,
            IAuthKeysRepository authKeysRepository, IUserBindRepository userBindRepository, IVideoApproveRepository videoApproveRepository, IUserFansRepository userFansRepository)
        {
            this._userRepository = userRepository;
            this._userAttention = userAttention;
            this._videoRepository = videoRepository;
            this._userHistoryRepository = userHistoryRepository;
            this._qiniuUploadBusiness = qiniuUploadBusiness;
            _authKeysRepository = authKeysRepository;
            this._videoApproveRepository = videoApproveRepository;
            _userFansRepository = userFansRepository;
            _userBindRepository = userBindRepository;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserView GetUserInfo(int userId)
        {
            User user;
            CheckUserId(userId, out user);
            return new UserView
            {
                Id = user.Id,
                NickName = user.NickName ?? "",
                Picture = user.Picture ?? "",
                Account = user.Account,
                Pwd = user.Password,
                BB = user.BB,
                UseBB = user.UseBB,
                PlayCount = user.PlayCount,
                FansCount = user.FansCount,
                SkinId = user.SkinId,
                BannerImage = user.BannerImage ?? "",
                Bardian = user.Bardian ?? "",
                Phone = user.Phone ?? "",
                Email = user.Email ?? "",
                SubscribeNum = user.SubscribeNum,
                Level = user.Level,
                State = user.State,
                IsSubed = false,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General) ?? "",
                City = user.City,
                Birthdate = user.Birthdate.HasValue ? Convert.ToDateTime(user.Birthdate).ToString("yyyy-MM-dd") : "",
                Gender = user.Gender.HasValue && Convert.ToBoolean(user.Gender)
            };
        }

        /// <summary>
        /// 用户登录
        /// Author : AxOne
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="password">已用MD5加密成16位的用户密码</param>
        /// <returns></returns>
        public UserView LoginUser(string account, string password)
        {
            this._userRepository.IncomeLogin(account, password, IpAddress);
            var users = (from u in this._userRepository.GetEntityList() where u.State == false && u.Password == password.UrlDecode() select u).ToList();
            var user = users.FirstOrDefault(p => p.Phone == account.UrlDecode() || p.Account == account.UrlDecode());
            if (user == null || user.Id < 1)
            {
                user = users.FirstOrDefault(p => p.Email == account.UrlDecode() || p.Account == account.UrlDecode());
            }
            var userView = new UserView
            {
                Id = user.Id,
                Account = user.Account,
                Pwd = user.Password,
                NickName = user.NickName ?? "",
                BB = user.BB,
                UseBB = user.UseBB,
                Picture = user.Picture ?? "",
                Phone = user.Phone ?? "",
                Email = user.Email ?? "",
                SubscribeNum = user.SubscribeNum,
                FansCount = user.FansCount,
                PlayCount = user.PlayCount,
                SkinId = user.SkinId,
                Bardian = user.Bardian ?? "",
                BannerImage = user.BannerImage ?? "",
                Level = user.Level,
                State = user.State,
                IsSubed = false,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General) ?? ""
            };
            return userView;
        }

        /// <summary>
        /// 用户登录SSO
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="password">已用MD5加密成16位的用户密码</param>
        /// <returns></returns>
        public UserView LoginUser(int userId)
        {
            this._userRepository.IncomeLogin(userId, IpAddress);
            var user = (from u in this._userRepository.GetEntityList(ConditionEqualId(userId)) where u.State == false select u).FirstOrDefault<User>();
            var userView = new UserView
            {
                Id = user.Id,
                Account = user.Account,
                Pwd = user.Password,
                NickName = user.NickName,
                BB = user.BB,
                Picture = user.Picture,
                Phone = user.Phone,
                SubscribeNum = user.SubscribeNum,
                FansCount = user.FansCount,
                SkinId = user.SkinId,
                Bardian = user.Bardian,
                BannerImage = user.BannerImage,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General)
            };
            return userView;
        }

        /// <summary>
        /// 用户注册
        /// Author : AxOne
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="password">用户密码</param>
        /// <param name="type">账户类型(0:手机1:邮箱)</param>
        /// <returns></returns>
        public UserView RegisterUser(string account, string password, int type)
        {
            this._userRepository.IncomeRegister(account, password, IpAddress, type);
            var user = (from u in this._userRepository.GetEntityList()
                        where u.Account == account.UrlDecode()
                        select u).FirstOrDefault();
            return new UserView
            {
                Id = user.Id,
                Account = user.Account,
                Pwd = user.Password,
                NickName = user.NickName,
                Phone = user.Phone ?? "",
                Email = user.Email ?? "",
                PlayCount = user.PlayCount,
                FansCount = user.FansCount,
                BB = user.BB,
                Picture = user.Picture ?? "",
                SubscribeNum = user.SubscribeNum,
                BannerImage = user.BannerImage ?? "",
                SkinId = user.SkinId,
                Level = user.Level,
                Bardian = user.Bardian ?? "",
                UseBB = user.UseBB,
                State = user.State,
                IsSubed = false,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General)
            };
        }

        /// <summary>
        /// 设置账号
        /// Author : AxOne
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public UserView AccountSet(AccountPara para)
        {
            User user;
            CheckUserId(para.Id, out user);
            user.Gender = para.Gender != 1;
            if (!string.IsNullOrEmpty(para.Birthdate)) user.Birthdate = Convert.ToDateTime(para.Birthdate);
            user.Province = para.Province ?? "";
            user.ProvinceCode = para.ProvinceCode ?? 0;
            user.City = para.City ?? "";
            user.CityCode = para.CityCode ?? 0;
            user.Zodiac = para.Zodiac ?? 0;
            user.Constellation = para.Constellation ?? 0;
            user.Bardian = para.Bardian ?? "";
            user.NickName = para.NickName ?? "";
            user.UpdateTime = DateTime.Now;
            _userRepository.UpdateEntity(user);
            return new UserView
            {
                Id = user.Id,
                Account = user.Account,
                Pwd = user.Password,
                NickName = user.NickName,
                Phone = user.Phone,
                PlayCount = user.PlayCount,
                FansCount = user.FansCount,
                BB = user.BB,
                Picture = user.Picture,
                SubscribeNum = user.SubscribeNum,
                BannerImage = user.BannerImage,
                SkinId = user.SkinId,
                Level = user.Level,
                Bardian = user.Bardian,
                UseBB = user.UseBB,
                State = user.State,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General)
            };
        }

        /// <summary>
        /// 根据手机号修改密码
        /// Author : AxOne
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="password">MD5加密成16位的新密码</param>
        /// <returns></returns>
        public UserView UpdatePwdByPhone(string account, string password)
        {
            account = account.UrlDecode();
            password = password.UrlDecode();
            CheckAccountNotNull(account);
            CheckPwdNotNull(password);
            User user;
            CheckAccount(account, out user);
            user.Password = password;
            user.UpdateTime = DateTime.Now;
            _userRepository.UpdateEntity(user);
            return new UserView
            {
                Id = user.Id,
                Account = user.Account,
                Pwd = user.Password,
                NickName = user.NickName,
                BB = user.BB,
                Picture = user.Picture,
                Phone = user.Phone,
                SubscribeNum = user.SubscribeNum,
                FansCount = user.FansCount,
                BannerImage = user.BannerImage,
                SkinId = user.SkinId,
                Level = user.Level,
                Bardian = user.Bardian,
                UseBB = user.UseBB,
                State = user.State,
                PlayCount = user.PlayCount,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General)
            };
        }

        /// <summary>
        /// 获取用户账户信息
        /// Author : AxOne
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public AccountView GetAccountInfo(int uid)
        {
            User user;
            CheckUserId(uid, out user);
            return new AccountView
            {
                Id = user.Id,
                Bardian = user.Bardian,
                City = user.City,
                CityCode = user.CityCode,
                Province = user.Province,
                ProvinceCode = user.ProvinceCode,
                Gender = Convert.ToBoolean(user.Gender) ? 0 : 1,
                Constellation = user.Constellation,
                Zodiac = user.Zodiac,
                Birthdate = user.Birthdate.HasValue ? Convert.ToDateTime(user.Birthdate).ToString("yyyy-MM-dd") : "",
                NickName = user.NickName,
                LastLoginIp = user.LastLoginIp,
                Email = user.Email,
                Phone = user.Phone,
                Pwd = user.Password
            };
        }

        /// <summary>
        /// 检查密码
        /// Author : AxOne
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pwd">已用MD5加密成16位的用户密码</param>
        /// <returns></returns>
        public bool CheckPwd(int uid, string pwd)
        {
            pwd = pwd.UrlDecode();
            CheckPwdNotNull(pwd);
            User user;
            CheckUserId(uid, out user);
            return string.Equals(user.Password, pwd);
        }

        /// <summary>
        /// 检查手机号
        /// Author : AxOne
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool CheckPhone(int uid, string phone)
        {
            phone = phone.UrlDecode();
            CheckPwdNotNull(phone);
            User user;
            CheckUserId(uid, out user);
            return string.Equals(phone, user.Phone);
        }

        /// <summary>
        /// 检查昵称是否唯一
        /// Author : AxOne
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public bool CheckNickName(int uid, string nickName)
        {
            nickName = nickName.UrlDecode();
            CheckNickNameNotNull(nickName);
            var condition = new Condtion
            {
                FiledName = "NickName",
                FiledValue = nickName,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            var user = _userRepository.GetEntity(condition);
            if (user == null)
            {
                return true;
            }
            return user.Id == uid;
        }

        /// <summary>
        /// 修改用户头像,修改key用来获取七牛图片
        /// Author : AxOne
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public UserView UpdateUserPic(int uid, string key)
        {
            CheckKeyNotNull(key);
            User user;
            CheckUserId(uid, out user);
            user.Picture = key;
            _userRepository.UpdateEntity(user);
            return new UserView
            {
                Id = user.Id,
                Account = user.Account,
                Pwd = user.Password,
                NickName = user.NickName,
                BB = user.BB,
                Picture = user.Picture,
                Phone = user.Phone,
                SubscribeNum = user.SubscribeNum,
                FansCount = user.FansCount,
                BannerImage = user.BannerImage,
                SkinId = user.SkinId,
                Level = user.Level,
                Bardian = user.Bardian,
                UseBB = user.UseBB,
                State = user.State,
                PlayCount = user.PlayCount,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General)
            };
        }

        /// <summary>
        /// 获取用户头像Key
        /// Author : AxOne
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string GetUserPicKey(int uid)
        {
            User user;
            CheckUserId(uid, out user);
            if (user == null || string.IsNullOrWhiteSpace(user.Picture))
            {
                return "NULL";
            }
            return user.Picture;
        }

        /// <summary>
        /// 重置密码
        /// Author : AxOne
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pwd">已用MD5加密成16位的用户密码</param>
        /// <returns></returns>
        public UserView SetPassword(int uid, string pwd)
        {
            pwd = pwd.UrlDecode();
            CheckPwdNotNull(pwd);
            User user;
            CheckUserId(uid, out user);
            user.Password = pwd;
            _userRepository.UpdateEntity(user);
            var userView = new UserView
            {
                Id = user.Id,
                Account = user.Account,
                Pwd = user.Password,
                NickName = user.NickName,
                BB = user.BB,
                Picture = user.Picture,
                Phone = user.Phone,
                SubscribeNum = user.SubscribeNum,
                FansCount = user.FansCount,
                BannerImage = user.BannerImage,
                SkinId = user.SkinId,
                Level = user.Level,
                Bardian = user.Bardian,
                UseBB = user.UseBB,
                State = user.State,
                PlayCount = user.PlayCount,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General)
            };
            return userView;
        }

        /// <summary>
        /// 重置手机号码
        /// Author : AxOne
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public UserView SetPhone(int uid, string phone, string pwd = "")
        {
            phone = phone.UrlDecode();
            CheckPhoneNotNull(phone);
            User user;
            CheckUserId(uid, out user);
            user.Phone = phone;
            user.Account = phone;
            if (!string.IsNullOrWhiteSpace(pwd)) user.Password = Md5Helper.MD5(pwd, 32);//TODO 用户密码 MD5加密
            _userRepository.UpdateEntity(user);
            var userView = new UserView
            {
                Id = user.Id,
                Account = user.Account,
                Pwd = user.Password,
                NickName = user.NickName,
                BB = user.BB,
                Picture = user.Picture,
                Phone = user.Phone,
                SubscribeNum = user.SubscribeNum,
                FansCount = user.FansCount,
                BannerImage = user.BannerImage,
                SkinId = user.SkinId,
                Level = user.Level,
                Bardian = user.Bardian,
                UseBB = user.UseBB,
                State = user.State,
                PlayCount = user.PlayCount,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General)
            };
            return userView;
        }

        /// <summary>
        /// 重置邮箱
        /// Author : AxOne
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserView SetEmail(int uid, string email, string pwd = "")
        {
            email = email.UrlDecode();
            CheckEmailNotNull(email);
            User user;
            CheckUserId(uid, out user);
            user.Email = email;
            user.Account = email; 
            if (!string.IsNullOrWhiteSpace(pwd)) user.Password = Md5Helper.MD5(pwd, 32);//TODO 用户密码 MD5加密
            _userRepository.UpdateEntity(user);
            var userView = new UserView
            {
                Id = user.Id,
                Account = user.Account,
                Pwd = user.Password,
                NickName = user.NickName,
                BB = user.BB,
                Picture = user.Picture,
                Phone = user.Phone,
                SubscribeNum = user.SubscribeNum,
                FansCount = user.FansCount,
                BannerImage = user.BannerImage,
                SkinId = user.SkinId,
                Level = user.Level,
                Bardian = user.Bardian,
                UseBB = user.UseBB,
                State = user.State,
                PlayCount = user.PlayCount,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General)
            };
            return userView;
        }

        /// <summary>
        /// 检测账户是否已经存在
        /// Author : AxOne
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool IUserBusiness.CheckAccount(string account)
        {
            CheckAccountNotNull(account);
            return _userRepository.GetEntity(CondtionEqualAccount(account)) == null;
        }

        /// <summary>
        /// 检测邮箱是否已经存在
        /// Author : AxOne
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool IUserBusiness.CheckEmail(string email)
        {
            CheckEmailNotNull(email);
            return _userRepository.GetEntity(CondtionEqualEmail(email)) == null;
        }

        /// <summary>
        /// 修改用户签名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Bardian"></param>
        /// <returns></returns>
        public bool UpdateUserBardian(int userId, string bardian)
        {
            User user;
            CheckUserId(userId, out user);
            user.Bardian = bardian;
            return _userRepository.UpdateEntity(user);
        }

        /// <summary>
        /// 多条件查询用户信息
        /// Author : AxOne
        /// </summary>
        /// <returns></returns>
        public PageResult SearchUser(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            int totalCount;
            int totalIndex;
            var plateViews = GetUsers(condtions, orderCondtions, out totalCount, out totalIndex);
            return new PageResult
            {
                PageSize = PageSize,
                PageIndex = PageIndex,
                TotalCount = totalCount,
                TotalIndex = totalIndex,
                Data = plateViews
            };
        }

        /// <summary>
        /// 禁用(启用)用户
        /// Author : AxOne
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool ForbiddenUser(int uid, bool state)
        {
            User user;
            CheckUserId(uid, out user);
            user.State = state;
            return _userRepository.UpdateEntity(user);
        }

        /// <summary>
        /// 数据列表
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalIndex"></param>
        /// <returns></returns>
        public IList<Manage.UserView> GetUsers(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions, out int totalCount, out int totalIndex)
        {
            var models = (from v in _userRepository.GetEntityList()
                          select new Manage.UserView
                              {
                                  Id = v.Id,
                                  Account = v.Account,
                                  NickName = v.NickName ?? "",
                                  Phone = v.Phone ?? "",
                                  Email = v.Email,
                                  Bardian = v.Bardian,
                                  IDCardNo = v.IDCardNo,
                                  Province = v.Province,
                                  City = v.City,
                                  PlayCount = v.PlayCount,
                                  Birthdate = Convert.ToDateTime(v.Birthdate).ToString("yyyy-MM-dd"),
                                  Gender = Convert.ToBoolean(v.Gender) ? LanguageUtil.Translate("api_Business_User_GetUsers_Gender_man") : LanguageUtil.Translate("api_Business_User_GetUsers_Gender_woman"),
                                  Level = v.Level,
                                  BB = v.BB,
                                  UseBB = v.UseBB,
                                  FansCount = v.FansCount,
                                  SubscribeNum = v.SubscribeNum,
                                  CreateTime = v.CreateTime,
                                  State = v.State ? LanguageUtil.Translate("api_Business_User_GetUsers_Gender_forbidden") : LanguageUtil.Translate("api_Business_User_GetUsers_Gender_enabled")
                              }).AsQueryable();

            if (condtions != null && condtions.Count > 0)
            {
                models = models.Query(condtions);
            }
            var isExists = models.Any();
            if (isExists)
            {
                if (orderCondtions != null && orderCondtions.Count > 0)
                {
                    models = models.OrderBy(orderCondtions);
                }
            }
            totalCount = isExists ? models.Count() : 0;
            if (PageSize <= 0)
            {
                totalIndex = 0;
                var queryable = isExists ? models.ToList() : new List<Manage.UserView>();
                return queryable;
            }
            else
            {
                totalIndex = totalCount % PageSize == 0 ? (totalCount / PageSize) : (totalCount / PageSize + 1);
                if (PageIndex <= 0)
                {
                    PageIndex = 1;
                }
                if (PageIndex >= totalIndex)
                {
                    PageIndex = totalIndex;
                }
                var queryable = isExists ? models.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList() : new List<Manage.UserView>();
                return queryable;
            }
        }


        /// <summary>
        /// 修改个人空间背景图地址
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bannerImage"></param>
        /// <returns></returns>
        public UserView UpdateUserBannerImage(int userId, string bannerImage)
        {
            CheckBackgroundImageNotNull(bannerImage);
            User user;
            CheckUserId(userId, out user);
            user.BannerImage = bannerImage;
            _userRepository.UpdateEntity(user);
            return new UserView
            {
                Id = user.Id,
                Account = user.Account,
                Pwd = user.Password,
                NickName = user.NickName,
                BB = user.BB,
                Picture = user.Picture,
                Phone = user.Phone,
                SubscribeNum = user.SubscribeNum,
                FansCount = user.FansCount,
                BannerImage = user.BannerImage,
                PlayCount = user.PlayCount,
                UseBB = user.UseBB,
                State = user.State,
                Bardian = user.Bardian,
                Level = user.Level,
                SkinId = user.SkinId,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General)
            };
        }

        /// <summary>
        /// 更换用户的皮肤
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="skinId">皮肤id</param>
        /// <returns></returns>
        public bool SaveSkinByUserId(int userId, int skinId)
        {
            User user;
            CheckUserId(userId, out user);
            user.SkinId = skinId;
            return _userRepository.UpdateEntity(user);
        }

        #region 分享接口
        public bool IncomeShare(int videoId, int demandUserId, string ipAddress, int shareUserId, string shareUserIp)
        {
            this._videoRepository.IncomeShare(videoId, demandUserId, IpAddress, shareUserId, shareUserIp);
            return true;
        }
        #endregion


        #region 根据用户ID查询用户
        /// <summary>
        /// 根据用户ID查询用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public User GetEntityByUserId(int uid)
        {
            var condtion = new Condtion()
            {
                FiledName = "Id",
                FiledValue = uid,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            User userInfo = _userRepository.GetEntity(condtion);
            return userInfo;
        }
        /// <summary>
        /// 根据用户ID查询用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserView GetUser(int id)
        {
            if (id <= 0)
            {
                return new UserView();
            }
            IList<Condtion> condtions = new List<Condtion>()
            {
                ConditionEqualId(id),
                CondtionEqualState()
            };
            var user = _userRepository.GetEntity(condtions);
            if (user == null)
            {
                return new UserView();
            }
            return new UserView()
            {
                Account = user.Account,
                BB = user.BB,
                UseBB = user.UseBB,
                FansCount = user.FansCount,
                Id = user.Id,
                NickName = user.NickName,
                PlayCount = user.PlayCount,
                Picture = user.Picture,
                Pwd = user.Password,
                SubscribeNum = user.SubscribeNum,
                State = user.State,
                BannerImage = user.BannerImage,
                Phone = user.Phone,
                Bardian = user.Bardian,
                SkinId = user.SkinId,
                Level = user.Level,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General)
            };
        }

        public UserView GetUserRoomInfo(int loginUserId, int browserUserId)
        {
            var userview = GetUser(browserUserId);
            userview.IsSubed = IsLogin(this._userRepository.GetEntityList(),loginUserId)&&IsSubed(this._userFansRepository.GetEntityList(),browserUserId,loginUserId);
            return userview;
        }
        #endregion

        #region 用户粉丝数
        /// <summary>
        /// 根据用户ID查询用户粉丝数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int GetFansByUserId(int uid)
        {
            var condtion = new Condtion()
            {
                FiledName = "StarUserId",
                FiledValue = uid,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            List<UserAttention> userAttentions = _userAttention.GetEntityList(condtion).ToList();
            return userAttentions.Count;
        }
        #endregion

        #region 播放次数
        /// <summary>
        /// 得到播放次数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int GetPlayCountByUserId(int uid)
        {
            var condtion = new Condtion()
            {
                FiledName = "CreateManageId",
                FiledValue = uid,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            List<Video> videos = _videoRepository.GetEntityList(condtion).ToList();
            var playNum = videos.Sum(c => c.PlayCount);
            return playNum;
        }
        #endregion

        #region 新增一个视频观看记录

        /// <summary>
        /// 新增一个视频观看记录
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="userId"></param>
        /// <param name="watchTime"></param>
        /// <returns></returns>
        public bool AddHistoryVideo(int videoId, int userId, int watchTime)
        {
            // if (watchTime < 1) return false;
            User user;
            CheckUserId(userId, out user);
            Video video;
            CheckVideoId(videoId, out video);
            var userHistory = new UserHistory()
            {
                UserId = userId,
                VideoId = videoId,
                WatchTime = watchTime,
                CreateUserId = userId,
                IpAddress = IpAddress,
                CreateTime = DateTime.Now
            };
            var condtions = new List<HKSJ.WBVV.Common.Extender.LinqExtender.Condtion>()
            {
                CondtionEqualCreateUserId(userId),
                CondtionEqualVideoId(videoId)
            };
            var flagHistory = this._userHistoryRepository.GetEntity(condtions);
            bool success;
            if (flagHistory == null)
            {
                success = this._userHistoryRepository.CreateEntity(userHistory);
            }
            else
            {
                flagHistory.CreateTime = DateTime.Now;
                flagHistory.WatchTime = watchTime;
                success = this._userHistoryRepository.UpdateEntity(flagHistory);
            }

            return success;
        }
        #endregion


        #region 得到用户观看记录
        /// <summary>
        /// 得到用户观看记录
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public UserHistoryViews GetHistoryVideoByUserId(int uid, int pageIndex, int pageSize)
        {
            UserHistoryViews userHistoryViewses = new UserHistoryViews();
            var condtion = new Condtion()
            {
                FiledName = "UserId",
                FiledValue = uid,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            var userHistories = _userHistoryRepository.GetEntityList(condtion);
            var vv = _videoRepository.GetEntityList(CondtionEqualState());
            if (userHistories == null) return userHistoryViewses;
            var queryable = (from uh in userHistories
                             join v in vv on (long)uh.VideoId equals v.Id
                             where v.VideoState == 3//TODO 刘强CheckState=1
                             select new UserHistoryView()
                             {
                                 Id = uh.Id,
                                 SmallPicturePath = v.SmallPicturePath,
                                 Title = v.Title,
                                 UserId = Convert.ToInt32(uh.UserId),
                                 VideoId = Convert.ToInt32(uh.VideoId),
                                 WatchTime = Convert.ToInt32(uh.WatchTime),
                                 CreateTime = Convert.ToDateTime(uh.CreateTime)
                             }).AsQueryable();

            //今天观看记录
            var todayHistories = queryable.Where(c => 0 == c.SpanDay).OrderByDescending(c => c.CreateTime).Take(20).ToList();
            //昨天观看记录
            var yesterdayHistories = queryable.Where(c => c.SpanDay == 1).OrderByDescending(c => c.CreateTime).Take(20).ToList();
            //一周内
            var oneWeekHistories = queryable.Where(c => c.SpanDay <= 7 && c.SpanDay >= 2).OrderByDescending(c => c.CreateTime).Take(20).ToList();
            //更多 
            userHistoryViewses.TotalMoreCount = queryable.Count(c => c.SpanDay > 7);
            var moreHistories = queryable.Where(c => c.SpanDay > 7).OrderByDescending(c => c.CreateTime).Take(20).ToList();

            userHistoryViewses.TodayHistories = todayHistories;
            userHistoryViewses.YesterdayHistories = yesterdayHistories;
            userHistoryViewses.OneWeekHistories = oneWeekHistories;
            userHistoryViewses.MoreHistories = moreHistories;
            return userHistoryViewses;
        }
        #endregion

        /// <summary>
        /// 获取当前用户观看视频的记录
        /// </summary>
        /// <param name="uId">用户ID</param>
        /// <param name="videoId">视频ID</param>
        /// <returns>已观看视频的时间，以秒为单位</returns>
        public long GetUserWatchTime(int uId, int videoId)
        {
            var condtion = new Condtion()
            {
                FiledName = "UserId",
                FiledValue = uId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            IList<Condtion> condtions = new List<Condtion>()
                {
                    CondtionEqualVideoId(videoId),
                    condtion
                };

            var uHistory = this._userHistoryRepository.GetEntity(condtions);
            if (uHistory != null)
            {
                Video v = this._videoRepository.GetEntity(ConditionEqualId(videoId));
                if (v != null)
                {
                    if (uHistory.WatchTime < v.TimeLength)
                    {
                        return uHistory.WatchTime;
                    }
                }
            }
            return 0;
        }

        #region 获取用户上传视频

        /// <summary>
        /// 获取用户上传视频
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="videoState"></param>
        /// <returns></returns>
        public MyVideoViewResult GetUserVideoViews(int userId, int pageIndex, int pageSize, int videoState)
        {
            var result = new MyVideoViewResult() { MyVideoViews = new List<MyVideoView>(), TotalCount = 0 };
            this._iVideoBusiness = ((Autofac.IContainer)HttpRuntime.Cache["containerKey"]).Resolve<IVideoBusiness>();
            IQueryable<MyVideoView> queryable;
            if (videoState == -1)
            {
                queryable =
                   (from myvideo in _videoRepository.GetEntityList(CondtionEqualCreateManageId(userId))
                    join vappay in _videoApproveRepository.GetEntityList() on myvideo.Id equals vappay.VideoId into mjv
                    from mv in mjv.DefaultIfEmpty()
                    select new MyVideoView
                    {
                        Id = myvideo.Id,
                        Title = myvideo.Title,
                        About = myvideo.About,
                        Tags = myvideo.Tags,
                        CommentCount = myvideo.CommentCount,
                        PlayCount = myvideo.PlayCount,
                        SmallPicturePath = myvideo.SmallPicturePath,
                        VideoState = myvideo.VideoState,
                        CreateTime = myvideo.CreateTime,
                        ApproveContent = mv != null ? mv.ApproveContent : "",
                        ApproveRemark = mv != null ? mv.ApproveRemark : ""
                    }).AsQueryable();
            }
            else
            {
                queryable =
                  (from myvideo in _videoRepository.GetEntityList(CondtionEqualCreateManageId(userId))
                   join vappay in _videoApproveRepository.GetEntityList() on myvideo.Id equals vappay.VideoId into mjv
                   from mv in mjv.DefaultIfEmpty()
                   where myvideo.VideoState == videoState
                   select new MyVideoView
                   {
                       Id = myvideo.Id,
                       Title = myvideo.Title,
                       About = myvideo.About,
                       Tags = myvideo.Tags,
                       CommentCount = myvideo.CommentCount,
                       PlayCount = myvideo.PlayCount,
                       SmallPicturePath = myvideo.SmallPicturePath,
                       VideoState = myvideo.VideoState,
                       CreateTime = myvideo.CreateTime,
                       ApproveContent = mv != null ? mv.ApproveContent : "",
                       ApproveRemark = mv != null ? mv.ApproveRemark : ""
                   }).AsQueryable();

            }
            if (!queryable.Any()) return result;
            queryable = queryable.Where(c => !string.IsNullOrEmpty(c.Title));
            var mylist = queryable.ToList();
            result.TotalCount = mylist.Count;
            result.PageCount = result.TotalCount % pageSize == 0
                ? result.TotalCount / pageSize
                : result.TotalCount / pageSize + 1;
            result.MyVideoViews = mylist.OrderByDescending(c => c.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return result;
        }
        #endregion

        #region 删除用户所有观看记录
        /// <summary>
        /// 删除用户所有观看记录
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool DelAllRecByUserId(int uid)
        {
            var condtion = new Condtion()
            {
                FiledName = "UserId",
                FiledValue = uid,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            try
            {
                _userHistoryRepository.DeleteEntitys(_userHistoryRepository.GetEntityList(condtion).ToList());
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        #endregion


        #region 传入参数
        /// <summary>
        /// 比较用户账号相等
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private Condtion CondtionEqualAccount(string account)
        {
            var condtion = new Condtion()
            {
                FiledName = "Account",
                FiledValue = account,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        /// <summary>
        /// 比较用户账号相等
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private Condtion CondtionEqualEmail(string email)
        {
            var condtion = new Condtion()
            {
                FiledName = "Email",
                FiledValue = email,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        private Condtion CondtionEqualPhone(string phone)
        {
            var condtion = new Condtion()
            {
                FiledName = "Phone",
                FiledValue = phone,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        /// <summary>
        /// 比较上传用户相等
        /// </summary>
        /// <param name="createManageId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualCreateManageId(int createManageId)
        {
            var condtion = new Condtion()
            {
                FiledName = "CreateManageId",
                FiledValue = createManageId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        /// <summary>
        /// 比较上传用户相等
        /// </summary>
        /// <param name="createManageId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualCreateUserId(int createUserId)
        {
            var condtion = new Condtion()
            {
                FiledName = "CreateUserId",
                FiledValue = createUserId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        /// <summary>
        /// 判断用户专辑子表下专辑编号相等
        /// </summary>
        /// <param name="specialId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualSpecialId(int specialId)
        {
            var condtion = new Condtion()
            {
                FiledName = "MySpecialId",
                FiledValue = specialId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        // <summary>
        /// 判断视频编号相等
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualVideoId(int videoId)
        {
            var condtion = new Condtion()
            {
                FiledName = "VideoId",
                FiledValue = videoId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }


        #endregion

        #region 传入参数检测
        /// <summary>
        /// 个人空间上传的背景图片不能为空
        /// </summary>
        /// <param name="backgroundImage"></param>
        private void CheckBackgroundImageNotNull(string backgroundImage)
        {
            AssertUtil.NotNullOrWhiteSpace(backgroundImage, LanguageUtil.Translate("api_Business_User_CheckBackgroundImageNotNull_theBackgroundPictureCanNottEeEmpty"));
        }
        /// <summary>
        /// 检测用户账号不为空
        /// </summary>
        /// <param name="account"></param>
        private void CheckAccountNotNull(string account)
        {
            AssertUtil.NotNullOrWhiteSpace(account, LanguageUtil.Translate("api_Business_User_CheckAccountNotNull_accountIsNull"));
        }

        /// <summary>
        /// 检测qiniu key不为空
        /// </summary>
        /// <param name="key"></param>
        private void CheckKeyNotNull(string key)
        {
            AssertUtil.NotNullOrWhiteSpace(key, LanguageUtil.Translate("api_Business_User_CheckKeyNotNull_qiniukeyIsNull"));
        }

        /// <summary>
        /// 检测用户昵称不为空
        /// </summary>
        /// <param name="nickName"></param>
        private void CheckNickNameNotNull(string nickName)
        {
            AssertUtil.NotNullOrWhiteSpace(nickName, LanguageUtil.Translate("api_Business_User_CheckNickNameNotNull_nickNameIsNull"));
        }

        /// <summary>
        /// 检测用户密码不为空
        /// </summary>
        /// <param name="pwd"></param>
        private void CheckPwdNotNull(string pwd)
        {
            AssertUtil.NotNullOrWhiteSpace(pwd, LanguageUtil.Translate("api_Business_User_CheckPwdNotNull_pwdIsNull"));
        }

        /// <summary>
        /// 检测用户手机号码不为空
        /// </summary>
        /// <param name="phone"></param>
        private void CheckPhoneNotNull(string phone)
        {
            AssertUtil.NotNullOrWhiteSpace(phone, LanguageUtil.Translate("api_Business_User_CheckPhoneNotNull_phoneIsNull"));
        }

        /// <summary>
        /// 检测用户邮箱不为空
        /// </summary>
        /// <param name="email"></param>
        private void CheckEmailNotNull(string email)
        {
            AssertUtil.NotNullOrWhiteSpace(email, LanguageUtil.Translate("api_Business_User_CheckEmailNotNull_emailIsNull"));
        }

        /// <summary>
        /// 检测用户账号是否存在
        /// </summary>
        /// <param name="account"></param>
        private void CheckAccount(string account)
        {
            var condtionAccount = CondtionEqualAccount(account);
            var userAccount = this._userRepository.GetEntity(condtionAccount);
            AssertUtil.IsNull(userAccount, LanguageUtil.Translate("api_Business_User_CheckAccount_accountAlreadyExist"));
        }

        /// <summary>
        /// 检测用户账号是否存在
        /// </summary>
        /// <param name="account"></param>
        private void CheckAccount(string account, out User user)
        {
            var condtionAccount = CondtionEqualAccount(account);
            user = _userRepository.GetEntity(condtionAccount);
            if (user == null)
            {
                var condtionEmail = CondtionEqualEmail(account);
                user = _userRepository.GetEntity(condtionEmail);
            }
            if (user == null)
            {
                var condtionPhone = CondtionEqualPhone(account);
                user = _userRepository.GetEntity(condtionPhone);
            }
            AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Business_User_CheckAccount_accountNotExist"));
        }

        /// <summary>
        /// 检测用户邮箱账号是否存在
        /// </summary>
        /// <param name="email"></param>
        private void CheckEmailAccount(string email, out User user)
        {
            var condtionAccount = CondtionEqualEmail(email);
            user = _userRepository.GetEntity(condtionAccount);
            AssertUtil.IsNotNull(user,LanguageUtil.Translate("api_Business_User_CheckEmailAccount_accountNotExist"));
        }

        private void CheckAccount(string account, string pwd, out User user)
        {
            var condtions = new List<Condtion>
            {
                new Condtion
                {
                    FiledName = "Account",
                    FiledValue = account,
                    ExpressionLogic = ExpressionLogic.And,
                    ExpressionType = ExpressionType.Equal
                },
                new Condtion
                {
                    FiledName = "Password",
                    FiledValue = pwd,
                    ExpressionLogic = ExpressionLogic.And,
                    ExpressionType = ExpressionType.Equal
                },
                new Condtion
                {
                    FiledName = "State",
                    FiledValue = false,
                    ExpressionLogic = ExpressionLogic.And,
                    ExpressionType = ExpressionType.Equal
                }
            };
            user = _userRepository.GetEntity(condtions);
            AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Business_User_CheckAccount_accountNotExistOrpwdMistake"));
        }

        /// <summary>
        /// 检测用户是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        private void CheckUserId(int userId, out User user)
        {
            user = _userRepository.GetEntity(ConditionEqualId(userId));
            AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Business_User_CheckUserId_userIdNotExist"));
        }

        /// <summary>
        /// 检测视频编号
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="video"></param>
        private void CheckVideoId(int videoId, out Video video)
        {
            var condtionId = ConditionEqualId(videoId);
            video = _videoRepository.GetEntity(condtionId);
            AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Business_User_CheckVideoId_videoIdNotExist"));
        }
        #endregion

        #region 第三方绑定、登录,注册、绑定，自动注册、绑定（跳过）处理
        /// <summary>
        /// 第三方绑定并登录
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="password">已用MD5加密成16位的用户密码</param>
        /// <param name="typeCode">第三方类型编码</param>
        /// <param name="relatedId">第三方唯一身份标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="figureURL">头像路径</param>
        /// <returns></returns>
        public UserView ThirdPartyBindAndLogin(string account, string password, string typeCode, string relatedId, string nickName, string figureURL)
        {
            //给nickName做转码处理
            nickName = Base64Util.Base64Decrypt(nickName);

            this._userRepository.ThirdPartyBindAndLogin(account, password, typeCode, relatedId, nickName, figureURL, IpAddress);
            var user = (from u in this._userRepository.GetEntityList()
                        where u.State == false && u.Account == account.UrlDecode() && u.Password == password.UrlDecode()
                        select u).FirstOrDefault();

            var userView = new UserView
            {
                Id = user.Id,
                Account = user.Account,
                Pwd = user.Password,
                NickName = user.NickName,
                BB = user.BB,
                Picture = user.Picture,
                Phone = user.Phone,
                SubscribeNum = user.SubscribeNum,
                FansCount = user.FansCount,
                SkinId = user.SkinId,
                Bardian = user.Bardian,
                BannerImage = user.BannerImage,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General)
            };
            return userView;
        }

        /// <summary>
        /// 第三方注册并绑定
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="password">用户密码</param>
        /// <param name="typeCode">第三方类型编码</param>
        /// <param name="relatedId">第三方唯一身份标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="figureURL">头像路径</param>
        /// <returns></returns>
        public UserView ThirdPartyBindAndRegister(string account, string password, string typeCode, string relatedId, string nickName, string figureURL, int type)
        {
            //给nickName做转码处理
            nickName = Base64Util.Base64Decrypt(nickName);

            this._userRepository.ThirdPartyBindAndRegister(account, password, typeCode, relatedId, nickName, figureURL, type, IpAddress);
            var user = (from u in this._userRepository.GetEntityList()
                        where u.Account == account.UrlDecode()
                        select u).FirstOrDefault();
            return new UserView
            {
                Id = user.Id,
                Account = user.Account,
                Pwd = user.Password,
                NickName = user.NickName,
                Phone = user.Phone,
                PlayCount = user.PlayCount,
                FansCount = user.FansCount,
                BB = user.BB,
                Picture = user.Picture,
                SubscribeNum = user.SubscribeNum,
                BannerImage = user.BannerImage,
                SkinId = user.SkinId,
                Level = user.Level,
                Bardian = user.Bardian,
                UseBB = user.UseBB,
                State = user.State,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General)
            };
        }

        /// <summary>
        /// 自动注册并绑定第三方
        /// </summary>
        /// <param name="typeCode">第三方类型编码</param>
        /// <param name="relatedId">第三方唯一身份标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="figureURL">头像路径</param>
        /// <returns></returns>
        public UserView AutoRegisterAndBindThirdParty(string typeCode, string relatedId, string nickName, string figureURL)
        {
            //给nickName做转码处理
            nickName = Base64Util.Base64Decrypt(nickName);

            this._userRepository.AutoRegisterAndBindThirdParty(typeCode, relatedId, nickName, figureURL, IpAddress);
            var user = (from u in this._userRepository.GetEntityList()
                        join ub in this._userBindRepository.GetEntityList(CondtionEqualState()) on u.Id equals ub.UserId
                        where ub.RelatedId == relatedId.UrlDecode() && ub.TypeCode == typeCode.UrlDecode()
                        select u).FirstOrDefault();
            return new UserView
            {
                Id = user.Id,
                Account = user.Account,
                Pwd = user.Password,
                NickName = user.NickName,
                Phone = user.Phone,
                PlayCount = user.PlayCount,
                FansCount = user.FansCount,
                BB = user.BB,
                Picture = user.Picture,
                SubscribeNum = user.SubscribeNum,
                BannerImage = user.BannerImage,
                SkinId = user.SkinId,
                Level = user.Level,
                Bardian = user.Bardian,
                UseBB = user.UseBB,
                State = user.State,
                Token = _authKeysRepository.CreatePublicKey(user.Id, AuthUserType.General)
            };
        }
        #endregion


    }
}
