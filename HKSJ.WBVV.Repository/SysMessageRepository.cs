



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
  
     public class SysMessageRepository : BaseRepository, ISysMessageRepository
    {
        public IQueryable<SysMessage> GetEntityList()
        {
            return base.GetEntityList<SysMessage>();
        }

        public IQueryable<SysMessage> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<SysMessage>(orderCondtion);
        }

        public IQueryable<SysMessage> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<SysMessage>(orderCondtions);
        }

        public IQueryable<SysMessage> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<SysMessage>(condtion);
        }

        public IQueryable<SysMessage> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<SysMessage>(condtion, orderCondtion);
        }

        public IQueryable<SysMessage> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<SysMessage>(condtion, orderCondtions);
        }

        public IQueryable<SysMessage> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<SysMessage>(condtions);
        }

        public IQueryable<SysMessage> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<SysMessage>(condtions, orderCondtion);
        }

        public IQueryable<SysMessage> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<SysMessage>(condtions, orderCondtions);
        }
        public SysMessage GetEntity(Condtion condtion)
        {
            return base.GetEntity<SysMessage>(condtion);
        }

        public SysMessage GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<SysMessage>(condtions);
        }
        public bool CreateEntity(SysMessage entity)
        {
            return base.CreateEntity<SysMessage>(entity);
        }

        public void CreateEntitys(IList<SysMessage> entitys)
        {
            base.CreateEntitys<SysMessage>(entitys);
        }

        public bool UpdateEntity(SysMessage entity)
        {
            return base.UpdateEntity<SysMessage>(entity);
        }

        public void UpdateEntitys(IList<SysMessage> entitys)
        {
            base.UpdateEntitys<SysMessage>(entitys);
        }

        public bool DeleteEntity(SysMessage entity)
        {
            return base.DeleteEntity<SysMessage>(entity);
        }

        public void DeleteEntitys(IList<SysMessage> entitys)
        {
            base.DeleteEntitys<SysMessage>(entitys);
        }
    }
}