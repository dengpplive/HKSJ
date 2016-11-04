



using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Common.Language;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Base;
using HKSJ.WBVV.Repository.Interface;
namespace HKSJ.WBVV.Repository
{

    public class UserLevelRepository : BaseRepository, IUserLevelRepository
    {
        public IQueryable<UserLevel> GetEntityList()
        {
            return base.GetEntityList<UserLevel>();
        }

        public IQueryable<UserLevel> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserLevel>(orderCondtion);
        }

        public IQueryable<UserLevel> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserLevel>(orderCondtions);
        }

        public IQueryable<UserLevel> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserLevel>(condtion);
        }

        public IQueryable<UserLevel> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserLevel>(condtion, orderCondtion);
        }

        public IQueryable<UserLevel> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserLevel>(condtion, orderCondtions);
        }

        public IQueryable<UserLevel> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserLevel>(condtions);
        }

        public IQueryable<UserLevel> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserLevel>(condtions, orderCondtion);
        }

        public IQueryable<UserLevel> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserLevel>(condtions, orderCondtions);
        }
        public UserLevel GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserLevel>(condtion);
        }

        public UserLevel GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserLevel>(condtions);
        }
        public bool CreateEntity(UserLevel entity)
        {
            return base.CreateEntity<UserLevel>(entity);
        }

        public void CreateEntitys(IList<UserLevel> entitys)
        {
            base.CreateEntitys(entitys);
        }

        public bool UpdateEntity(UserLevel entity)
        {
            return base.UpdateEntity(entity);
        }

        public void UpdateEntitys(IList<UserLevel> entitys)
        {
            base.UpdateEntitys<UserLevel>(entitys);
        }

        public bool DeleteEntity(UserLevel entity)
        {
            return base.DeleteEntity<UserLevel>(entity);
        }

        public void DeleteEntitys(IList<UserLevel> entitys)
        {
            base.DeleteEntitys<UserLevel>(entitys);
        }
        /// <summary>
        /// 添加等级
        /// </summary>
        /// <param name="levelName"></param>
        /// <param name="levelStart"></param>
        /// <param name="levelEnd"></param>
        /// <param name="levelIcon"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        public bool CreateUserLevel(string levelName, int levelStart, int levelEnd, string levelIcon, int createUserId)
        {
            AssertUtil.NotNullOrWhiteSpace(levelName, LanguageUtil.Translate("api_Repository_UserLevel_CreateUserLevel_levelName_NotNullOrWhiteSpace"));
            AssertUtil.AreBigger(levelEnd, levelStart, LanguageUtil.Translate("api_Repository_UserLevel_CreateUserLevel_levelEnd_AreBigger"));
            AssertUtil.NotNullOrWhiteSpace(levelIcon, LanguageUtil.Translate("api_Repository_UserLevel_CreateUserLevel_levelIcon_NotNullOrWhiteSpace"));
            AssertUtil.AreBigger(createUserId, 0, LanguageUtil.Translate("api_Repository_UserLevel_CreateUserLevel_createUserId_AreBigger"));
            var success = Execute<bool>(db =>
            {
                var manage = db.Manage.FirstOrDefault(m => m.State == false && m.Id == createUserId);
                AssertUtil.IsNotNull(manage, LanguageUtil.Translate("api_Repository_UserLevel_CreateUserLevel_createUserId_IsNotNull"));
                var userLevel = new UserLevel()
                {
                    CreateTime = DateTime.Now,
                    CreateUserId = manage.Id,
                    LevelEnd = levelEnd,
                    LevelIcon = levelIcon,
                    LevelName = levelName,
                    LevelStart = levelStart
                };
                db.UserLevel.Add(userLevel);
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserLevel>();
            }
            return success;
        }

        /// <summary>
        /// 修改等级
        /// </summary>
        /// <param name="id"></param>
        /// <param name="levelName"></param>
        /// <param name="levelStart"></param>
        /// <param name="levelEnd"></param>
        /// <param name="levelIcon"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        public bool UpdateUserLevel(int id, string levelName, int levelStart, int levelEnd, string levelIcon, int createUserId)
        {
            AssertUtil.AreBigger(id, 0, LanguageUtil.Translate("api_Repository_UserLevel_UpdateUserLevel_id_AreBigger"));
            AssertUtil.NotNullOrWhiteSpace(levelName, LanguageUtil.Translate("api_Repository_UserLevel_UpdateUserLevel_levelName_NotNullOrWhiteSpace"));
            AssertUtil.AreBigger(levelEnd, levelStart, LanguageUtil.Translate("api_Repository_UserLevel_UpdateUserLevel_levelEnd_AreBigger"));
            AssertUtil.NotNullOrWhiteSpace(levelIcon, LanguageUtil.Translate("api_Repository_UserLevel_UpdateUserLevel_levelIcon_NotNullOrWhiteSpace"));
            AssertUtil.AreBigger(createUserId, 0, LanguageUtil.Translate("api_Repository_UserLevel_UpdateUserLevel_createUserId_AreBigger"));
            var success = Execute<bool>(db =>
            {
                var manage = db.Manage.FirstOrDefault(m => m.State == false && m.Id == createUserId);
                AssertUtil.IsNotNull(manage, LanguageUtil.Translate("api_Repository_UserLevel_UpdateUserLevel_createUserId_IsNotNull"));
                var userLevel = db.UserLevel.FirstOrDefault(ul => ul.State == false && ul.Id == id);
                AssertUtil.IsNotNull(userLevel,LanguageUtil.Translate("api_Repository_UserLevel_UpdateUserLevel_id_IsNotNull"));
                userLevel.UpdateTime = DateTime.Now;
                userLevel.UpdateUserId = manage.Id;
                userLevel.LevelEnd = levelEnd;
                userLevel.LevelIcon = levelIcon;
                userLevel.LevelName = levelName;
                userLevel.LevelStart = levelStart;
                db.Entry(userLevel).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserLevel>();
            }
            return success;
        }
    }
}