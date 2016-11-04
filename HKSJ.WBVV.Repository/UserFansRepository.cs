using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Base;
using HKSJ.WBVV.Repository.Interface;

namespace HKSJ.WBVV.Repository
{

    public class UserFansRepository : BaseRepository, IUserFansRepository
    {
        public IQueryable<UserFans> GetEntityList()
        {
            return base.GetEntityList<UserFans>();
        }

        public IQueryable<UserFans> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserFans>(orderCondtion);
        }

        public IQueryable<UserFans> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserFans>(orderCondtions);
        }

        public IQueryable<UserFans> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserFans>(condtion);
        }

        public IQueryable<UserFans> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserFans>(condtion, orderCondtion);
        }

        public IQueryable<UserFans> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserFans>(condtion, orderCondtions);
        }

        public IQueryable<UserFans> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserFans>(condtions);
        }

        public IQueryable<UserFans> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserFans>(condtions, orderCondtion);
        }

        public IQueryable<UserFans> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserFans>(condtions, orderCondtions);
        }
        public UserFans GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserFans>(condtion);
        }

        public UserFans GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserFans>(condtions);
        }
        public bool CreateEntity(UserFans entity)
        {
            return base.CreateEntity<UserFans>(entity);
        }

        public void CreateEntitys(IList<UserFans> entitys)
        {
            base.CreateEntitys<UserFans>(entitys);
        }

        public bool UpdateEntity(UserFans entity)
        {
            return base.UpdateEntity<UserFans>(entity);
        }

        public void UpdateEntitys(IList<UserFans> entitys)
        {
            base.UpdateEntitys<UserFans>(entitys);
        }

        public bool DeleteEntity(UserFans entity)
        {
            return base.DeleteEntity<UserFans>(entity);
        }

        public void DeleteEntitys(IList<UserFans> entitys)
        {
            base.DeleteEntitys<UserFans>(entitys);
        }

        #region 关注用户
        /// <summary>
        /// 关注用户
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="subscribeUserId">关注用户编号</param>
        /// <returns></returns>
        public bool UserSubscribe(int loginUserId, int subscribeUserId)
        {
            var success = Execute<bool>((db) =>
            {
                var loginUser = db.User.FirstOrDefault(u => u.State == false && u.Id == loginUserId);
                AssertUtil.IsNotNull(loginUser, "登录用户不存在或者被禁用");
                var subscribeUser = db.User.FirstOrDefault(u => u.State == false && u.Id == subscribeUserId);
                AssertUtil.IsNotNull(subscribeUser, "关注用户不存在或者被禁用");
                AssertUtil.AreNotEqual(loginUser.Id, subscribeUser.Id,"不能关注自己");
                var userFans = db.UserFans.FirstOrDefault(uf => uf.CreateUserId == loginUser.Id && uf.SubscribeUserId == subscribeUser.Id);
                if (userFans == null)
                {
                    var newUserFans = new UserFans()
                    {
                        CreateTime = DateTime.Now,
                        CreateUserId = loginUser.Id,
                        SubscribeUserId = subscribeUser.Id
                    };
                    db.UserFans.Add(newUserFans);
                }
                else
                {
                    AssertUtil.IsTrue(userFans.State, "您已经关注了这个用户");
                    userFans.UpdateTime = DateTime.Now;
                    userFans.UpdateUserId = loginUser.Id;
                    userFans.State = false;
                    db.Entry(userFans).State = EntityState.Modified;
                }
                loginUser.SubscribeNum++;
                db.Entry(loginUser).State = EntityState.Modified;
                subscribeUser.FansCount++;
                db.Entry(subscribeUser).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserFans>();
                CreateCache<User>();
            }
            return success;
        }

        #endregion

        #region 取消关注
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="subscribeUserId">关注用户编号</param>
        /// <returns></returns>
        public bool UserCancelSubscribe(int loginUserId, int subscribeUserId)
        {
            var success = Execute<bool>((db) =>
            {
                var loginUser = db.User.FirstOrDefault(u => u.State == false && u.Id == loginUserId);
                AssertUtil.IsNotNull(loginUser, "登录用户不存在或者被禁用");
                var subscribeUser = db.User.FirstOrDefault(u => u.State == false && u.Id == subscribeUserId);
                AssertUtil.IsNotNull(subscribeUser, "关注用户不存在或者被禁用");
                AssertUtil.AreNotEqual(loginUser.Id, subscribeUser.Id, "自己不能取消关注自己");
                var userFans = db.UserFans.FirstOrDefault(uf => uf.State == false && uf.CreateUserId == loginUser.Id && uf.SubscribeUserId == subscribeUser.Id);
                AssertUtil.IsNotNull(userFans, "您还没有关注这个用户");
                AssertUtil.AreNotEqual(userFans.State, true, "您已经取消关注了");
                userFans.UpdateTime = DateTime.Now;
                userFans.UpdateUserId = loginUser.Id;
                userFans.State = true;
                db.Entry(userFans).State = EntityState.Modified;
                loginUser.SubscribeNum--;
                db.Entry(loginUser).State = EntityState.Modified;
                subscribeUser.FansCount--;
                db.Entry(subscribeUser).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserFans>();
                CreateCache<User>();
            }
            return success;
        }
        #endregion

        #region 订阅和取消订阅
        /// <summary>
        /// 订阅和取消订阅
        /// </summary>
        /// <param name="createUserId">登录的用户编号</param>
        /// <param name="subscribeUserId">订阅的用户编号</param>
        /// <param name="careState">订阅状态</param>
        /// <returns></returns>
        public bool UserSubscribeTransaction(int createUserId, int subscribeUserId, bool careState)
        {
            var success = Execute<bool>((db) =>
            {
                AssertUtil.AreBigger(createUserId, 0, "您还没有登录");
                AssertUtil.AreBigger(subscribeUserId, 0, "您订阅的用户不存在");
                var subscribeUser = db.User.FirstOrDefault(u => u.State == false && u.Id == createUserId);
                AssertUtil.IsNotNull(subscribeUser, "您还没有登录或者被禁用");
                var fansUser = db.User.FirstOrDefault(u => u.State == false && u.Id == subscribeUserId);
                AssertUtil.IsNotNull(subscribeUser, "您订阅的用户不存在或者被禁用");
                AssertUtil.AreNotEqual(createUserId, subscribeUserId, "用户不能自己订阅的自己");
                //用户粉丝
                var userFans = db.UserFans.FirstOrDefault(p => p.CreateUserId == createUserId && p.SubscribeUserId == subscribeUserId);

                if (userFans == null)
                {
                    //添加
                   var  newuserFans = new UserFans
                    {
                        CreateUserId = createUserId,
                        SubscribeUserId = subscribeUserId,
                        CreateTime = DateTime.Now,
                        State = careState
                    };
                    fansUser.FansCount += 1;
                    subscribeUser.SubscribeNum += 1;
                    db.UserFans.Add(newuserFans);
                }
                else
                {
                    if (careState)
                    {
                        fansUser.FansCount -= 1;
                        subscribeUser.SubscribeNum -= 1;
                    }
                    else
                    {
                        fansUser.FansCount += 1;
                        subscribeUser.SubscribeNum += 1;
                    }
                    //修改
                    userFans.UpdateTime =DateTime.Now;
                    userFans.UpdateUserId = createUserId;
                    userFans.State = careState;
                    db.Entry<UserFans>(userFans).State = EntityState.Modified;
                }
                db.Entry<User>(subscribeUser).State = EntityState.Modified;
                db.Entry<User>(fansUser).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserFans>();
                CreateCache<User>();
            }
            return success;
        }
        #endregion
    }
}