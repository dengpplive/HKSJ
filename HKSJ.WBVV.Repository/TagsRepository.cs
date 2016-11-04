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
  
     public class TagsRepository : BaseRepository, ITagsRepository
    {
        public IQueryable<Tags> GetEntityList()
        {
            return base.GetEntityList<Tags>();
        }

        public IQueryable<Tags> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Tags>(orderCondtion);
        }

        public IQueryable<Tags> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Tags>(orderCondtions);
        }

        public IQueryable<Tags> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<Tags>(condtion);
        }

        public IQueryable<Tags> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Tags>(condtion, orderCondtion);
        }

        public IQueryable<Tags> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Tags>(condtion, orderCondtions);
        }

        public IQueryable<Tags> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<Tags>(condtions);
        }

        public IQueryable<Tags> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Tags>(condtions, orderCondtion);
        }

        public IQueryable<Tags> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Tags>(condtions, orderCondtions);
        }
        public Tags GetEntity(Condtion condtion)
        {
            return base.GetEntity<Tags>(condtion);
        }

        public Tags GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<Tags>(condtions);
        }
        public bool CreateEntity(Tags entity)
        {
            return base.CreateEntity<Tags>(entity);
        }

        public void CreateEntitys(IList<Tags> entitys)
        {
            base.CreateEntitys<Tags>(entitys);
        }

        public bool UpdateEntity(Tags entity)
        {
            return base.UpdateEntity<Tags>(entity);
        }

        public void UpdateEntitys(IList<Tags> entitys)
        {
            base.UpdateEntitys<Tags>(entitys);
        }

        public bool DeleteEntity(Tags entity)
        {
            return base.DeleteEntity<Tags>(entity);
        }

        public void DeleteEntitys(IList<Tags> entitys)
        {
            base.DeleteEntitys<Tags>(entitys);
        }
    }
}