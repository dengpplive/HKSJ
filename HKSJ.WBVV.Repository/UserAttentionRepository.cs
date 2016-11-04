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
  
     public class UserAttentionRepository : BaseRepository, IUserAttentionRepository
    {
        public IQueryable<UserAttention> GetEntityList()
        {
            return base.GetEntityList<UserAttention>();
        }

        public IQueryable<UserAttention> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserAttention>(orderCondtion);
        }

        public IQueryable<UserAttention> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserAttention>(orderCondtions);
        }

        public IQueryable<UserAttention> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserAttention>(condtion);
        }

        public IQueryable<UserAttention> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserAttention>(condtion, orderCondtion);
        }

        public IQueryable<UserAttention> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserAttention>(condtion, orderCondtions);
        }

        public IQueryable<UserAttention> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserAttention>(condtions);
        }

        public IQueryable<UserAttention> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserAttention>(condtions, orderCondtion);
        }

        public IQueryable<UserAttention> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserAttention>(condtions, orderCondtions);
        }
        public UserAttention GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserAttention>(condtion);
        }

        public UserAttention GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserAttention>(condtions);
        }
        public bool CreateEntity(UserAttention entity)
        {
            return base.CreateEntity<UserAttention>(entity);
        }

        public void CreateEntitys(IList<UserAttention> entitys)
        {
            base.CreateEntitys<UserAttention>(entitys);
        }

        public bool UpdateEntity(UserAttention entity)
        {
            return base.UpdateEntity<UserAttention>(entity);
        }

        public void UpdateEntitys(IList<UserAttention> entitys)
        {
            base.UpdateEntitys<UserAttention>(entitys);
        }

        public bool DeleteEntity(UserAttention entity)
        {
            return base.DeleteEntity<UserAttention>(entity);
        }

        public void DeleteEntitys(IList<UserAttention> entitys)
        {
            base.DeleteEntitys<UserAttention>(entitys);
        }
    }
}