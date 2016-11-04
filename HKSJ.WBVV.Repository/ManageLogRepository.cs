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
  
     public class ManageLogRepository : BaseRepository, IManageLogRepository
    {
        public IQueryable<ManageLog> GetEntityList()
        {
            return base.GetEntityList<ManageLog>();
        }

        public IQueryable<ManageLog> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<ManageLog>(orderCondtion);
        }

        public IQueryable<ManageLog> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<ManageLog>(orderCondtions);
        }

        public IQueryable<ManageLog> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<ManageLog>(condtion);
        }

        public IQueryable<ManageLog> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<ManageLog>(condtion, orderCondtion);
        }

        public IQueryable<ManageLog> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<ManageLog>(condtion, orderCondtions);
        }

        public IQueryable<ManageLog> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<ManageLog>(condtions);
        }

        public IQueryable<ManageLog> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<ManageLog>(condtions, orderCondtion);
        }

        public IQueryable<ManageLog> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<ManageLog>(condtions, orderCondtions);
        }
        public ManageLog GetEntity(Condtion condtion)
        {
            return base.GetEntity<ManageLog>(condtion);
        }

        public ManageLog GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<ManageLog>(condtions);
        }
        public bool CreateEntity(ManageLog entity)
        {
            return base.CreateEntity<ManageLog>(entity);
        }

        public void CreateEntitys(IList<ManageLog> entitys)
        {
            base.CreateEntitys<ManageLog>(entitys);
        }

        public bool UpdateEntity(ManageLog entity)
        {
            return base.UpdateEntity<ManageLog>(entity);
        }

        public void UpdateEntitys(IList<ManageLog> entitys)
        {
            base.UpdateEntitys<ManageLog>(entitys);
        }

        public bool DeleteEntity(ManageLog entity)
        {
            return base.DeleteEntity<ManageLog>(entity);
        }

        public void DeleteEntitys(IList<ManageLog> entitys)
        {
            base.DeleteEntitys<ManageLog>(entitys);
        }
    }
}