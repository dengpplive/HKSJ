using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IAuthKeysBusiness : IBaseBusiness
    {
        /// <summary>
        /// 获取API验证密钥
        /// </summary>
        /// <param name="uid">用户编号</param>
        /// <param name="userType">用户类型(1:管理员  2:普通用户)</param>
        /// <returns>返回密钥</returns>
        AuthKeys GetAuthKeys(int uid, AuthUserType userType);

        /// <summary>
        /// 创建API验证密钥
        /// </summary>
        /// <param name="uid">用户编号</param>
        /// <param name="userType">用户类型(1:管理员  2:普通用户)</param>
        /// <returns>返回密钥</returns>
        string CreatePublicKey(int uid, AuthUserType userType);
    }
}