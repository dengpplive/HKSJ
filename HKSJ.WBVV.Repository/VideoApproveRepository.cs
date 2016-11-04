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
  
     public class VideoApproveRepository : BaseRepository, IVideoApproveRepository
    {
        public IQueryable<VideoApprove> GetEntityList()
        {
            return base.GetEntityList<VideoApprove>();
        }

        public IQueryable<VideoApprove> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<VideoApprove>(orderCondtion);
        }

        public IQueryable<VideoApprove> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<VideoApprove>(orderCondtions);
        }

        public IQueryable<VideoApprove> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<VideoApprove>(condtion);
        }

        public IQueryable<VideoApprove> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<VideoApprove>(condtion, orderCondtion);
        }

        public IQueryable<VideoApprove> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<VideoApprove>(condtion, orderCondtions);
        }

        public IQueryable<VideoApprove> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<VideoApprove>(condtions);
        }

        public IQueryable<VideoApprove> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<VideoApprove>(condtions, orderCondtion);
        }

        public IQueryable<VideoApprove> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<VideoApprove>(condtions, orderCondtions);
        }
        public VideoApprove GetEntity(Condtion condtion)
        {
            return base.GetEntity<VideoApprove>(condtion);
        }

        public VideoApprove GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<VideoApprove>(condtions);
        }
        public bool CreateEntity(VideoApprove entity)
        {
            return base.CreateEntity<VideoApprove>(entity);
        }

        public void CreateEntitys(IList<VideoApprove> entitys)
        {
            base.CreateEntitys<VideoApprove>(entitys);
        }

        public bool UpdateEntity(VideoApprove entity)
        {
            return base.UpdateEntity<VideoApprove>(entity);
        }

        public void UpdateEntitys(IList<VideoApprove> entitys)
        {
            base.UpdateEntitys<VideoApprove>(entitys);
        }

        public bool DeleteEntity(VideoApprove entity)
        {
            return base.DeleteEntity<VideoApprove>(entity);
        }

        public void DeleteEntitys(IList<VideoApprove> entitys)
        {
            base.DeleteEntitys<VideoApprove>(entitys);
        }
    }
}