using System;
using System.Collections.Generic;
using System.Linq;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{
    /// <summary>
    /// 用户收藏
    /// Author : AxOne
    /// </summary>
    public class UserCollectBusiness : BaseBusiness, IUserCollectBusiness
    {
        private readonly IUserCollectRepository _userCollectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVideoRepository _videoRepository;
        public UserCollectBusiness(IUserCollectRepository userCollectRepository, IUserRepository userRepository, IVideoRepository videoRepository)
        {
            _userCollectRepository = userCollectRepository;
            _userRepository = userRepository;
            _videoRepository = videoRepository;
        }

        #region 用户收藏
        /// <summary>
        /// 获取用户收藏列表
        /// Author : AxOne
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public UserCollectResult GetUserCollectList(int userId, int pageIndex, int pageSize)
        {
            var result = new UserCollectResult { UserCollectViews = new List<UserCollectView>(), TotalCount = 0 };
            if (userId < 1) return result;
            User user;
            CheckUserId(userId, out user);
            var queryable = (from collect in _userCollectRepository.GetEntityList(CondtionEqualState())
                             join video in _videoRepository.GetEntityList(CondtionEqualState()) on collect.VideoId equals video.Id
                             where collect.UserId == userId && collect.State == false && video.VideoState == 3//TODO 刘强CheckState=1
                             orderby collect.UpdateTime descending
                             select new UserCollectView
                             {
                                 Id = collect.Id,
                                 SmallPicturePath = video.SmallPicturePath,
                                 UserId = collect.UserId,
                                 Title = video.Title,
                                 VideoId = collect.VideoId,
                                 CreateTime = Convert.ToDateTime(collect.UpdateTime).ToString("yyyy-MM-dd")
                             }).AsQueryable();
            if (!queryable.Any()) return result;
            result.TotalCount = queryable.Count();
            result.UserCollectViews = queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return result;
        }

        /// <summary>
        /// 收藏视频
        /// Author : AxOne
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CollectVideo(int videoId, int userId)
        {
            return this._userCollectRepository.CollectVideo(videoId, userId);
        }

        /// <summary>
        /// 取消收藏视频
        /// Author : AxOne
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="vid"></param>
        /// <returns></returns>
        public bool UnCollectVideo(int id, int userId, int vid)
        {
            return this._userCollectRepository.UnCollectVideo(id, userId, vid);
        }

        /// <summary>
        /// 取消收藏视频
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vid"></param>
        /// <returns></returns>
        public bool UnCollectVideo(int userId, int vid)
        {
            return this._userCollectRepository.UnCollectVideo(userId, vid);
        }

        /// <summary>
        /// 取消收藏所有视频
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DelAllCollectVideo(int userId)
        {
            return this._userCollectRepository.DelAllCollectVideo(userId);
        }
        #endregion

        #region 传入参数检测 Author : AxOne
        /// <summary>
        /// 检测用户是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        private void CheckUserId(int userId, out User user)
        {
            var condtions = new List<Condtion>
            {
                CondtionEqualState(),
                ConditionEqualId(userId)
            };
            user = _userRepository.GetEntity(condtions);
            AssertUtil.IsNotNull(user,LanguageUtil.Translate("api_Business_UserCollect_CheckUserId_userNotExist"));
        }

        /// <summary>
        /// 检测用户收藏编号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collect"></param>
        private void CheckId(int id, out UserCollect collect)
        {
            var condtionId = ConditionEqualId(id);
            collect = _userCollectRepository.GetEntity(condtionId);
            AssertUtil.IsNotNull(collect, LanguageUtil.Translate("api_Business_UserCollect_CheckId_conditionIdNotExist"));
        }

        /// <summary>
        /// 检测视频编号
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="video"></param>
        private void CheckVideoId(int videoId, out Video video)
        {
            var condtionId = ConditionEqualId(videoId);
            video = _videoRepository.GetEntity(condtionId);
            AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Business_UserCollect_CheckVideoId_videoIdNotExist"));
        }

        private void CheckCollectList(int uid, out IList<UserCollect> collects)
        {
            var condtions = new List<Condtion>
            {
                CondtionEqualState(),
                CondtionEqualUserId(uid)
            };
            collects = _userCollectRepository.GetEntityList(condtions).ToList();
            AssertUtil.IsNotNull(collects, LanguageUtil.Translate("api_Business_UserCollect_CheckCollectList_collectListIsNull"));
        }

        private void CheckUnCollect(int id, int uid, int videoId)
        {
            var condtions = new List<Condtion>
            {
                CondtionEqualState(true),
                CondtionEqualId(id),
                CondtionEqualUserId(uid),
                CondtionEqualVideoId(videoId)
            };
            var collect = _userCollectRepository.GetEntity(condtions);
            AssertUtil.IsNull(collect, LanguageUtil.Translate("api_Business_UserCollect_CheckUnCollect_collectionCancel"));
        }
        #endregion

        #region 传入参数 Author : AxOne
        /// <summary>
        /// 比较收藏编号相等
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static Condtion CondtionEqualId(int id)
        {
            var condtion = new Condtion
            {
                FiledName = "Id",
                FiledValue = id,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        /// <summary>
        /// 比较用户编号相等
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static Condtion CondtionEqualUserId(int userId)
        {
            var condtion = new Condtion
            {
                FiledName = "UserId",
                FiledValue = userId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        /// <summary>
        /// 比较视频编号相等
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        private static Condtion CondtionEqualVideoId(int videoId)
        {
            var condtion = new Condtion
            {
                FiledName = "VideoId",
                FiledValue = videoId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        #endregion

    }
}
