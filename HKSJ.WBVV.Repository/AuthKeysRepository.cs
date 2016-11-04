using System;
using System.Collections.Generic;
using System.Linq;
using HKSJ.Utilities.Base.Security;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Repository.Base;
using HKSJ.WBVV.Repository.Interface;

namespace HKSJ.WBVV.Repository
{

    public class AuthKeysRepository : BaseRepository, IAuthKeysRepository
    {
        /// <summary>
        /// 生成用户的公钥,返回加密UserId的加密串
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public string CreatePublicKey(int uid, AuthUserType userType)
        {
            var encryptStr = Execute(db =>
            {
                var authKey = db.AuthKeys.FirstOrDefault(u => u.UserId == uid && u.UserType == (int)userType);
                if (authKey != null) return RSAHelper.EncryptString(uid.ToString(), authKey.PublicKey);
                var keyPair = RSAHelper.GetRASKey();
                authKey = new AuthKeys
                {
                    UserId = uid,
                    PublicKey = keyPair.PublicKey,
                    PrivateKey = keyPair.PrivateKey,
                    UserType = (int)userType,
                    CreateTime = DateTime.Now
                };
                db.AuthKeys.Add(authKey);
                db.SaveChanges();
                return RSAHelper.EncryptString(uid.ToString(), authKey.PublicKey);
            });
            CreateCache<AuthKeys>();
            return encryptStr;
        }

        public IQueryable<AuthKeys> GetEntityList()
        {
            return base.GetEntityList<AuthKeys>();
        }

        public IQueryable<AuthKeys> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<AuthKeys>(orderCondtion);
        }

        public IQueryable<AuthKeys> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<AuthKeys>(orderCondtions);
        }

        public IQueryable<AuthKeys> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<AuthKeys>(condtion);
        }

        public IQueryable<AuthKeys> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<AuthKeys>(condtion, orderCondtion);
        }

        public IQueryable<AuthKeys> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<AuthKeys>(condtion, orderCondtions);
        }

        public IQueryable<AuthKeys> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<AuthKeys>(condtions);
        }

        public IQueryable<AuthKeys> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<AuthKeys>(condtions, orderCondtion);
        }

        public IQueryable<AuthKeys> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<AuthKeys>(condtions, orderCondtions);
        }

        public AuthKeys GetEntity(Condtion condtion)
        {
            return base.GetEntity<AuthKeys>(condtion);
        }

        public AuthKeys GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<AuthKeys>(condtions);
        }

        public bool CreateEntity(AuthKeys entity)
        {
            return base.CreateEntity(entity);
        }

        public void CreateEntitys(IList<AuthKeys> entitys)
        {
            base.CreateEntitys(entitys);
        }

        public bool UpdateEntity(AuthKeys entity)
        {
            return base.UpdateEntity(entity);
        }

        public void UpdateEntitys(IList<AuthKeys> entitys)
        {
            base.UpdateEntitys(entitys);
        }

        public bool DeleteEntity(AuthKeys entity)
        {
            return base.DeleteEntity(entity);
        }

        public void DeleteEntitys(IList<AuthKeys> entitys)
        {
            base.DeleteEntitys(entitys);
        }
    }
}