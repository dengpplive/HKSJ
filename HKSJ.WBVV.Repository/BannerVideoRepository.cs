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
  
     public class BannerVideoRepository : BaseRepository, IBannerVideoRepository
    {
        public IQueryable<BannerVideo> GetEntityList()
        {
            return base.GetEntityList<BannerVideo>();
        }

        public IQueryable<BannerVideo> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<BannerVideo>(orderCondtion);
        }

        public IQueryable<BannerVideo> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<BannerVideo>(orderCondtions);
        }

        public IQueryable<BannerVideo> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<BannerVideo>(condtion);
        }

        public IQueryable<BannerVideo> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<BannerVideo>(condtion, orderCondtion);
        }

        public IQueryable<BannerVideo> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<BannerVideo>(condtion, orderCondtions);
        }

        public IQueryable<BannerVideo> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<BannerVideo>(condtions);
        }

        public IQueryable<BannerVideo> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<BannerVideo>(condtions, orderCondtion);
        }

        public IQueryable<BannerVideo> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<BannerVideo>(condtions, orderCondtions);
        }
        public BannerVideo GetEntity(Condtion condtion)
        {
            return base.GetEntity<BannerVideo>(condtion);
        }

        public BannerVideo GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<BannerVideo>(condtions);
        }
        public bool CreateEntity(BannerVideo entity)
        {
            return base.CreateEntity<BannerVideo>(entity);
        }

        public void CreateEntitys(IList<BannerVideo> entitys)
        {
            base.CreateEntitys<BannerVideo>(entitys);
        }

        public bool UpdateEntity(BannerVideo entity)
        {
            return base.UpdateEntity<BannerVideo>(entity);
        }

        public void UpdateEntitys(IList<BannerVideo> entitys)
        {
            base.UpdateEntitys<BannerVideo>(entitys);
        }

        public bool DeleteEntity(BannerVideo entity)
        {
            return base.DeleteEntity<BannerVideo>(entity);
        }

        public void DeleteEntitys(IList<BannerVideo> entitys)
        {
            base.DeleteEntitys<BannerVideo>(entitys);
        }
    }
}