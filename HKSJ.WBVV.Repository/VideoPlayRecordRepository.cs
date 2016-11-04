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

    public class VideoPlayRecordRepository : BaseRepository, IVideoPlayRecordRepository
    {
        public IQueryable<VideoPlayRecord> GetEntityList()
        {
            return base.GetEntityList<VideoPlayRecord>();
        }

        public IQueryable<VideoPlayRecord> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<VideoPlayRecord>(orderCondtion);
        }

        public IQueryable<VideoPlayRecord> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<VideoPlayRecord>(orderCondtions);
        }

        public IQueryable<VideoPlayRecord> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<VideoPlayRecord>(condtion);
        }

        public IQueryable<VideoPlayRecord> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<VideoPlayRecord>(condtion, orderCondtion);
        }

        public IQueryable<VideoPlayRecord> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<VideoPlayRecord>(condtion, orderCondtions);
        }

        public IQueryable<VideoPlayRecord> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<VideoPlayRecord>(condtions);
        }

        public IQueryable<VideoPlayRecord> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<VideoPlayRecord>(condtions, orderCondtion);
        }

        public IQueryable<VideoPlayRecord> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<VideoPlayRecord>(condtions, orderCondtions);
        }
        public VideoPlayRecord GetEntity(Condtion condtion)
        {
            return base.GetEntity<VideoPlayRecord>(condtion);
        }

        public VideoPlayRecord GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<VideoPlayRecord>(condtions);
        }
        public bool CreateEntity(VideoPlayRecord entity)
        {
            return base.CreateEntity<VideoPlayRecord>(entity);
        }

        public void CreateEntitys(IList<VideoPlayRecord> entitys)
        {
            base.CreateEntitys<VideoPlayRecord>(entitys);
        }

        public bool UpdateEntity(VideoPlayRecord entity)
        {
            return base.UpdateEntity<VideoPlayRecord>(entity);
        }

        public void UpdateEntitys(IList<VideoPlayRecord> entitys)
        {
            base.UpdateEntitys<VideoPlayRecord>(entitys);
        }

        public bool DeleteEntity(VideoPlayRecord entity)
        {
            return base.DeleteEntity<VideoPlayRecord>(entity);
        }

        public void DeleteEntitys(IList<VideoPlayRecord> entitys)
        {
            base.DeleteEntitys<VideoPlayRecord>(entitys);
        }
    }
}