using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Repository.Interface.IBase;
using HKSJ.WBVV.Entity;

namespace HKSJ.WBVV.Repository.Interface
{
    public interface  IUserRepository:IBaseAccess<User>
    {
        void IncomeLogin(string account, string password, string ipAddress);
        void IncomeLogin(int userId, string ipAddress);
        void IncomeRegister(string account, string password, string ipAddress, int type);
        //第三方：绑定已有帐号并登录
        void ThirdPartyBindAndLogin(string account, string password, string typeCode, string relatedId, string nickName, string figureURL, string ipAddress);
        //第三方：注册新帐号并绑定
        void ThirdPartyBindAndRegister(string account, string password, string typeCode, string relatedId, string nickName, string figureURL, int type, string ipAddress);
        //第三方：自动创建新帐号并绑定（跳过）
        void AutoRegisterAndBindThirdParty(string typeCode, string relatedId, string nickName, string figureURL, string ipAddress);
    }
}