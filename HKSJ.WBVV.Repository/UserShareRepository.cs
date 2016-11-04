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
  
     public class UserShareRepository : BaseRepository, IUserShareRepository
    {
        public IQueryable<UserShare> GetEntityList()
        {
            return base.GetEntityList<UserShare>();
        }

        public IQueryable<UserShare> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserShare>(orderCondtion);
        }

        public IQueryable<UserShare> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserShare>(orderCondtions);
        }

        public IQueryable<UserShare> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserShare>(condtion);
        }

        public IQueryable<UserShare> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserShare>(condtion, orderCondtion);
        }

        public IQueryable<UserShare> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserShare>(condtion, orderCondtions);
        }

        public IQueryable<UserShare> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserShare>(condtions);
        }

        public IQueryable<UserShare> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserShare>(condtions, orderCondtion);
        }

        public IQueryable<UserShare> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserShare>(condtions, orderCondtions);
        }
        public UserShare GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserShare>(condtion);
        }

        public UserShare GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserShare>(condtions);
        }
        public bool CreateEntity(UserShare entity)
        {
            return base.CreateEntity<UserShare>(entity);
        }

        public void CreateEntitys(IList<UserShare> entitys)
        {
            base.CreateEntitys<UserShare>(entitys);
        }

        public bool UpdateEntity(UserShare entity)
        {
            return base.UpdateEntity<UserShare>(entity);
        }

        public void UpdateEntitys(IList<UserShare> entitys)
        {
            base.UpdateEntitys<UserShare>(entitys);
        }

        public bool DeleteEntity(UserShare entity)
        {
            return base.DeleteEntity<UserShare>(entity);
        }

        public void DeleteEntitys(IList<UserShare> entitys)
        {
            base.DeleteEntitys<UserShare>(entitys);
        }
    }
}