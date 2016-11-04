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
  
     public class PermissionRepository : BaseRepository, IPermissionRepository
    {
        public IQueryable<Permission> GetEntityList()
        {
            return base.GetEntityList<Permission>();
        }

        public IQueryable<Permission> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Permission>(orderCondtion);
        }

        public IQueryable<Permission> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Permission>(orderCondtions);
        }

        public IQueryable<Permission> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<Permission>(condtion);
        }

        public IQueryable<Permission> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Permission>(condtion, orderCondtion);
        }

        public IQueryable<Permission> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Permission>(condtion, orderCondtions);
        }

        public IQueryable<Permission> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<Permission>(condtions);
        }

        public IQueryable<Permission> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Permission>(condtions, orderCondtion);
        }

        public IQueryable<Permission> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Permission>(condtions, orderCondtions);
        }
        public Permission GetEntity(Condtion condtion)
        {
            return base.GetEntity<Permission>(condtion);
        }

        public Permission GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<Permission>(condtions);
        }
        public bool CreateEntity(Permission entity)
        {
            return base.CreateEntity<Permission>(entity);
        }

        public void CreateEntitys(IList<Permission> entitys)
        {
            base.CreateEntitys<Permission>(entitys);
        }

        public bool UpdateEntity(Permission entity)
        {
            return base.UpdateEntity<Permission>(entity);
        }

        public void UpdateEntitys(IList<Permission> entitys)
        {
            base.UpdateEntitys<Permission>(entitys);
        }

        public bool DeleteEntity(Permission entity)
        {
            return base.DeleteEntity<Permission>(entity);
        }

        public void DeleteEntitys(IList<Permission> entitys)
        {
            base.DeleteEntitys<Permission>(entitys);
        }
    }
}