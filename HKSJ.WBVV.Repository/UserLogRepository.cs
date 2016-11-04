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
  
     public class UserLogRepository : BaseRepository, IUserLogRepository
    {
        public IQueryable<UserLog> GetEntityList()
        {
            return base.GetEntityList<UserLog>();
        }

        public IQueryable<UserLog> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserLog>(orderCondtion);
        }

        public IQueryable<UserLog> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserLog>(orderCondtions);
        }

        public IQueryable<UserLog> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserLog>(condtion);
        }

        public IQueryable<UserLog> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserLog>(condtion, orderCondtion);
        }

        public IQueryable<UserLog> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserLog>(condtion, orderCondtions);
        }

        public IQueryable<UserLog> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserLog>(condtions);
        }

        public IQueryable<UserLog> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserLog>(condtions, orderCondtion);
        }

        public IQueryable<UserLog> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserLog>(condtions, orderCondtions);
        }
        public UserLog GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserLog>(condtion);
        }

        public UserLog GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserLog>(condtions);
        }
        public bool CreateEntity(UserLog entity)
        {
            return base.CreateEntity<UserLog>(entity);
        }

        public void CreateEntitys(IList<UserLog> entitys)
        {
            base.CreateEntitys<UserLog>(entitys);
        }

        public bool UpdateEntity(UserLog entity)
        {
            return base.UpdateEntity<UserLog>(entity);
        }

        public void UpdateEntitys(IList<UserLog> entitys)
        {
            base.UpdateEntitys<UserLog>(entitys);
        }

        public bool DeleteEntity(UserLog entity)
        {
            return base.DeleteEntity<UserLog>(entity);
        }

        public void DeleteEntitys(IList<UserLog> entitys)
        {
            base.DeleteEntitys<UserLog>(entitys);
        }
    }
}