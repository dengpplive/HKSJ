using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Model.Context;

namespace HKSJ.WBVV.Repository.Interface.IBase
{
    /// <summary>
    /// 数据访问父类接口
    /// </summary>
    public interface IBaseAccess<T> where T : class, new()
    {
        IQueryable<T> GetEntityList();
        IQueryable<T> GetEntityList(OrderCondtion orderCondtion);
        IQueryable<T> GetEntityList(IList<OrderCondtion> orderCondtions);
        IQueryable<T> GetEntityList(Condtion condtion);
        IQueryable<T> GetEntityList(Condtion condtion, OrderCondtion orderCondtion);
        IQueryable<T> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions);
        IQueryable<T> GetEntityList(IList<Condtion> condtions);
        IQueryable<T> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion);
        IQueryable<T> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
        T GetEntity(Condtion condtion);
        T GetEntity(IList<Condtion> condtions);
        bool CreateEntity(T entity);
        void CreateEntitys(IList<T> entitys);
        bool UpdateEntity(T entity);
        void UpdateEntitys(IList<T> entitys);
        bool DeleteEntity(T entity);
        void DeleteEntitys(IList<T> entitys);

    }
}
