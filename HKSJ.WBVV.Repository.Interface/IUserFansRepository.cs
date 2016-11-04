using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Repository.Interface.IBase;
using HKSJ.WBVV.Entity;

namespace HKSJ.WBVV.Repository.Interface
{
    public interface  IUserFansRepository:IBaseAccess<UserFans>
    {
        bool UserSubscribe(int loginUserId, int subscribeUserId);
        bool UserCancelSubscribe(int loginUserId, int subscribeUserId);
        bool UserSubscribeTransaction(int createUserId, int subscribeUserId, bool careState);
    }
}