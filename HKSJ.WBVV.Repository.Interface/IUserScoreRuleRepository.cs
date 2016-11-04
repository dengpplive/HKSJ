using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Repository.Interface.IBase;
using HKSJ.WBVV.Entity;

namespace HKSJ.WBVV.Repository.Interface
{
    public interface  IUserScoreRuleRepository:IBaseAccess<UserScoreRule>
    {
        bool CreateUserScoreRule(string name, int score, int limitScore, int createUserId);
        bool UpdateUserScoreRule(int id, string name, int score, int limitScore, int createUserId);
        bool DisableUserScoreRule(int id, int createUserId);
        bool EnabledUserScoreRule(int id, int createUserId);
    }
}