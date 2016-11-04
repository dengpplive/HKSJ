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
  
     public class KeyWordsRepository : BaseRepository, IKeyWordsRepository
    {
        public IQueryable<KeyWords> GetEntityList()
        {
            return base.GetEntityList<KeyWords>();
        }

        public IQueryable<KeyWords> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<KeyWords>(orderCondtion);
        }

        public IQueryable<KeyWords> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<KeyWords>(orderCondtions);
        }

        public IQueryable<KeyWords> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<KeyWords>(condtion);
        }

        public IQueryable<KeyWords> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<KeyWords>(condtion, orderCondtion);
        }

        public IQueryable<KeyWords> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<KeyWords>(condtion, orderCondtions);
        }

        public IQueryable<KeyWords> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<KeyWords>(condtions);
        }

        public IQueryable<KeyWords> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<KeyWords>(condtions, orderCondtion);
        }

        public IQueryable<KeyWords> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<KeyWords>(condtions, orderCondtions);
        }
        public KeyWords GetEntity(Condtion condtion)
        {
            return base.GetEntity<KeyWords>(condtion);
        }

        public KeyWords GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<KeyWords>(condtions);
        }
        public bool CreateEntity(KeyWords entity)
        {
            return base.CreateEntity<KeyWords>(entity);
        }

        public void CreateEntitys(IList<KeyWords> entitys)
        {
            base.CreateEntitys<KeyWords>(entitys);
        }

        public bool UpdateEntity(KeyWords entity)
        {
            return base.UpdateEntity<KeyWords>(entity);
        }

        public void UpdateEntitys(IList<KeyWords> entitys)
        {
            base.UpdateEntitys<KeyWords>(entitys);
        }

        public bool DeleteEntity(KeyWords entity)
        {
            return base.DeleteEntity<KeyWords>(entity);
        }

        public void DeleteEntitys(IList<KeyWords> entitys)
        {
            base.DeleteEntitys<KeyWords>(entitys);
        }
    }
}