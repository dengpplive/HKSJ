using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Diagnostics;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Runtime.Remoting.Messaging;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Cache;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Model.Context
{
    /// <summary>
    /// 实体模型执行器
    /// </summary>
    /// <typeparam name="TContext">实体模型类型</typeparam>
    [DebuggerStepThrough]
    public sealed class ContextExecutor<TContext> where TContext : DbContext
    {
        #region 私有构造方法
        private ContextExecutor()
        {
            context = new DataContext("DataContext");
        }
        #endregion

        Func<TContext> CreateHandler = null;

        #region 创建执行对象
        /// <summary>
        /// 创建ObjectContext执行对象
        /// </summary>
        /// <returns>ObjectContext执行对象</returns>
        public static ContextExecutor<TContext> Create()
        {
            return Create(null);
        }

        /// <summary>
        /// 创建ObjectContext执行对象
        /// </summary>
        /// <param name="createHandler">ObjectContext创建方法</param>
        /// <returns>ObjectContext执行对象</returns>
        public static ContextExecutor<TContext> Create(Func<TContext> createHandler)
        {
            var executor = new ContextExecutor<TContext>();
            executor.CreateHandler = createHandler;
            return executor;
        }
        #endregion

        #region 私有方法
        private static object SyncContextExecutor = new object();
        private TContext GetObjectContext(Action<string> logHandler = null)
        {
            //lock (SyncContextExecutor)
            //{
            TContext tmp = null;
            tmp = CreateHandler != null ? CreateHandler() : Activator.CreateInstance<TContext>();
            if (logHandler == null)
                tmp.Database.Log = s => Console.WriteLine(s);//默认打印在控制台
            else
                tmp.Database.Log = logHandler;
            return tmp;
            //}
        }
        #endregion

        //[ThreadStatic]
        //private static TContext _localContext;
        [ThreadStatic]
        private static TContext LocalContext;
        //{
        //    get { return _localContext; }
        //    set { _localContext = value; }
        //}

        [ThreadStatic]
        private static IsolationLevel? Level;

        #region 内部方法

        public T Execute<T>(Func<TContext, T> handler, Action<string> logHandler = null)
        {
            return Execute<T>(handler, IsolationLevel.ReadCommitted, logHandler);
        }
        private DataContext context;
        private DbConnection con;
        private DbTransaction trans;
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="handler">处理方法</param>
        /// <param name="level">事务隔离级别</param>
        /// <param name="logHandler"></param>
        public T Execute<T>(Func<TContext, T> handler, IsolationLevel level, Action<string> logHandler = null)
        {
            AssertUtil.IsNotNull(handler,LanguageUtil.Translate("model_Context_ContentExecutor_check_execFunc"));
            TContext db;
            lock (SyncContextExecutor)
            {
                if (LocalContext == null)
                {
                    LocalContext = this.GetObjectContext(logHandler);
                    //AxOne begin
                    //context = LocalContext as DataContext;
                    //con = ((IObjectContextAdapter)context).ObjectContext.Connection;
                    //if (con.State != ConnectionState.Open)
                    //{
                    //    con.Open();
                    //}
                    //trans = con.BeginTransaction();
                    //AxOne end

                    Level = level;
                    #region
                    db = LocalContext;
                    DbContextTransaction tran = null;
                    T t;

                    //AxOne begin
                    //try
                    //{
                    //    t = handler(LocalContext);
                    //    trans.Commit();
                    //}
                    //catch
                    //{
                    //    trans.Rollback();
                    //    throw;
                    //}
                    //finally
                    //{
                    //    trans.Dispose();
                    //    con.Close();
                    //    context = null;
                    //    //db.Dispose();
                    //    //db = null;
                    //}
                    //trans.Dispose();
                    //con.Close();
                    //AxOne end

                    try
                    {
                        tran = db.Database.BeginTransaction(level);
                        t = handler(db);
                        tran.Commit();
                    }
                    catch
                    {
                        if (tran != null && tran.UnderlyingTransaction.Connection != null)
                            tran.Rollback();
                        throw;
                    }
                    finally
                    {
                        LocalContext = null;
                        db.Dispose();
                        db = null;
                    }
                    return t;
                    #endregion
                }
                db = LocalContext;
                return handler(db);
            }
        }
        #endregion
    }
}