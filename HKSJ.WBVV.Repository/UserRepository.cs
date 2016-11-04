using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HKSJ.Utilities;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Repository.Base;
using HKSJ.WBVV.Repository.Interface;

namespace HKSJ.WBVV.Repository
{
  
     public class UserRepository : BaseRepository, IUserRepository
    {
        public IQueryable<User> GetEntityList()
        {
            return base.GetEntityList<User>();
        }

        public IQueryable<User> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<User>(orderCondtion);
        }

        public IQueryable<User> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<User>(orderCondtions);
        }

        public IQueryable<User> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<User>(condtion);
        }

        public IQueryable<User> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<User>(condtion, orderCondtion);
        }

        public IQueryable<User> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<User>(condtion, orderCondtions);
        }

        public IQueryable<User> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<User>(condtions);
        }

        public IQueryable<User> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<User>(condtions, orderCondtion);
        }

        public IQueryable<User> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<User>(condtions, orderCondtions);
        }
        public User GetEntity(Condtion condtion)
        {
            return base.GetEntity<User>(condtion);
        }

        public User GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<User>(condtions);
        }
        public bool CreateEntity(User entity)
        {
            return base.CreateEntity<User>(entity);
        }

        public void CreateEntitys(IList<User> entitys)
        {
            base.CreateEntitys<User>(entitys);
        }

        public bool UpdateEntity(User entity)
        {
            return base.UpdateEntity<User>(entity);
        }

        public void UpdateEntitys(IList<User> entitys)
        {
            base.UpdateEntitys<User>(entitys);
        }

        public bool DeleteEntity(User entity)
        {
            return base.DeleteEntity<User>(entity);
        }

        public void DeleteEntitys(IList<User> entitys)
        {
            base.DeleteEntitys<User>(entitys);
        }

        #region 用户登录

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="password">密码</param>
        /// <param name="ipAddress">ip地址</param>
        public void IncomeLogin(string account, string password, string ipAddress)
        {
            var success = Execute(db =>
            {
                account = account.UrlDecode();
                password = password.UrlDecode();
                AssertUtil.NotNullOrWhiteSpace(account, "用户账号为空");
                AssertUtil.NotNullOrWhiteSpace(password, "用户密码为空");
                AssertUtil.NotNullOrWhiteSpace(ipAddress, "你想怎么样");
                var user = db.User.FirstOrDefault(u => u.State == false && u.Password == password && (u.Phone == account || u.Account == account));
                if (user == null || user.Id < 1)
                {
                    user = db.User.FirstOrDefault(u => u.State == false && u.Password == password && (u.Email == account || u.Account == account));
                }
                AssertUtil.IsNotNull(user, "账号不存在或密码错误");
                //上次登录时间和ip地址
                DateTime? lastLoginTime = user.CurrentLoginTime;
                string lastLoginIp = user.CurrentLoginIp;

                //记录新的当前登录事件和上次登录时间
                user.LastLoginIp = lastLoginIp;
                user.LastLoginTime = lastLoginTime;
                user.CurrentLoginTime = DateTime.Now;
                user.CurrentLoginIp = ipAddress;

                db.Entry<User>(user).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<User>();
            }
        }

        #endregion

        #region 用户注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        public void IncomeRegister(string account, string password, string ipAddress, int type)
        {
            var success = Execute<bool>((db) =>
            {
                account = account.UrlDecode();
                password = password.UrlDecode();
                AssertUtil.NotNullOrWhiteSpace(account, "用户账号为空");
                AssertUtil.NotNullOrWhiteSpace(password, "用户密码为空");
                var userAccount = db.User.FirstOrDefault(u => u.Account == account);
                AssertUtil.IsNull(userAccount, "账号已经存在");

                var user = new User
                {
                    Account = account,
                    SubscribeNum = 0,
                    UserState = 0,
                    BB = 0,
                    Gender = false,
                    PlayCount = 0,
                    FansCount = 0,
                    State = false,
                    Password = Md5Helper.MD5(password, 32),//TODO 用户密码 MD5加密
                    CreateTime = DateTime.Now,
                    Level = 0,
                    UseBB = 0
                };
                if (type == 0)
                {
                    user.Phone = account;
                }
                else
                {
                    user.Email = account;
                }
                db.User.Add(user);
                var result = db.SaveChanges() > 0;
                if (result)
                {
                    user.NickName = "播客_" + user.Id;
                    db.Entry(user).State = EntityState.Modified;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    result = db.SaveChanges() > 0;
                }
                return result;
            });
            if (success)
            {
                CreateCache<User>();
            }
        }
        #endregion

        #region 第三方绑定已有帐号并登录
        /// <summary>
        /// 第三方绑定已有帐号并登录
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="password">密码</param>
        /// <param name="typeCode">第三方类型编码</param>
        /// <param name="relatedId">第三方唯一身份标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="figureURL">头像</param>
        /// <param name="ipAddress">ip地址</param>
        public void ThirdPartyBindAndLogin(string account, string password, string typeCode, string relatedId, string nickName, string figureURL, string ipAddress)
        {
            var success = Execute((db) =>
            {
                account = account.UrlDecode();
                password = password.UrlDecode();
                typeCode = typeCode.UrlDecode();
                relatedId = relatedId.UrlDecode();
                nickName = nickName.UrlDecode();
                figureURL = figureURL.UrlDecode();

                AssertUtil.NotNullOrWhiteSpace(account, "用户账号为空");
                AssertUtil.NotNullOrWhiteSpace(password, "用户密码为空");
                AssertUtil.NotNullOrWhiteSpace(typeCode, "第三方类型编码丢失,请稍后再试");
                AssertUtil.NotNullOrWhiteSpace(relatedId, "第三方身份标识丢失,请稍后再试");
                AssertUtil.NotNullOrWhiteSpace(nickName, "参数丢失，请稍后再试");
                AssertUtil.NotNullOrWhiteSpace(figureURL, "参数丢失，请稍后再试");
                AssertUtil.NotNullOrWhiteSpace(ipAddress, "请稍后再试");

                var user = db.User.FirstOrDefault(u => u.State == false && u.Password == password && (u.Phone == account || u.Account == account));
                if (user == null || user.Id < 1)
                {
                    user = db.User.FirstOrDefault(u => u.State == false && u.Password == password && (u.Email == account || u.Account == account));
                }
                AssertUtil.IsNotNull(user, "账号不存在或密码错误");

                //检测是否已绑定
                var userBind = db.UserBind.FirstOrDefault(u => u.State == false && u.TypeCode == typeCode && u.UserId == user.Id && u.RelatedId == relatedId);
                if (userBind == null)
                {
                    //检测该帐号是否绑定其他第三方帐号
                    if (db.UserBind.Any(u => u.State == false && u.TypeCode == typeCode && u.UserId == user.Id && u.RelatedId != relatedId))
                        AssertUtil.IsTrue(false, "该帐号已被绑定，请换个帐号试试...");
                    //检测第三方帐号是否绑定其他帐号
                    if (db.UserBind.Any(u => u.State == false && u.TypeCode == typeCode && u.UserId != user.Id && u.RelatedId == relatedId))
                        AssertUtil.IsTrue(false, "第三方帐号已绑定其他帐号，请换个帐号试试...");

                    //信息没有修改的情况下，拉取第三方昵称，头像等信息
                    if (user.UpdateTime.IsNull() || user.CreateTime >= user.UpdateTime)
                    {
                        user.NickName = nickName;
                        user.Picture = figureURL;
                    }

                    DateTime dt = DateTime.Now;
                    //绑定第三方帐号
                    UserBind ub = new UserBind
                    {
                        UserId = user.Id,
                        TypeCode = typeCode,
                        RelatedId = relatedId,
                        CreateUserId = user.Id,
                        CreateTime = dt,
                        UpdateUserId = user.Id,
                        UpdateTime = dt,
                        State = false
                    };

                    user.UpdateTime = dt;

                    db.UserBind.Add(ub);

                    db.SaveChanges();
                }



                //上次登录时间和ip地址
                DateTime? lastLoginTime = user.CurrentLoginTime;
                string lastLoginIp = user.CurrentLoginIp;

                //记录新的当前登录事件和上次登录时间
                user.LastLoginIp = lastLoginIp;
                user.LastLoginTime = lastLoginTime;
                user.CurrentLoginTime = DateTime.Now;
                user.CurrentLoginIp = ipAddress;

                db.Entry<User>(user).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<User>();
                CreateCache<UserBind>();
            }
        }
        #endregion

        #region 第三方注册新帐号并绑定
        /// <summary>
        /// 第三方注册新帐号并绑定
        /// </summary>
        /// <returns></returns>
        public void ThirdPartyBindAndRegister(string account, string password, string typeCode, string relatedId, string nickName, string figureURL, int type, string ipAddress)
        {
            var success = Execute<bool>((db) =>
            {
                account = account.UrlDecode();
                password = password.UrlDecode();
                typeCode = typeCode.UrlDecode();
                relatedId = relatedId.UrlDecode();
                nickName = nickName.UrlDecode();
                figureURL = figureURL.UrlDecode();

                AssertUtil.NotNullOrWhiteSpace(account, "用户账号为空");
                AssertUtil.NotNullOrWhiteSpace(password, "用户密码为空");
                AssertUtil.NotNullOrWhiteSpace(typeCode, "第三方类型编码丢失,请稍后再试");
                AssertUtil.NotNullOrWhiteSpace(relatedId, "第三方身份标识丢失,请稍后再试");
                AssertUtil.NotNullOrWhiteSpace(nickName, "参数丢失，请稍后再试");
                AssertUtil.NotNullOrWhiteSpace(figureURL, "参数丢失，请稍后再试");

                var userAccount = db.User.FirstOrDefault(u => u.Account == account || u.Phone == account || u.Email == account);
                AssertUtil.IsNull(userAccount, "账号已经存在");

                DateTime dt = DateTime.Now;

                var user = new User
                {
                    Account = account,
                    SubscribeNum = 0,
                    UserState = 0,
                    BB = 0,
                    Gender = false,
                    PlayCount = 0,
                    FansCount = 0,
                    State = false,
                    Password = Md5Helper.MD5(password, 32),//TODO 用户密码 MD5加密
                    CreateTime = dt,
                    UpdateTime = dt,
                    Level = 0,
                    UseBB = 0,
                    NickName = nickName,
                    Picture = figureURL
                };

                if (type == 0)
                {
                    user.Phone = account;
                }
                else
                {
                    user.Email = account;
                }

                db.User.Add(user);
                var result = db.SaveChanges() > 0;

                if (result)
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    result = db.SaveChanges() > 0;
                }

                //绑定第三方，新帐号不用做绑定判断，判断第三方是否已被绑定后写入关系

                //检测第三方帐号是否绑定其他帐号
                if (db.UserBind.Any(u => u.State == false && u.TypeCode == typeCode && u.UserId != user.Id && u.RelatedId == relatedId))
                    AssertUtil.IsTrue(false, "第三方帐号已绑定其他帐号，请换个帐号试试...");
                //绑定第三方帐号
                var ub = new UserBind
                {
                    UserId = user.Id,
                    TypeCode = typeCode,
                    RelatedId = relatedId,
                    CreateUserId = user.Id,
                    CreateTime = dt,
                    UpdateUserId = user.Id,
                    UpdateTime = dt,
                    State = false
                };
                db.UserBind.Add(ub);
                db.SaveChanges();

                return result;
            });
            if (success)
            {
                CreateCache<User>();
                CreateCache<UserBind>();
            }
        }
        #endregion

        #region 自动注册并绑定第三方
        /// <summary>
        /// 自动注册并绑定第三方
        /// </summary>
        /// <returns></returns>
        public void AutoRegisterAndBindThirdParty(string typeCode, string relatedId, string nickName, string figureURL, string ipAddress)
        {
            var success = Execute<bool>((db) =>
            {

                typeCode = typeCode.UrlDecode();
                relatedId = relatedId.UrlDecode();
                nickName = nickName.UrlDecode();
                figureURL = figureURL.UrlDecode();

                AssertUtil.NotNullOrWhiteSpace(typeCode, "第三方类型编码丢失,请稍后再试");
                AssertUtil.NotNullOrWhiteSpace(relatedId, "第三方身份标识丢失,请稍后再试");
                AssertUtil.NotNullOrWhiteSpace(nickName, "参数丢失，请稍后再试");
                AssertUtil.NotNullOrWhiteSpace(figureURL, "参数丢失，请稍后再试");

                DateTime dt = DateTime.Now;
                Random rdm = new Random();
                var user = new User
                {
                    Account = "AutoRegist" + rdm.Next(50000, 999999999).ToString(),
                    Phone = "",
                    SubscribeNum = 0,
                    UserState = 0,
                    BB = 0,
                    Gender = false,
                    PlayCount = 0,
                    FansCount = 0,
                    State = false,
                    Password = Md5Helper.MD5("AutoRegist" + rdm.Next(50000, 999999999).ToString(), 32),
                    CreateTime = dt,
                    UpdateTime = dt,
                    Level = 0,
                    NickName = nickName,
                    Picture = figureURL,
                    UseBB = 0
                };
                db.User.Add(user);
                var result = db.SaveChanges() > 0;
                //绑定第三方，新帐号不用做绑定判断，判断第三方是否已被绑定后写入关系

                //检测第三方帐号是否绑定其他帐号
                if (db.UserBind.Any(u => u.State == false && u.TypeCode == typeCode && u.UserId != user.Id && u.RelatedId == relatedId))
                    AssertUtil.IsTrue(false, "第三方帐号已绑定其他帐号，请换个帐号试试...");
                //绑定第三方帐号
                var ub = new UserBind
                {
                    UserId = user.Id,
                    TypeCode = typeCode,
                    RelatedId = relatedId,
                    CreateUserId = user.Id,
                    CreateTime = dt,
                    UpdateUserId = user.Id,
                    UpdateTime = dt,
                    State = false
                };
                db.UserBind.Add(ub);
                db.SaveChanges();

                return result;
            });
            if (success)
            {
                CreateCache<User>();
                CreateCache<UserBind>();
            }
        }
        #endregion

        #region 通过ID登录，一般用于SSO
        /// <summary>
        /// 通过ID登录，一般用于SSO
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ipAddress"></param>
        public void IncomeLogin(int userId, string ipAddress)
        {
            var success = Execute(db =>
            {

                AssertUtil.AreBigger(userId, 0, "参数错误，你想怎么样");
                AssertUtil.NotNullOrWhiteSpace(ipAddress, "你想怎么样");
                var user = db.User.FirstOrDefault(u => u.State == false && u.Id == userId);

                AssertUtil.IsNotNull(user, "账号不存在或密码错误");
                //上次登录时间和ip地址
                DateTime? lastLoginTime = user.CurrentLoginTime;
                string lastLoginIp = user.CurrentLoginIp;

                //记录新的当前登录事件和上次登录时间
                user.LastLoginIp = lastLoginIp;
                user.LastLoginTime = lastLoginTime;
                user.CurrentLoginTime = DateTime.Now;
                user.CurrentLoginIp = ipAddress;

                db.Entry<User>(user).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<User>();
            }
        }
        #endregion
    }
}