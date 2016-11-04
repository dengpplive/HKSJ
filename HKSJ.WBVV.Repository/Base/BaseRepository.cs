using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Cache;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Model.Context;

namespace HKSJ.WBVV.Repository.Base
{
    /// <summary>
    /// 数据访问父类
    /// </summary>
    public class BaseRepository
    {
        #region 上下文读操作和更改操作
        private static DataContext ReadContext
        {
            get
            {
                //1.0 先从线程缓存CallContext中根据key查找EF容器对象，如果没有则创建,同时保存到缓存中
                object obj = CallContext.GetData(typeof(DataContext).FullName);
                if (obj == null)
                {
                    //1.0.1 例化EF的上下文容器对象
                    obj = new DataContext("DataContextReader");
                    //1.0.2 将EF的上下文容器对象存入线程缓存CallContext中
                    CallContext.SetData(typeof(DataContext).FullName, obj);
                }
                //2.0 将当前的EF上下文对象返回
                return obj as DataContext;
            }
        }
        public static IList<T> GetEntitys<T>() where T : class
        {
            var data = ReadContext.Set<T>().AsNoTracking();
            return data.Any() ? data.ToList() : new List<T>();
        }
        public static T Execute<T>(Func<DataContext, T> func)
        {
            return ContextExecutor<DataContext>.Create().Execute<T>(func);
        }
        #endregion

        #region 查询操作
        protected IQueryable<T> GetEntityList<T>() where T : class, new()
        {
            return GetCacheList<T>().AsQueryable().AsNoTracking();
        }
        protected IQueryable<T> GetEntityList<T>(OrderCondtion orderCondtion) where T : class, new()
        {
            return GetEntityList<T>().OrderBy(orderCondtion).AsQueryable().AsNoTracking();
        }
        protected IQueryable<T> GetEntityList<T>(IList<OrderCondtion> orderCondtions) where T : class, new()
        {
            return GetEntityList<T>().OrderBy(orderCondtions).AsQueryable().AsNoTracking();
        }
        protected IQueryable<T> GetEntityList<T>(Condtion condtion) where T : class, new()
        {
            return GetEntityList<T>().Query(condtion).AsNoTracking();
        }
        protected IQueryable<T> GetEntityList<T>(Condtion condtion, OrderCondtion orderCondtion) where T : class, new()
        {
            return GetEntityList<T>().Query(condtion).OrderBy(orderCondtion).AsNoTracking();
        }
        protected IQueryable<T> GetEntityList<T>(Condtion condtion, IList<OrderCondtion> orderCondtions) where T : class, new()
        {
            return GetEntityList<T>().Query(condtion).OrderBy(orderCondtions).AsNoTracking();
        }
        protected IQueryable<T> GetEntityList<T>(IList<Condtion> condtions) where T : class, new()
        {
            return GetEntityList<T>().Query(condtions).AsNoTracking();
        }
        protected IQueryable<T> GetEntityList<T>(IList<Condtion> condtions, OrderCondtion orderCondtion) where T : class, new()
        {
            return GetEntityList<T>().Query(condtions).OrderBy(orderCondtion).AsNoTracking();
        }
        protected IQueryable<T> GetEntityList<T>(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions) where T : class, new()
        {
            return GetEntityList<T>().Query(condtions).OrderBy(orderCondtions).AsNoTracking();
        }
        protected T GetEntity<T>(Condtion condtion) where T : class, new()
        {
            return GetEntityList<T>().Query(condtion).AsNoTracking().FirstOrDefault();
        }
        protected T GetEntity<T>(IList<Condtion> condtions) where T : class, new()
        {
            return GetEntityList<T>().Query(condtions).AsNoTracking().FirstOrDefault();
        }

        #endregion

        #region 增删改操作,并且更新缓存
        protected bool CreateEntity<T>(T entity) where T : class, new()
        {
            var success = Execute<bool>(db =>
            {
                db.Set<T>().Add(entity);
                return db.SaveChanges() > 0;
            });
            if (success) CreateCache<T>();
            return success;
        }
        protected void CreateEntitys<T>(IList<T> entitys) where T : class, new()
        {
            var success = Execute<bool>(db =>
            {
                foreach (var entity in entitys)
                {
                    db.Set<T>().Add(entity);
                }
                return db.SaveChanges() > 0;
            });
            if (success) CreateCache<T>();
        }
        protected bool UpdateEntity<T>(T entity) where T : class, new()
        {
            var success = Execute<bool>(db =>
            {
                db.Entry(entity).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success) CreateCache<T>();
            return success;
        }
        protected void UpdateEntitys<T>(IList<T> entitys) where T : class, new()
        {
            var success = Execute<bool>(db =>
            {
                foreach (var entity in entitys)
                {
                    db.Entry(entity).State = EntityState.Modified;
                }
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success) CreateCache<T>();
        }
        protected bool DeleteEntity<T>(T entity) where T : class, new()
        {
            var success = Execute<bool>(db =>
            {
                db.Entry<T>(entity).State = EntityState.Deleted;
                return db.SaveChanges() > 0;
            });
            if (success) CreateCache<T>();
            return success;
        }
        protected void DeleteEntitys<T>(IList<T> entitys) where T : class, new()
        {
            var success = Execute<bool>(db =>
            {
                foreach (var entity in entitys)
                {
                    db.Entry<T>(entity).State = EntityState.Deleted;
                }
                return db.SaveChanges() > 0;
            });
            if (success) CreateCache<T>();
        }
        #endregion

        #region 缓存操作
        /// <summary>
        /// 数据写入缓存
        /// </summary>
        /// <typeparam name="T">T泛型</typeparam>
        /// <returns></returns>
        protected bool CreateCache<T>() where T : class, new()
        {
            string cacheKey = typeof(T).Name;
            return MemberCacheHelper.Set(cacheKey, GetEntitys<T>().ToJSON());
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <typeparam name="T">T泛型</typeparam>
        /// <returns></returns>
        protected IList<T> GetCacheList<T>() where T : class, new()
        {
            string cacheKey = typeof(T).Name;
            var list = MemberCacheHelper.Get(cacheKey).FromJSON<IList<T>>();
            if (list == null || list.Count <= 0)
            {
                list = GetEntitys<T>();
                MemberCacheHelper.Set(cacheKey, list.ToJSON());
            }
            return list;
        }
        #endregion
    }
}
