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
  
     public class ManageRepository : BaseRepository, IManageRepository
    {
        public IQueryable<Manage> GetEntityList()
        {
            return base.GetEntityList<Manage>();
        }

        public IQueryable<Manage> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Manage>(orderCondtion);
        }

        public IQueryable<Manage> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Manage>(orderCondtions);
        }

        public IQueryable<Manage> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<Manage>(condtion);
        }

        public IQueryable<Manage> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Manage>(condtion, orderCondtion);
        }

        public IQueryable<Manage> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Manage>(condtion, orderCondtions);
        }

        public IQueryable<Manage> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<Manage>(condtions);
        }

        public IQueryable<Manage> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Manage>(condtions, orderCondtion);
        }

        public IQueryable<Manage> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Manage>(condtions, orderCondtions);
        }
        public Manage GetEntity(Condtion condtion)
        {
            return base.GetEntity<Manage>(condtion);
        }

        public Manage GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<Manage>(condtions);
        }
        public bool CreateEntity(Manage entity)
        {
            return base.CreateEntity<Manage>(entity);
        }

        public void CreateEntitys(IList<Manage> entitys)
        {
            base.CreateEntitys<Manage>(entitys);
        }

        public bool UpdateEntity(Manage entity)
        {
            return base.UpdateEntity<Manage>(entity);
        }

        public void UpdateEntitys(IList<Manage> entitys)
        {
            base.UpdateEntitys<Manage>(entitys);
        }

        public bool DeleteEntity(Manage entity)
        {
            return base.DeleteEntity<Manage>(entity);
        }

        public void DeleteEntitys(IList<Manage> entitys)
        {
            base.DeleteEntitys<Manage>(entitys);
        }
    }
}