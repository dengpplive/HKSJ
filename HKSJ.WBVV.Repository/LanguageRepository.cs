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

    public class LanguageRepository : BaseRepository, ILanguageRepository
    {
        public IQueryable<Language> GetEntityList()
        {
            return base.GetEntityList<Language>();
        }

        public IQueryable<Language> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Language>(orderCondtion);
        }

        public IQueryable<Language> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Language>(orderCondtions);
        }

        public IQueryable<Language> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<Language>(condtion);
        }

        public IQueryable<Language> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Language>(condtion, orderCondtion);
        }

        public IQueryable<Language> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Language>(condtion, orderCondtions);
        }

        public IQueryable<Language> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<Language>(condtions);
        }

        public IQueryable<Language> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Language>(condtions, orderCondtion);
        }

        public IQueryable<Language> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Language>(condtions, orderCondtions);
        }
        public Language GetEntity(Condtion condtion)
        {
            return base.GetEntity<Language>(condtion);
        }

        public Language GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<Language>(condtions);
        }
        public bool CreateEntity(Language entity)
        {
            return base.CreateEntity<Language>(entity);
        }

        public void CreateEntitys(IList<Language> entitys)
        {
            base.CreateEntitys<Language>(entitys);
        }

        public bool UpdateEntity(Language entity)
        {
            return base.UpdateEntity<Language>(entity);
        }

        public void UpdateEntitys(IList<Language> entitys)
        {
            base.UpdateEntitys<Language>(entitys);
        }

        public bool DeleteEntity(Language entity)
        {
            return base.DeleteEntity<Language>(entity);
        }

        public void DeleteEntitys(IList<Language> entitys)
        {
            base.DeleteEntitys<Language>(entitys);
        }
    }
}