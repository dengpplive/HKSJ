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
  
     public class PlateRepository : BaseRepository, IPlateRepository
    {
        public IQueryable<Plate> GetEntityList()
        {
            return base.GetEntityList<Plate>();
        }

        public IQueryable<Plate> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Plate>(orderCondtion);
        }

        public IQueryable<Plate> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Plate>(orderCondtions);
        }

        public IQueryable<Plate> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<Plate>(condtion);
        }

        public IQueryable<Plate> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Plate>(condtion, orderCondtion);
        }

        public IQueryable<Plate> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Plate>(condtion, orderCondtions);
        }

        public IQueryable<Plate> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<Plate>(condtions);
        }

        public IQueryable<Plate> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Plate>(condtions, orderCondtion);
        }

        public IQueryable<Plate> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Plate>(condtions, orderCondtions);
        }
        public Plate GetEntity(Condtion condtion)
        {
            return base.GetEntity<Plate>(condtion);
        }

        public Plate GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<Plate>(condtions);
        }
        public bool CreateEntity(Plate entity)
        {
            return base.CreateEntity<Plate>(entity);
        }

        public void CreateEntitys(IList<Plate> entitys)
        {
            base.CreateEntitys<Plate>(entitys);
        }

        public bool UpdateEntity(Plate entity)
        {
            return base.UpdateEntity<Plate>(entity);
        }

        public void UpdateEntitys(IList<Plate> entitys)
        {
            base.UpdateEntitys<Plate>(entitys);
        }

        public bool DeleteEntity(Plate entity)
        {
            return base.DeleteEntity<Plate>(entity);
        }

        public void DeleteEntitys(IList<Plate> entitys)
        {
            base.DeleteEntitys<Plate>(entitys);
        }
    }
}