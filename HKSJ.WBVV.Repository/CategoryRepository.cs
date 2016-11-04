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
  
     public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public IQueryable<Category> GetEntityList()
        {
            return base.GetEntityList<Category>();
        }

        public IQueryable<Category> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Category>(orderCondtion);
        }

        public IQueryable<Category> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Category>(orderCondtions);
        }

        public IQueryable<Category> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<Category>(condtion);
        }

        public IQueryable<Category> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Category>(condtion, orderCondtion);
        }

        public IQueryable<Category> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Category>(condtion, orderCondtions);
        }

        public IQueryable<Category> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<Category>(condtions);
        }

        public IQueryable<Category> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Category>(condtions, orderCondtion);
        }

        public IQueryable<Category> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Category>(condtions, orderCondtions);
        }
        public Category GetEntity(Condtion condtion)
        {
            return base.GetEntity<Category>(condtion);
        }

        public Category GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<Category>(condtions);
        }
        public bool CreateEntity(Category entity)
        {
            return base.CreateEntity<Category>(entity);
        }

        public void CreateEntitys(IList<Category> entitys)
        {
            base.CreateEntitys<Category>(entitys);
        }

        public bool UpdateEntity(Category entity)
        {
            return base.UpdateEntity<Category>(entity);
        }

        public void UpdateEntitys(IList<Category> entitys)
        {
            base.UpdateEntitys<Category>(entitys);
        }

        public bool DeleteEntity(Category entity)
        {
            return base.DeleteEntity<Category>(entity);
        }

        public void DeleteEntitys(IList<Category> entitys)
        {
            base.DeleteEntitys<Category>(entitys);
        }
    }
}