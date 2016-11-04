using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Base;
using HKSJ.WBVV.Repository.Interface;

namespace HKSJ.WBVV.Repository
{

    public class UserBindRepository : BaseRepository, IUserBindRepository
    {
        public IQueryable<UserBind> GetEntityList()
        {
            return base.GetEntityList<UserBind>();
        }

        public IQueryable<UserBind> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserBind>(orderCondtion);
        }

        public IQueryable<UserBind> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserBind>(orderCondtions);
        }

        public IQueryable<UserBind> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserBind>(condtion);
        }

        public IQueryable<UserBind> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserBind>(condtion, orderCondtion);
        }

        public IQueryable<UserBind> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserBind>(condtion, orderCondtions);
        }

        public IQueryable<UserBind> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserBind>(condtions);
        }

        public IQueryable<UserBind> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserBind>(condtions, orderCondtion);
        }

        public IQueryable<UserBind> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserBind>(condtions, orderCondtions);
        }
        public UserBind GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserBind>(condtion);
        }

        public UserBind GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserBind>(condtions);
        }
        public bool CreateEntity(UserBind entity)
        {
            return base.CreateEntity<UserBind>(entity);
        }

        public void CreateEntitys(IList<UserBind> entitys)
        {
            base.CreateEntitys<UserBind>(entitys);
        }

        public bool UpdateEntity(UserBind entity)
        {
            return base.UpdateEntity<UserBind>(entity);
        }

        public void UpdateEntitys(IList<UserBind> entitys)
        {
            base.UpdateEntitys<UserBind>(entitys);
        }

        public bool DeleteEntity(UserBind entity)
        {
            return base.DeleteEntity<UserBind>(entity);
        }

        public void DeleteEntitys(IList<UserBind> entitys)
        {
            base.DeleteEntitys<UserBind>(entitys);
        }
    }
}