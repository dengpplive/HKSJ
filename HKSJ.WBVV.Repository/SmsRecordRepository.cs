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
  
     public class SmsRecordRepository : BaseRepository, ISmsRecordRepository
    {
        public IQueryable<SmsRecord> GetEntityList()
        {
            return base.GetEntityList<SmsRecord>();
        }

        public IQueryable<SmsRecord> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<SmsRecord>(orderCondtion);
        }

        public IQueryable<SmsRecord> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<SmsRecord>(orderCondtions);
        }

        public IQueryable<SmsRecord> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<SmsRecord>(condtion);
        }

        public IQueryable<SmsRecord> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<SmsRecord>(condtion, orderCondtion);
        }

        public IQueryable<SmsRecord> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<SmsRecord>(condtion, orderCondtions);
        }

        public IQueryable<SmsRecord> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<SmsRecord>(condtions);
        }

        public IQueryable<SmsRecord> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<SmsRecord>(condtions, orderCondtion);
        }

        public IQueryable<SmsRecord> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<SmsRecord>(condtions, orderCondtions);
        }
        public SmsRecord GetEntity(Condtion condtion)
        {
            return base.GetEntity<SmsRecord>(condtion);
        }

        public SmsRecord GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<SmsRecord>(condtions);
        }
        public bool CreateEntity(SmsRecord entity)
        {
            return base.CreateEntity<SmsRecord>(entity);
        }

        public void CreateEntitys(IList<SmsRecord> entitys)
        {
            base.CreateEntitys<SmsRecord>(entitys);
        }

        public bool UpdateEntity(SmsRecord entity)
        {
            return base.UpdateEntity<SmsRecord>(entity);
        }

        public void UpdateEntitys(IList<SmsRecord> entitys)
        {
            base.UpdateEntitys<SmsRecord>(entitys);
        }

        public bool DeleteEntity(SmsRecord entity)
        {
            return base.DeleteEntity<SmsRecord>(entity);
        }

        public void DeleteEntitys(IList<SmsRecord> entitys)
        {
            base.DeleteEntitys<SmsRecord>(entitys);
        }
    }
}