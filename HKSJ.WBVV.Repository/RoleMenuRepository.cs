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
  
     public class RoleMenuRepository : BaseRepository, IRoleMenuRepository
    {
        public IQueryable<RoleMenu> GetEntityList()
        {
            return base.GetEntityList<RoleMenu>();
        }

        public IQueryable<RoleMenu> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<RoleMenu>(orderCondtion);
        }

        public IQueryable<RoleMenu> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<RoleMenu>(orderCondtions);
        }

        public IQueryable<RoleMenu> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<RoleMenu>(condtion);
        }

        public IQueryable<RoleMenu> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<RoleMenu>(condtion, orderCondtion);
        }

        public IQueryable<RoleMenu> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<RoleMenu>(condtion, orderCondtions);
        }

        public IQueryable<RoleMenu> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<RoleMenu>(condtions);
        }

        public IQueryable<RoleMenu> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<RoleMenu>(condtions, orderCondtion);
        }

        public IQueryable<RoleMenu> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<RoleMenu>(condtions, orderCondtions);
        }
        public RoleMenu GetEntity(Condtion condtion)
        {
            return base.GetEntity<RoleMenu>(condtion);
        }

        public RoleMenu GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<RoleMenu>(condtions);
        }
        public bool CreateEntity(RoleMenu entity)
        {
            return base.CreateEntity<RoleMenu>(entity);
        }

        public void CreateEntitys(IList<RoleMenu> entitys)
        {
            base.CreateEntitys<RoleMenu>(entitys);
        }

        public bool UpdateEntity(RoleMenu entity)
        {
            return base.UpdateEntity<RoleMenu>(entity);
        }

        public void UpdateEntitys(IList<RoleMenu> entitys)
        {
            base.UpdateEntitys<RoleMenu>(entitys);
        }

        public bool DeleteEntity(RoleMenu entity)
        {
            return base.DeleteEntity<RoleMenu>(entity);
        }

        public void DeleteEntitys(IList<RoleMenu> entitys)
        {
            base.DeleteEntitys<RoleMenu>(entitys);
        }
    }
}