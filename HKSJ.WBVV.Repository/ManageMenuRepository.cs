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
  
     public class ManageMenuRepository : BaseRepository, IManageMenuRepository
    {
        public IQueryable<ManageMenu> GetEntityList()
        {
            return base.GetEntityList<ManageMenu>();
        }

        public IQueryable<ManageMenu> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<ManageMenu>(orderCondtion);
        }

        public IQueryable<ManageMenu> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<ManageMenu>(orderCondtions);
        }

        public IQueryable<ManageMenu> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<ManageMenu>(condtion);
        }

        public IQueryable<ManageMenu> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<ManageMenu>(condtion, orderCondtion);
        }

        public IQueryable<ManageMenu> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<ManageMenu>(condtion, orderCondtions);
        }

        public IQueryable<ManageMenu> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<ManageMenu>(condtions);
        }

        public IQueryable<ManageMenu> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<ManageMenu>(condtions, orderCondtion);
        }

        public IQueryable<ManageMenu> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<ManageMenu>(condtions, orderCondtions);
        }
        public ManageMenu GetEntity(Condtion condtion)
        {
            return base.GetEntity<ManageMenu>(condtion);
        }

        public ManageMenu GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<ManageMenu>(condtions);
        }
        public bool CreateEntity(ManageMenu entity)
        {
            return base.CreateEntity<ManageMenu>(entity);
        }

        public void CreateEntitys(IList<ManageMenu> entitys)
        {
            base.CreateEntitys<ManageMenu>(entitys);
        }

        public bool UpdateEntity(ManageMenu entity)
        {
            return base.UpdateEntity<ManageMenu>(entity);
        }

        public void UpdateEntitys(IList<ManageMenu> entitys)
        {
            base.UpdateEntitys<ManageMenu>(entitys);
        }

        public bool DeleteEntity(ManageMenu entity)
        {
            return base.DeleteEntity<ManageMenu>(entity);
        }

        public void DeleteEntitys(IList<ManageMenu> entitys)
        {
            base.DeleteEntitys<ManageMenu>(entitys);
        }
    }
}