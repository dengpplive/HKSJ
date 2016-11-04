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
  
     public class UserUploadVedioLogRepository : BaseRepository, IUserUploadVedioLogRepository
    {
        public IQueryable<UserUploadVedioLog> GetEntityList()
        {
            return base.GetEntityList<UserUploadVedioLog>();
        }

        public IQueryable<UserUploadVedioLog> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserUploadVedioLog>(orderCondtion);
        }

        public IQueryable<UserUploadVedioLog> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserUploadVedioLog>(orderCondtions);
        }

        public IQueryable<UserUploadVedioLog> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserUploadVedioLog>(condtion);
        }

        public IQueryable<UserUploadVedioLog> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserUploadVedioLog>(condtion, orderCondtion);
        }

        public IQueryable<UserUploadVedioLog> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserUploadVedioLog>(condtion, orderCondtions);
        }

        public IQueryable<UserUploadVedioLog> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserUploadVedioLog>(condtions);
        }

        public IQueryable<UserUploadVedioLog> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserUploadVedioLog>(condtions, orderCondtion);
        }

        public IQueryable<UserUploadVedioLog> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserUploadVedioLog>(condtions, orderCondtions);
        }
        public UserUploadVedioLog GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserUploadVedioLog>(condtion);
        }

        public UserUploadVedioLog GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserUploadVedioLog>(condtions);
        }
        public bool CreateEntity(UserUploadVedioLog entity)
        {
            return base.CreateEntity<UserUploadVedioLog>(entity);
        }

        public void CreateEntitys(IList<UserUploadVedioLog> entitys)
        {
            base.CreateEntitys<UserUploadVedioLog>(entitys);
        }

        public bool UpdateEntity(UserUploadVedioLog entity)
        {
            return base.UpdateEntity<UserUploadVedioLog>(entity);
        }

        public void UpdateEntitys(IList<UserUploadVedioLog> entitys)
        {
            base.UpdateEntitys<UserUploadVedioLog>(entitys);
        }

        public bool DeleteEntity(UserUploadVedioLog entity)
        {
            return base.DeleteEntity<UserUploadVedioLog>(entity);
        }

        public void DeleteEntitys(IList<UserUploadVedioLog> entitys)
        {
            base.DeleteEntitys<UserUploadVedioLog>(entitys);
        }
    }
}