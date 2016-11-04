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
  
     public class UserVisitLogRepository : BaseRepository, IUserVisitLogRepository
    {
        public IQueryable<UserVisitLog> GetEntityList()
        {
            return base.GetEntityList<UserVisitLog>();
        }

        public IQueryable<UserVisitLog> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserVisitLog>(orderCondtion);
        }

        public IQueryable<UserVisitLog> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserVisitLog>(orderCondtions);
        }

        public IQueryable<UserVisitLog> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserVisitLog>(condtion);
        }

        public IQueryable<UserVisitLog> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserVisitLog>(condtion, orderCondtion);
        }

        public IQueryable<UserVisitLog> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserVisitLog>(condtion, orderCondtions);
        }

        public IQueryable<UserVisitLog> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserVisitLog>(condtions);
        }

        public IQueryable<UserVisitLog> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserVisitLog>(condtions, orderCondtion);
        }

        public IQueryable<UserVisitLog> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserVisitLog>(condtions, orderCondtions);
        }
        public UserVisitLog GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserVisitLog>(condtion);
        }

        public UserVisitLog GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserVisitLog>(condtions);
        }
        public bool CreateEntity(UserVisitLog entity)
        {
            return base.CreateEntity<UserVisitLog>(entity);
        }

        public void CreateEntitys(IList<UserVisitLog> entitys)
        {
            base.CreateEntitys<UserVisitLog>(entitys);
        }

        public bool UpdateEntity(UserVisitLog entity)
        {
            return base.UpdateEntity<UserVisitLog>(entity);
        }

        public void UpdateEntitys(IList<UserVisitLog> entitys)
        {
            base.UpdateEntitys<UserVisitLog>(entitys);
        }

        public bool DeleteEntity(UserVisitLog entity)
        {
            return base.DeleteEntity<UserVisitLog>(entity);
        }

        public void DeleteEntitys(IList<UserVisitLog> entitys)
        {
            base.DeleteEntitys<UserVisitLog>(entitys);
        }
    }
}