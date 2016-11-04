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
  
     public class DictionaryRepository : BaseRepository, IDictionaryRepository
    {
        public IQueryable<Dictionary> GetEntityList()
        {
            return base.GetEntityList<Dictionary>();
        }

        public IQueryable<Dictionary> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Dictionary>(orderCondtion);
        }

        public IQueryable<Dictionary> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Dictionary>(orderCondtions);
        }

        public IQueryable<Dictionary> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<Dictionary>(condtion);
        }

        public IQueryable<Dictionary> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Dictionary>(condtion, orderCondtion);
        }

        public IQueryable<Dictionary> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Dictionary>(condtion, orderCondtions);
        }

        public IQueryable<Dictionary> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<Dictionary>(condtions);
        }

        public IQueryable<Dictionary> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Dictionary>(condtions, orderCondtion);
        }

        public IQueryable<Dictionary> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Dictionary>(condtions, orderCondtions);
        }
        public Dictionary GetEntity(Condtion condtion)
        {
            return base.GetEntity<Dictionary>(condtion);
        }

        public Dictionary GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<Dictionary>(condtions);
        }
        public bool CreateEntity(Dictionary entity)
        {
            return base.CreateEntity<Dictionary>(entity);
        }

        public void CreateEntitys(IList<Dictionary> entitys)
        {
            base.CreateEntitys<Dictionary>(entitys);
        }

        public bool UpdateEntity(Dictionary entity)
        {
            return base.UpdateEntity<Dictionary>(entity);
        }

        public void UpdateEntitys(IList<Dictionary> entitys)
        {
            base.UpdateEntitys<Dictionary>(entitys);
        }

        public bool DeleteEntity(Dictionary entity)
        {
            return base.DeleteEntity<Dictionary>(entity);
        }

        public void DeleteEntitys(IList<Dictionary> entitys)
        {
            base.DeleteEntitys<Dictionary>(entitys);
        }
    }
}