using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Tables;
using HKSJ.WBVV.Repository.Base;
using HKSJ.WBVV.Repository.Interface;

namespace HKSJ.WBVV.Repository
{
    public class QiniuFopLogRepository : BaseRepository, IQiniuFopLogRepository
    {
        public IQueryable<QiniuFopLog> GetEntityList()
        {
            return base.GetEntityList<QiniuFopLog>();
        }

        public IQueryable<QiniuFopLog> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<QiniuFopLog>(orderCondtion);
        }

        public IQueryable<QiniuFopLog> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<QiniuFopLog>(orderCondtions);
        }

        public IQueryable<QiniuFopLog> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<QiniuFopLog>(condtion);
        }

        public IQueryable<QiniuFopLog> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<QiniuFopLog>(condtion, orderCondtion);
        }

        public IQueryable<QiniuFopLog> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<QiniuFopLog>(condtion, orderCondtions);
        }

        public IQueryable<QiniuFopLog> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<QiniuFopLog>(condtions);
        }

        public IQueryable<QiniuFopLog> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<QiniuFopLog>(condtions, orderCondtion);
        }

        public IQueryable<QiniuFopLog> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<QiniuFopLog>(condtions, orderCondtions);
        }
        public QiniuFopLog GetEntity(Condtion condtion)
        {
            return base.GetEntity<QiniuFopLog>(condtion);
        }

        public QiniuFopLog GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<QiniuFopLog>(condtions);
        }
        public bool CreateEntity(QiniuFopLog entity)
        {
            return base.CreateEntity<QiniuFopLog>(entity);
        }

        public void CreateEntitys(IList<QiniuFopLog> entitys)
        {
            base.CreateEntitys<QiniuFopLog>(entitys);
        }

        public bool UpdateEntity(QiniuFopLog entity)
        {
            return base.UpdateEntity<QiniuFopLog>(entity);
        }

        public void UpdateEntitys(IList<QiniuFopLog> entitys)
        {
            base.UpdateEntitys<QiniuFopLog>(entitys);
        }

        public bool DeleteEntity(QiniuFopLog entity)
        {
            return base.DeleteEntity<QiniuFopLog>(entity);
        }

        public void DeleteEntitys(IList<QiniuFopLog> entitys)
        {
            base.DeleteEntitys<QiniuFopLog>(entitys);
        }
    }
}
