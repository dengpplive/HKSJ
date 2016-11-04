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
  
     public class LevelRepository : BaseRepository, ILevelRepository
    {
        public IQueryable<Level> GetEntityList()
        {
            return base.GetEntityList<Level>();
        }

        public IQueryable<Level> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Level>(orderCondtion);
        }

        public IQueryable<Level> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Level>(orderCondtions);
        }

        public IQueryable<Level> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<Level>(condtion);
        }

        public IQueryable<Level> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Level>(condtion, orderCondtion);
        }

        public IQueryable<Level> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Level>(condtion, orderCondtions);
        }

        public IQueryable<Level> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<Level>(condtions);
        }

        public IQueryable<Level> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Level>(condtions, orderCondtion);
        }

        public IQueryable<Level> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Level>(condtions, orderCondtions);
        }
        public Level GetEntity(Condtion condtion)
        {
            return base.GetEntity<Level>(condtion);
        }

        public Level GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<Level>(condtions);
        }
        public bool CreateEntity(Level entity)
        {
            return base.CreateEntity<Level>(entity);
        }

        public void CreateEntitys(IList<Level> entitys)
        {
            base.CreateEntitys<Level>(entitys);
        }

        public bool UpdateEntity(Level entity)
        {
            return base.UpdateEntity<Level>(entity);
        }

        public void UpdateEntitys(IList<Level> entitys)
        {
            base.UpdateEntitys<Level>(entitys);
        }

        public bool DeleteEntity(Level entity)
        {
            return base.DeleteEntity<Level>(entity);
        }

        public void DeleteEntitys(IList<Level> entitys)
        {
            base.DeleteEntitys<Level>(entitys);
        }
    }
}