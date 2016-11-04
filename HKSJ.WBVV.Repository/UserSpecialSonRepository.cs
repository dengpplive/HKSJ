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
  
     public class UserSpecialSonRepository : BaseRepository, IUserSpecialSonRepository
    {
        public IQueryable<UserSpecialSon> GetEntityList()
        {
            return base.GetEntityList<UserSpecialSon>();
        }

        public IQueryable<UserSpecialSon> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserSpecialSon>(orderCondtion);
        }

        public IQueryable<UserSpecialSon> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserSpecialSon>(orderCondtions);
        }

        public IQueryable<UserSpecialSon> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserSpecialSon>(condtion);
        }

        public IQueryable<UserSpecialSon> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserSpecialSon>(condtion, orderCondtion);
        }

        public IQueryable<UserSpecialSon> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserSpecialSon>(condtion, orderCondtions);
        }

        public IQueryable<UserSpecialSon> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserSpecialSon>(condtions);
        }

        public IQueryable<UserSpecialSon> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserSpecialSon>(condtions, orderCondtion);
        }

        public IQueryable<UserSpecialSon> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserSpecialSon>(condtions, orderCondtions);
        }
        public UserSpecialSon GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserSpecialSon>(condtion);
        }

        public UserSpecialSon GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserSpecialSon>(condtions);
        }
        public bool CreateEntity(UserSpecialSon entity)
        {
            return base.CreateEntity<UserSpecialSon>(entity);
        }

        public void CreateEntitys(IList<UserSpecialSon> entitys)
        {
            base.CreateEntitys<UserSpecialSon>(entitys);
        }

        public bool UpdateEntity(UserSpecialSon entity)
        {
            return base.UpdateEntity<UserSpecialSon>(entity);
        }

        public void UpdateEntitys(IList<UserSpecialSon> entitys)
        {
            base.UpdateEntitys<UserSpecialSon>(entitys);
        }

        public bool DeleteEntity(UserSpecialSon entity)
        {
            return base.DeleteEntity<UserSpecialSon>(entity);
        }

        public void DeleteEntitys(IList<UserSpecialSon> entitys)
        {
            base.DeleteEntitys<UserSpecialSon>(entitys);
        }
    }
}