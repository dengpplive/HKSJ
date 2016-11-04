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
  
     public class AccusationRepository : BaseRepository, IAccusationRepository
    {
        public IQueryable<Accusation> GetEntityList()
        {
            return base.GetEntityList<Accusation>();
        }

        public IQueryable<Accusation> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Accusation>(orderCondtion);
        }

        public IQueryable<Accusation> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Accusation>(orderCondtions);
        }

        public IQueryable<Accusation> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<Accusation>(condtion);
        }

        public IQueryable<Accusation> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Accusation>(condtion, orderCondtion);
        }

        public IQueryable<Accusation> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Accusation>(condtion, orderCondtions);
        }

        public IQueryable<Accusation> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<Accusation>(condtions);
        }

        public IQueryable<Accusation> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Accusation>(condtions, orderCondtion);
        }

        public IQueryable<Accusation> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Accusation>(condtions, orderCondtions);
        }
        public Accusation GetEntity(Condtion condtion)
        {
            return base.GetEntity<Accusation>(condtion);
        }

        public Accusation GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<Accusation>(condtions);
        }
        public bool CreateEntity(Accusation entity)
        {
            return base.CreateEntity<Accusation>(entity);
        }

        public void CreateEntitys(IList<Accusation> entitys)
        {
            base.CreateEntitys<Accusation>(entitys);
        }

        public bool UpdateEntity(Accusation entity)
        {
            return base.UpdateEntity<Accusation>(entity);
        }

        public void UpdateEntitys(IList<Accusation> entitys)
        {
            base.UpdateEntitys<Accusation>(entitys);
        }

        public bool DeleteEntity(Accusation entity)
        {
            return base.DeleteEntity<Accusation>(entity);
        }

        public void DeleteEntitys(IList<Accusation> entitys)
        {
            base.DeleteEntitys<Accusation>(entitys);
        }
    }
}