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
  
     public class RoleRepository : BaseRepository, IRoleRepository
    {
        public IQueryable<Role> GetEntityList()
        {
            return base.GetEntityList<Role>();
        }

        public IQueryable<Role> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Role>(orderCondtion);
        }

        public IQueryable<Role> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Role>(orderCondtions);
        }

        public IQueryable<Role> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<Role>(condtion);
        }

        public IQueryable<Role> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Role>(condtion, orderCondtion);
        }

        public IQueryable<Role> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Role>(condtion, orderCondtions);
        }

        public IQueryable<Role> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<Role>(condtions);
        }

        public IQueryable<Role> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Role>(condtions, orderCondtion);
        }

        public IQueryable<Role> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Role>(condtions, orderCondtions);
        }
        public Role GetEntity(Condtion condtion)
        {
            return base.GetEntity<Role>(condtion);
        }

        public Role GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<Role>(condtions);
        }
        public bool CreateEntity(Role entity)
        {
            return base.CreateEntity<Role>(entity);
        }

        public void CreateEntitys(IList<Role> entitys)
        {
            base.CreateEntitys<Role>(entitys);
        }

        public bool UpdateEntity(Role entity)
        {
            return base.UpdateEntity<Role>(entity);
        }

        public void UpdateEntitys(IList<Role> entitys)
        {
            base.UpdateEntitys<Role>(entitys);
        }

        public bool DeleteEntity(Role entity)
        {
            return base.DeleteEntity<Role>(entity);
        }

        public void DeleteEntitys(IList<Role> entitys)
        {
            base.DeleteEntitys<Role>(entitys);
        }
    }
}