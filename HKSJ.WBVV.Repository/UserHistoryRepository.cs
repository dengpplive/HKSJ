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
  
     public class UserHistoryRepository : BaseRepository, IUserHistoryRepository
    {
        public IQueryable<UserHistory> GetEntityList()
        {
            return base.GetEntityList<UserHistory>();
        }

        public IQueryable<UserHistory> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserHistory>(orderCondtion);
        }

        public IQueryable<UserHistory> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserHistory>(orderCondtions);
        }

        public IQueryable<UserHistory> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserHistory>(condtion);
        }

        public IQueryable<UserHistory> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserHistory>(condtion, orderCondtion);
        }

        public IQueryable<UserHistory> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserHistory>(condtion, orderCondtions);
        }

        public IQueryable<UserHistory> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserHistory>(condtions);
        }

        public IQueryable<UserHistory> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserHistory>(condtions, orderCondtion);
        }

        public IQueryable<UserHistory> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserHistory>(condtions, orderCondtions);
        }
        public UserHistory GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserHistory>(condtion);
        }

        public UserHistory GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserHistory>(condtions);
        }
        public bool CreateEntity(UserHistory entity)
        {
            return base.CreateEntity<UserHistory>(entity);
        }

        public void CreateEntitys(IList<UserHistory> entitys)
        {
            base.CreateEntitys<UserHistory>(entitys);
        }

        public bool UpdateEntity(UserHistory entity)
        {
            return base.UpdateEntity<UserHistory>(entity);
        }

        public void UpdateEntitys(IList<UserHistory> entitys)
        {
            base.UpdateEntitys<UserHistory>(entitys);
        }

        public bool DeleteEntity(UserHistory entity)
        {
            return base.DeleteEntity<UserHistory>(entity);
        }

        public void DeleteEntitys(IList<UserHistory> entitys)
        {
            base.DeleteEntitys<UserHistory>(entitys);
        }
    }
}