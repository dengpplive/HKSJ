using System.Data.Entity.Validation;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Model.Context
{
    public partial class DataContext : DbContext
    {

        #region 构造函数
        public DataContext()
            : this("name=DataContextWriter")
        {
            //禁用延迟加载
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
        public DataContext(string connectionString)
            : base(connectionString)
        {
            //禁用延迟加载
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            #region DbEntityValidationException
            catch (DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                {
                    var entry = error.Entry;
                    foreach (var err in error.ValidationErrors)
                    {
                        Debug.WriteLine(err.PropertyName + " " + err.ErrorMessage);
                    }
                }
                throw new Exception(LanguageUtil.Translate("model_Context_DataContent_SavaChanges_check"), ex.MostInnerException());
            }
            #endregion
            #region DbUpdateConcurrencyException
            catch (DbUpdateConcurrencyException ex)
            {
                var dbEntityEntry = ex.Entries.Single();
                //store wins
                dbEntityEntry.Reload();
                //OR client wins
                //var dbPropertyValues = dbEntityEntry.GetDatabaseValues();
                // dbEntityEntry.OriginalValues.SetValues(dbPropertyValues);
                throw new Exception(LanguageUtil.Translate("model_Context_DataContent_SavaChanges_dealWith"), ex.MostInnerException());
            }
            #endregion
            #region DbUpdateException
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    Debug.WriteLine(ex.InnerException.Message);
                }
                //which exceptions does it relate to
                foreach (var entry in ex.Entries)
                {
                    Debug.WriteLine(entry.Entity);
                }
                throw new Exception(LanguageUtil.Translate("model_Context_DataContent_SavaChanges_update"), ex.MostInnerException());
            }
            #endregion
        }
    }
}
