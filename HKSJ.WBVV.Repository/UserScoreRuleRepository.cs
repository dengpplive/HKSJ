



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

    public class UserScoreRuleRepository : BaseRepository, IUserScoreRuleRepository
    {
        public IQueryable<UserScoreRule> GetEntityList()
        {
            return base.GetEntityList<UserScoreRule>();
        }

        public IQueryable<UserScoreRule> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserScoreRule>(orderCondtion);
        }

        public IQueryable<UserScoreRule> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserScoreRule>(orderCondtions);
        }

        public IQueryable<UserScoreRule> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserScoreRule>(condtion);
        }

        public IQueryable<UserScoreRule> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserScoreRule>(condtion, orderCondtion);
        }

        public IQueryable<UserScoreRule> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserScoreRule>(condtion, orderCondtions);
        }

        public IQueryable<UserScoreRule> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserScoreRule>(condtions);
        }

        public IQueryable<UserScoreRule> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserScoreRule>(condtions, orderCondtion);
        }

        public IQueryable<UserScoreRule> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserScoreRule>(condtions, orderCondtions);
        }
        public UserScoreRule GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserScoreRule>(condtion);
        }

        public UserScoreRule GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserScoreRule>(condtions);
        }
        public bool CreateEntity(UserScoreRule entity)
        {
            return base.CreateEntity<UserScoreRule>(entity);
        }

        public void CreateEntitys(IList<UserScoreRule> entitys)
        {
            base.CreateEntitys(entitys);
        }

        public bool UpdateEntity(UserScoreRule entity)
        {
            return base.UpdateEntity(entity);
        }

        public void UpdateEntitys(IList<UserScoreRule> entitys)
        {
            base.UpdateEntitys<UserScoreRule>(entitys);
        }

        public bool DeleteEntity(UserScoreRule entity)
        {
            return base.DeleteEntity<UserScoreRule>(entity);
        }

        public void DeleteEntitys(IList<UserScoreRule> entitys)
        {
            base.DeleteEntitys<UserScoreRule>(entitys);
        }
        /// <summary>
        /// 添加积分规则
        /// </summary>
        /// <param name="name"></param>
        /// <param name="score"></param>
        /// <param name="limitScore"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        public bool CreateUserScoreRule(string name, int score, int limitScore, int createUserId)
        {
            AssertUtil.NotNullOrWhiteSpace(name, LanguageUtil.Translate("api_Repository_UserScoreRule_CreateUserScoreRule_name_NotNullOrWhiteSpace"));
            AssertUtil.AreBigger(score, 0, LanguageUtil.Translate("api_Repository_UserScoreRule_CreateUserScoreRule_score_AreBigger"));
            AssertUtil.AreBigger(limitScore, score, LanguageUtil.Translate("api_Repository_UserScoreRule_CreateUserScoreRule_limitScore_AreBigger"));
            AssertUtil.AreBigger(createUserId, 0, LanguageUtil.Translate("api_Repository_UserScoreRule_CreateUserScoreRule_createUserId_AreBiggere"));
            var success = Execute<bool>(db =>
            {
                var manage = db.Manage.FirstOrDefault(m => m.State == false && m.Id == createUserId);
                AssertUtil.IsNotNull(manage, LanguageUtil.Translate("api_Repository_UserScoreRule_CreateUserScoreRule_createUserId_IsNotNul"));
                var userScoreRule = new UserScoreRule()
                {
                    CreateTime = DateTime.Now,
                    CreateUserId = manage.Id,
                    Name = name,
                    Score = score,
                    LimitScore = limitScore
                };
                db.UserScoreRule.Add(userScoreRule);
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserScoreRule>();
            }
            return success;
        }

        /// <summary>
        /// 修改积分规则
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="score"></param>
        /// <param name="limitScore"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        public bool UpdateUserScoreRule(int id,string name, int score, int limitScore, int createUserId)
        {
            AssertUtil.AreBigger(id, 0, LanguageUtil.Translate("api_Repository_UserScoreRule_UpdateUserScoreRule_id_AreBigger"));
            AssertUtil.NotNullOrWhiteSpace(name, LanguageUtil.Translate("api_Repository_UserScoreRule_UpdateUserScoreRule_name_NotNullOrWhiteSpace"));
            AssertUtil.AreBigger(score, 0, LanguageUtil.Translate("api_Repository_UserScoreRule_UpdateUserScoreRule_score_AreBigger"));
            AssertUtil.AreBigger(limitScore, score, LanguageUtil.Translate("api_Repository_UserScoreRule_UpdateUserScoreRule_limitScore_AreBigger"));
            AssertUtil.AreBigger(createUserId, 0, LanguageUtil.Translate("api_Repository_UserScoreRule_UpdateUserScoreRule_createUserId_AreBigger"));
            var success = Execute<bool>(db =>
            {
                var manage = db.Manage.FirstOrDefault(m => m.State == false && m.Id == createUserId);
                AssertUtil.IsNotNull(manage, LanguageUtil.Translate("api_Repository_UserScoreRule_UpdateUserScoreRule_createUserId_IsNotNull"));
                var userScoreRule = db.UserScoreRule.FirstOrDefault(ul => ul.State == false && ul.Id == id);
                AssertUtil.IsNotNull(userScoreRule, LanguageUtil.Translate("api_Repository_UserScoreRule_UpdateUserScoreRule_id_IsNotNull"));
                userScoreRule.UpdateTime = DateTime.Now;
                userScoreRule.UpdateUserId = manage.Id;
                userScoreRule.Name = name;
                userScoreRule.Score = score;
                userScoreRule.LimitScore = limitScore;
                db.Entry(userScoreRule).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserScoreRule>();
            }
            return success;
        }

        /// <summary>
        /// 禁用积分规则
        /// </summary>
        /// <param name="id"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        public bool DisableUserScoreRule(int id,int createUserId)
        {
            AssertUtil.AreBigger(id, 0, LanguageUtil.Translate("api_Repository_UserScoreRule_DisableUserScoreRule_id_AreBigger"));
            AssertUtil.AreBigger(createUserId, 0, LanguageUtil.Translate("api_Repository_UserScoreRule_DisableUserScoreRule_createUserId_AreBiggere"));
            var success = Execute<bool>(db =>
            {
                var manage = db.Manage.FirstOrDefault(m => m.State == false && m.Id == createUserId);
                AssertUtil.IsNotNull(manage, LanguageUtil.Translate("api_Repository_UserScoreRule_DisableUserScoreRule_createUserId_IsNotNull"));
                var userScoreRule = db.UserScoreRule.FirstOrDefault(ul => ul.State == false && ul.Id == id);
                AssertUtil.IsNotNull(userScoreRule, LanguageUtil.Translate("api_Repository_UserScoreRule_DisableUserScoreRule_id_IsNotNull"));
                userScoreRule.UpdateTime = DateTime.Now;
                userScoreRule.UpdateUserId = manage.Id;
                userScoreRule.State = true;
                db.Entry(userScoreRule).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserScoreRule>();
            }
            return success;
        }
        /// <summary>
        /// 启用积分规则
        /// </summary>
        /// <param name="id"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        public bool EnabledUserScoreRule(int id, int createUserId)
        {
            AssertUtil.AreBigger(id, 0, LanguageUtil.Translate("api_Repository_UserScoreRule_EnabledUserScoreRule_id_AreBigger"));
            AssertUtil.AreBigger(createUserId, 0, LanguageUtil.Translate("api_Repository_UserScoreRule_EnabledUserScoreRule_createUserId_AreBiggere"));
            var success = Execute<bool>(db =>
            {
                var manage = db.Manage.FirstOrDefault(m => m.State == false && m.Id == createUserId);
                AssertUtil.IsNotNull(manage, LanguageUtil.Translate("api_Repository_UserScoreRule_EnabledUserScoreRule_createUserId_IsNotNull"));
                var userScoreRule = db.UserScoreRule.FirstOrDefault(ul => ul.State&& ul.Id == id);
                AssertUtil.IsNotNull(userScoreRule, LanguageUtil.Translate("api_Repository_UserScoreRule_EnabledUserScoreRule_id_IsNotNull"));
                userScoreRule.UpdateTime = DateTime.Now;
                userScoreRule.UpdateUserId = manage.Id;
                userScoreRule.State = false;
                db.Entry(userScoreRule).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserScoreRule>();
            }
            return success;
        }
    }
}