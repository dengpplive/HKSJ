using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Base;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Entity.Tables;

namespace HKSJ.WBVV.Repository
{

    public class UserSkinRepository : BaseRepository, IUserSkinRepository
    {
        public IQueryable<UserSkin> GetEntityList()
        {
            return base.GetEntityList<UserSkin>();
        }

        public IQueryable<UserSkin> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserSkin>(orderCondtion);
        }

        public IQueryable<UserSkin> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserSkin>(orderCondtions);
        }

        public IQueryable<UserSkin> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserSkin>(condtion);
        }

        public IQueryable<UserSkin> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserSkin>(condtion, orderCondtion);
        }

        public IQueryable<UserSkin> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserSkin>(condtion, orderCondtions);
        }

        public IQueryable<UserSkin> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserSkin>(condtions);
        }

        public IQueryable<UserSkin> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserSkin>(condtions, orderCondtion);
        }

        public IQueryable<UserSkin> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserSkin>(condtions, orderCondtions);
        }
        public UserSkin GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserSkin>(condtion);
        }

        public UserSkin GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserSkin>(condtions);
        }
        public bool CreateEntity(UserSkin entity)
        {
            return base.CreateEntity<UserSkin>(entity);
        }

        public void CreateEntitys(IList<UserSkin> entitys)
        {
            base.CreateEntitys<UserSkin>(entitys);
        }

        public bool UpdateEntity(UserSkin entity)
        {
            return base.UpdateEntity<UserSkin>(entity);
        }

        public void UpdateEntitys(IList<UserSkin> entitys)
        {
            base.UpdateEntitys<UserSkin>(entitys);
        }

        public bool DeleteEntity(UserSkin entity)
        {
            return base.DeleteEntity<UserSkin>(entity);
        }

        public void DeleteEntitys(IList<UserSkin> entitys)
        {
            base.DeleteEntitys<UserSkin>(entitys);
        }
    }
}