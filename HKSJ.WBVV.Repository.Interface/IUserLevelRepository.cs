using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Repository.Interface.IBase;
using HKSJ.WBVV.Entity;

namespace HKSJ.WBVV.Repository.Interface
{
    public interface  IUserLevelRepository:IBaseAccess<UserLevel>
    {
        bool CreateUserLevel(string levelName, int levelStart, int levelEnd, string levelIcon, int createUserId);
        bool UpdateUserLevel(int id, string levelName, int levelStart, int levelEnd, string levelIcon, int createUserId);
    }
}