



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
  
     public class UserScoreRepository : BaseRepository, IUserScoreRepository
    {
        public IQueryable<UserScore> GetEntityList()
        {
            return base.GetEntityList<UserScore>();
        }

        public IQueryable<UserScore> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserScore>(orderCondtion);
        }

        public IQueryable<UserScore> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserScore>(orderCondtions);
        }

        public IQueryable<UserScore> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserScore>(condtion);
        }

        public IQueryable<UserScore> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserScore>(condtion, orderCondtion);
        }

        public IQueryable<UserScore> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserScore>(condtion, orderCondtions);
        }

        public IQueryable<UserScore> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserScore>(condtions);
        }

        public IQueryable<UserScore> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserScore>(condtions, orderCondtion);
        }

        public IQueryable<UserScore> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserScore>(condtions, orderCondtions);
        }
        public UserScore GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserScore>(condtion);
        }

        public UserScore GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserScore>(condtions);
        }
        public bool CreateEntity(UserScore entity)
        {
            return base.CreateEntity<UserScore>(entity);
        }

        public void CreateEntitys(IList<UserScore> entitys)
        {
            base.CreateEntitys(entitys);
        }

        public bool UpdateEntity(UserScore entity)
        {
            return base.UpdateEntity(entity);
        }

        public void UpdateEntitys(IList<UserScore> entitys)
        {
            base.UpdateEntitys<UserScore>(entitys);
        }

        public bool DeleteEntity(UserScore entity)
        {
            return base.DeleteEntity<UserScore>(entity);
        }

        public void DeleteEntitys(IList<UserScore> entitys)
        {
            base.DeleteEntitys<UserScore>(entitys);
        }
    }
}