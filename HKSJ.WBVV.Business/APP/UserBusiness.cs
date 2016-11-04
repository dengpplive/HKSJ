using System;
using System.Collections.Generic;
using System.Linq;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface.APP;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Entity.RequestPara;
using HKSJ.WBVV.Entity.Response.App;
using HKSJ.WBVV.Repository.Interface;

namespace HKSJ.WBVV.Business.APP
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserBusiness : BaseBusiness, IUserBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHistoryRepository _userHistoryRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IUserCollectRepository _userCollectRepository;
        private readonly IUserFansRepository _userFansRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly IAuthKeysRepository _authKeysRepository;

        /// <summary>
        /// UserBusiness
        /// </summary>
        public UserBusiness(IUserRepository userRepository, IUserHistoryRepository userHistoryRepository, IVideoRepository videoRepository, IUserCollectRepository userCollectRepository, IUserFansRepository userFansRepository, ICommentsRepository commentsRepository, IAuthKeysRepository authKeysRepository)
        {
            _userRepository = userRepository;
            _userHistoryRepository = userHistoryRepository;
            _videoRepository = videoRepository;
            _userCollectRepository = userCollectRepository;
            _userFansRepository = userFansRepository;
            _commentsRepository = commentsRepository;
            _authKeysRepository = authKeysRepository;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public bool UpdateUserInfo(UpdateUserPara para)
        {
            User user;
            CheckUserId(para.UserId, out user);
            user.Picture = para.Picture ?? "";
            user.NickName = para.NickName ?? "";
            user.Gender = para.Gender;
            user.Birthdate = para.Birthdate;
            user.City = para.City ?? "";
            user.Bardian = para.Bardian ?? "";
            user.BannerImage = para.BannerImage ?? "";
            user.UpdateTime = DateTime.Now;
            return _userRepository.UpdateEntity(user);
        }

        /// <summary>
        /// 获取用户播放历史
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public AppChoicenesssView GetUserHistories(int userId, int pageIndex, int pageSize)
        {
            var choicenesss = new AppChoicenesssView { Choiceness = new List<AppChoicenessView>() };
            var userHistories = _userHistoryRepository.GetEntityList(CondtionEqualUserId(userId));
            var choicenessVideos = (
                from uh in userHistories
                join v in _videoRepository.GetEntityList(CondtionEqualState()) on uh.VideoId equals v.Id
                join u in _userRepository.GetEntityList(CondtionEqualState()) on uh.UserId equals u.Id
                where v.VideoSource && v.VideoState == 3
                orderby uh.CreateTime descending
                select new AppChoicenessView
                {
                    IsSubed = IsSubed(_userFansRepository.GetEntityList(), u.Id, userId),
                    IsCollect = IsCollect(_userCollectRepository.GetEntityList(), (int)v.Id, userId),
                    UserInfo = UserEasyView(u),
                    VideoInfo = VideoView(v),
                    Comments = ParentComments((int)v.Id)
                });
            choicenesss.Choiceness = PageList(choicenessVideos, pageSize, pageIndex);
            return choicenesss;
        }

        /// <summary>
        /// 清除所有播放历史
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DelUserHistories(int userId)
        {
            var condtion = new Condtion
            {
                FiledName = "UserId",
                FiledValue = userId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            try
            {
                _userHistoryRepository.DeleteEntitys(_userHistoryRepository.GetEntityList(condtion).ToList());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region 传入参数检测
        /// <summary>
        /// 检测用户是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        private void CheckUserId(int userId, out User user)
        {
            user = _userRepository.GetEntity(ConditionEqualId(userId));
            AssertUtil.IsNotNull(user, "用户不存在");
        }

        /// <summary>
        /// 判断用户编号相等
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualUserId(int userId)
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
        #endregion

        #region 获取视频下的根级评论
        /// <summary>
        /// 获取视频下的根级评论
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <returns></returns>
        private IList<AppCommentView> ParentComments(int videoId)
        {
            IList<AppCommentView> comments = (
                from c in _commentsRepository.GetEntityList()
                join v in _videoRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals v.Id
                join u in _userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                join fu in _userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals fu.Id
                join tu in _userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals tu.Id
                into tuJoin
                from toUser in tuJoin.DefaultIfEmpty()
                where c.State >= 0
                    && c.EntityType == (int)CommentEnum.Video
                    && c.EntityId == videoId
                    && c.ParentId == 0
                    && v.VideoState == 3
                    && v.VideoSource
                select CommentView(c, fu,u)).ToList();
            return comments;
        }
        #endregion
    }
}
