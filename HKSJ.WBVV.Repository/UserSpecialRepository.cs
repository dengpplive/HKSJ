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
  
     public class UserSpecialRepository : BaseRepository, IUserSpecialRepository
    {
        public IQueryable<UserSpecial> GetEntityList()
        {
            return base.GetEntityList<UserSpecial>();
        }

        public IQueryable<UserSpecial> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserSpecial>(orderCondtion);
        }

        public IQueryable<UserSpecial> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserSpecial>(orderCondtions);
        }

        public IQueryable<UserSpecial> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserSpecial>(condtion);
        }

        public IQueryable<UserSpecial> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserSpecial>(condtion, orderCondtion);
        }

        public IQueryable<UserSpecial> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserSpecial>(condtion, orderCondtions);
        }

        public IQueryable<UserSpecial> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserSpecial>(condtions);
        }

        public IQueryable<UserSpecial> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserSpecial>(condtions, orderCondtion);
        }

        public IQueryable<UserSpecial> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserSpecial>(condtions, orderCondtions);
        }
        public UserSpecial GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserSpecial>(condtion);
        }

        public UserSpecial GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserSpecial>(condtions);
        }
        public bool CreateEntity(UserSpecial entity)
        {
            return base.CreateEntity<UserSpecial>(entity);
        }

        public void CreateEntitys(IList<UserSpecial> entitys)
        {
            base.CreateEntitys<UserSpecial>(entitys);
        }

        public bool UpdateEntity(UserSpecial entity)
        {
            return base.UpdateEntity<UserSpecial>(entity);
        }

        public void UpdateEntitys(IList<UserSpecial> entitys)
        {
            base.UpdateEntitys<UserSpecial>(entitys);
        }

        public bool DeleteEntity(UserSpecial entity)
        {
            return base.DeleteEntity<UserSpecial>(entity);
        }

        public void DeleteEntitys(IList<UserSpecial> entitys)
        {
            base.DeleteEntitys<UserSpecial>(entitys);
        }
    }
}