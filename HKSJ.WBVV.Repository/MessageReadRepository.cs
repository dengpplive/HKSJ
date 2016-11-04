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
  
     public class MessageReadRepository : BaseRepository, IMessageReadRepository
    {
        public IQueryable<MessageRead> GetEntityList()
        {
            return base.GetEntityList<MessageRead>();
        }

        public IQueryable<MessageRead> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<MessageRead>(orderCondtion);
        }

        public IQueryable<MessageRead> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<MessageRead>(orderCondtions);
        }

        public IQueryable<MessageRead> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<MessageRead>(condtion);
        }

        public IQueryable<MessageRead> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<MessageRead>(condtion, orderCondtion);
        }

        public IQueryable<MessageRead> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<MessageRead>(condtion, orderCondtions);
        }

        public IQueryable<MessageRead> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<MessageRead>(condtions);
        }

        public IQueryable<MessageRead> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<MessageRead>(condtions, orderCondtion);
        }

        public IQueryable<MessageRead> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<MessageRead>(condtions, orderCondtions);
        }
        public MessageRead GetEntity(Condtion condtion)
        {
            return base.GetEntity<MessageRead>(condtion);
        }

        public MessageRead GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<MessageRead>(condtions);
        }
        public bool CreateEntity(MessageRead entity)
        {
            return base.CreateEntity<MessageRead>(entity);
        }

        public void CreateEntitys(IList<MessageRead> entitys)
        {
            base.CreateEntitys<MessageRead>(entitys);
        }

        public bool UpdateEntity(MessageRead entity)
        {
            return base.UpdateEntity<MessageRead>(entity);
        }

        public void UpdateEntitys(IList<MessageRead> entitys)
        {
            base.UpdateEntitys<MessageRead>(entitys);
        }

        public bool DeleteEntity(MessageRead entity)
        {
            return base.DeleteEntity<MessageRead>(entity);
        }

        public void DeleteEntitys(IList<MessageRead> entitys)
        {
            base.DeleteEntitys<MessageRead>(entitys);
        }
    }
}