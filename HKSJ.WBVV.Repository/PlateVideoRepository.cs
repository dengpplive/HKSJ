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
  
     public class PlateVideoRepository : BaseRepository, IPlateVideoRepository
    {
        public IQueryable<PlateVideo> GetEntityList()
        {
            return base.GetEntityList<PlateVideo>();
        }

        public IQueryable<PlateVideo> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<PlateVideo>(orderCondtion);
        }

        public IQueryable<PlateVideo> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<PlateVideo>(orderCondtions);
        }

        public IQueryable<PlateVideo> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<PlateVideo>(condtion);
        }

        public IQueryable<PlateVideo> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<PlateVideo>(condtion, orderCondtion);
        }

        public IQueryable<PlateVideo> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<PlateVideo>(condtion, orderCondtions);
        }

        public IQueryable<PlateVideo> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<PlateVideo>(condtions);
        }

        public IQueryable<PlateVideo> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<PlateVideo>(condtions, orderCondtion);
        }

        public IQueryable<PlateVideo> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<PlateVideo>(condtions, orderCondtions);
        }
        public PlateVideo GetEntity(Condtion condtion)
        {
            return base.GetEntity<PlateVideo>(condtion);
        }

        public PlateVideo GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<PlateVideo>(condtions);
        }
        public bool CreateEntity(PlateVideo entity)
        {
            return base.CreateEntity<PlateVideo>(entity);
        }

        public void CreateEntitys(IList<PlateVideo> entitys)
        {
            base.CreateEntitys<PlateVideo>(entitys);
        }

        public bool UpdateEntity(PlateVideo entity)
        {
            return base.UpdateEntity<PlateVideo>(entity);
        }

        public void UpdateEntitys(IList<PlateVideo> entitys)
        {
            base.UpdateEntitys<PlateVideo>(entitys);
        }

        public bool DeleteEntity(PlateVideo entity)
        {
            return base.DeleteEntity<PlateVideo>(entity);
        }

        public void DeleteEntitys(IList<PlateVideo> entitys)
        {
            base.DeleteEntitys<PlateVideo>(entitys);
        }
    }
}