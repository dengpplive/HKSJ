using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity.ViewModel.Client;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IUserCollectBusiness : IBaseBusiness
    {
        /// <summary>
        /// 获取收藏列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        UserCollectResult GetUserCollectList(int userId, int pageIndex, int pageSize);

        /// <summary>
        /// 收藏一个视频
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool CollectVideo(int videoId, int userId);

        /// <summary>
        /// 取消收藏一个视频
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="vid"></param>
        /// <returns></returns>
        bool UnCollectVideo(int id, int userId, int vid);

        /// <summary>
        /// 取消收藏一个视频
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vid"></param>
        /// <returns></returns>
        bool UnCollectVideo(int userId, int vid);

        /// <summary>
        /// 取消收藏所有视频
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool DelAllCollectVideo(int userId);
    }
}
