using HKSJ.WBVV.Entity.RequestPara;
using HKSJ.WBVV.Entity.Response.App;
using HKSJ.WBVV.Business.Interface.Base;

namespace HKSJ.WBVV.Business.Interface.APP
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public interface IUserBusiness : IBaseBusiness
    {
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        bool UpdateUserInfo(UpdateUserPara para);

        /// <summary>
        /// 获取用户播放历史
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        AppChoicenesssView GetUserHistories(int userId, int pageIndex, int pageSize);

        /// <summary>
        /// 清除所有播放历史
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool DelUserHistories(int userId);
    }
}
