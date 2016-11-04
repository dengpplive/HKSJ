using System.Collections.Generic;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Entity.ViewModel;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IUserLevelBusiness : IBaseBusiness
    {
        PageResult GetUserLevelPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
        UserLevel GetUserLevel(int id);
        bool CreateUserLevel(string levelName, int levelStart, int levelEnd, string levelIcon, int createUserId);
        bool UpdateUserLevel(int id, string levelName, int levelStart, int levelEnd, string levelIcon, int createUserId);

        PageResult GetUserScoreRulePageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
        UserScoreRule GetUserScoreRule(int id);
        bool CreateUserScoreRule(string name, int score, int limitScore, int createUserId);
        bool UpdateUserScoreRule(int id, string name, int score, int limitScore, int createUserId);
        bool DisableUserScoreRule(int id, int createUserId);
        bool EnabledUserScoreRule(int id, int createUserId);
    }
}