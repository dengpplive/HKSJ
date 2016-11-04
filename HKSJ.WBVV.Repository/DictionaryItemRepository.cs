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
  
     public class DictionaryItemRepository : BaseRepository, IDictionaryItemRepository
    {
        public IQueryable<DictionaryItem> GetEntityList()
        {
            return base.GetEntityList<DictionaryItem>();
        }

        public IQueryable<DictionaryItem> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<DictionaryItem>(orderCondtion);
        }

        public IQueryable<DictionaryItem> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<DictionaryItem>(orderCondtions);
        }

        public IQueryable<DictionaryItem> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<DictionaryItem>(condtion);
        }

        public IQueryable<DictionaryItem> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<DictionaryItem>(condtion, orderCondtion);
        }

        public IQueryable<DictionaryItem> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<DictionaryItem>(condtion, orderCondtions);
        }

        public IQueryable<DictionaryItem> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<DictionaryItem>(condtions);
        }

        public IQueryable<DictionaryItem> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<DictionaryItem>(condtions, orderCondtion);
        }

        public IQueryable<DictionaryItem> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<DictionaryItem>(condtions, orderCondtions);
        }
        public DictionaryItem GetEntity(Condtion condtion)
        {
            return base.GetEntity<DictionaryItem>(condtion);
        }

        public DictionaryItem GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<DictionaryItem>(condtions);
        }
        public bool CreateEntity(DictionaryItem entity)
        {
            return base.CreateEntity<DictionaryItem>(entity);
        }

        public void CreateEntitys(IList<DictionaryItem> entitys)
        {
            base.CreateEntitys<DictionaryItem>(entitys);
        }

        public bool UpdateEntity(DictionaryItem entity)
        {
            return base.UpdateEntity<DictionaryItem>(entity);
        }

        public void UpdateEntitys(IList<DictionaryItem> entitys)
        {
            base.UpdateEntitys<DictionaryItem>(entitys);
        }

        public bool DeleteEntity(DictionaryItem entity)
        {
            return base.DeleteEntity<DictionaryItem>(entity);
        }

        public void DeleteEntitys(IList<DictionaryItem> entitys)
        {
            base.DeleteEntitys<DictionaryItem>(entitys);
        }
    }
}