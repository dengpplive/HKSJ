using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Repository.Interface;

namespace HKSJ.WBVV.Business
{
    public class UserLevelBusiness : BaseBusiness, IUserLevelBusiness
    {
        private readonly IUserLevelRepository _userLevelRepository;
        private readonly IUserScoreRuleRepository _scoreRuleRepository;
        public UserLevelBusiness(IUserLevelRepository userLevelRepository, IUserScoreRuleRepository scoreRuleRepository)
        {
            _userLevelRepository = userLevelRepository;
            _scoreRuleRepository = scoreRuleRepository;
        }

        #region 等级管理
        /// <summary>
        /// 获取用户等级列表
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalIndex"></param>
        /// <returns></returns>
        public IList<UserLevel> GetUserLevels(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions, out int totalCount, out int totalIndex)
        {
            var userLevels = (from ul in this._userLevelRepository.GetEntityList()
                              orderby ul.CreateTime descending
                              select ul).AsQueryable();
            if (condtions != null && condtions.Count > 0)//查询条件
            {
                userLevels = userLevels.Query(condtions);
            }
            if (orderCondtions != null && orderCondtions.Count > 0)//排序条件
            {
                userLevels = userLevels.OrderBy(orderCondtions);
            }
            bool isExists = userLevels.Any();
            totalCount = isExists ? userLevels.Count() : 0;
            if (this.PageSize <= 0)
            {
                totalIndex = 0;
                var queryable = isExists
                    ? userLevels.ToList()
                    : new List<UserLevel>();
                return queryable;
            }
            else
            {
                totalIndex = totalCount % this.PageSize == 0
                    ? (totalCount / this.PageSize)
                    : (totalCount / this.PageSize + 1);
                if (this.PageIndex <= 0)
                {
                    this.PageIndex = 1;
                }
                if (this.PageIndex >= totalIndex)
                {
                    this.PageIndex = totalIndex;
                }

                var queryable = isExists
                    ? userLevels.Skip((this.PageIndex - 1) * this.PageSize).Take(this.PageSize).ToList()
                    : new List<UserLevel>();

                return queryable;
            }
        }
        /// <summary>
        /// 用户等级分页
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        public PageResult GetUserLevelPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            int totalCount = 0;
            int totalIndex = 0;
            IList<UserLevel> plateViews = GetUserLevels(condtions, orderCondtions, out totalCount, out totalIndex);
            return new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = totalCount,
                TotalIndex = totalIndex,
                Data = plateViews
            };
        }
        /// <summary>
        /// 获取用户等级信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserLevel GetUserLevel(int id)
        {
            UserLevel userLevel = new UserLevel();
            if (id > 0)
            {
                userLevel = this._userLevelRepository.GetEntity(ConditionEqualId(id)) ?? new UserLevel();
            }
            return userLevel;
        }

        /// <summary>
        /// 添加用户等级
        /// </summary>
        /// <param name="levelName"></param>
        /// <param name="levelStart"></param>
        /// <param name="levelEnd"></param>
        /// <param name="levelIcon"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        public bool CreateUserLevel(string levelName, int levelStart, int levelEnd, string levelIcon, int createUserId)
        {
            return this._userLevelRepository.CreateUserLevel(levelName, levelStart, levelEnd, levelIcon, createUserId);
        }
        /// <summary>
        /// 修改用户等级
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
            return this._userLevelRepository.UpdateUserLevel(id, levelName, levelStart, levelEnd, levelIcon, createUserId);
        }
        #endregion

        #region 积分规则管理
        /// <summary>
        /// 获取积分规则列表
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalIndex"></param>
        /// <returns></returns>
        public IList<UserScoreRule> GetUserScoreRules(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions, out int totalCount, out int totalIndex)
        {
            var userScoreRules = (from ul in this._scoreRuleRepository.GetEntityList()
                              orderby ul.CreateTime descending
                              select ul).AsQueryable();
            if (condtions != null && condtions.Count > 0)//查询条件
            {
                userScoreRules = userScoreRules.Query(condtions);
            }
            if (orderCondtions != null && orderCondtions.Count > 0)//排序条件
            {
                userScoreRules = userScoreRules.OrderBy(orderCondtions);
            }
            bool isExists = userScoreRules.Any();
            totalCount = isExists ? userScoreRules.Count() : 0;
            if (this.PageSize <= 0)
            {
                totalIndex = 0;
                var queryable = isExists
                    ? userScoreRules.ToList()
                    : new List<UserScoreRule>();
                return queryable;
            }
            else
            {
                totalIndex = totalCount % this.PageSize == 0
                    ? (totalCount / this.PageSize)
                    : (totalCount / this.PageSize + 1);
                if (this.PageIndex <= 0)
                {
                    this.PageIndex = 1;
                }
                if (this.PageIndex >= totalIndex)
                {
                    this.PageIndex = totalIndex;
                }

                var queryable = isExists
                    ? userScoreRules.Skip((this.PageIndex - 1) * this.PageSize).Take(this.PageSize).ToList()
                    : new List<UserScoreRule>();

                return queryable;
            }
        }
        /// <summary>
        /// 积分规则分页
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        public PageResult GetUserScoreRulePageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            int totalCount = 0;
            int totalIndex = 0;
            IList<UserScoreRule> plateViews = GetUserScoreRules(condtions, orderCondtions, out totalCount, out totalIndex);
            return new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = totalCount,
                TotalIndex = totalIndex,
                Data = plateViews
            };
        }
        /// <summary>
        /// 获取积分规则信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserScoreRule GetUserScoreRule(int id)
        {
            UserScoreRule userScoreRule = new UserScoreRule();
            if (id > 0)
            {
                userScoreRule = this._scoreRuleRepository.GetEntity(ConditionEqualId(id)) ?? new UserScoreRule();
            }
            return userScoreRule;
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
            return this._scoreRuleRepository.CreateUserScoreRule(name, score, limitScore, createUserId);
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
        public bool UpdateUserScoreRule(int id, string name, int score, int limitScore, int createUserId)
        {
            return this._scoreRuleRepository.CreateUserScoreRule(name, score, limitScore, createUserId);
        }
        /// <summary>
        /// 禁用积分规则
        /// </summary>
        /// <param name="id"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        public bool DisableUserScoreRule(int id, int createUserId)
        {
            return this._scoreRuleRepository.DisableUserScoreRule(id, createUserId);
        }
        /// <summary>
        /// 启用积分规则
        /// </summary>
        /// <param name="id"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        public bool EnabledUserScoreRule(int id, int createUserId)
        {
            return this._scoreRuleRepository.EnabledUserScoreRule(id, createUserId);
        }

        #endregion
    }
}
