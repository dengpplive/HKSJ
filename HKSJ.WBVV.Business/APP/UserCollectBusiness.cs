using System.Linq;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Business.Base;
using System.Collections.Generic;
using HKSJ.WBVV.Entity.Response.App;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Business.Interface.APP;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;

namespace HKSJ.WBVV.Business.APP
{
    /// <summary>
    /// 收藏
    /// </summary>
    public class UserCollectBusiness : BaseBusiness, IUserCollectBusiness
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserCollectRepository _userCollectRepository;
        private readonly IUserFansRepository _userFansRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly IAuthKeysRepository _authKeysRepository;
        public UserCollectBusiness(IVideoRepository videoRepository, IUserRepository userRepository, IUserCollectRepository userCollectRepository, IUserFansRepository userFansRepository,  ICommentsRepository commentsRepository, IAuthKeysRepository authKeysRepository)
        {
            _videoRepository = videoRepository;
            _userRepository = userRepository;
            _userCollectRepository = userCollectRepository;
            _userFansRepository = userFansRepository;
            _commentsRepository = commentsRepository;
            _authKeysRepository = authKeysRepository;
        }

        /// <summary>
        /// 获取用户收藏列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public AppChoicenesssView GetUserCollects(int userId, int pageIndex, int pageSize)
        {
            var choicenesss = new AppChoicenesssView
            {
                Choiceness = new List<AppChoicenessView>()
            };
            var choicenessVideos = (
                from uc in _userCollectRepository.GetEntityList(Condtions(userId))
                join v in _videoRepository.GetEntityList(CondtionEqualState()) on uc.VideoId equals v.Id
                join u in _userRepository.GetEntityList(CondtionEqualState()) on uc.UserId equals u.Id
                where v.VideoSource && v.VideoState == 3
                orderby v.UpdateTime descending 
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

        #region 传入参数
        /// <summary>
        /// 比较用户Id相等
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static Condtion CondtionEqualUserId(int userId)
        {
            var condtion = new Condtion()
            {
                FiledName = "UserId",
                FiledValue = userId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        private IList<Condtion> Condtions(int userId)
        {
            return new List<Condtion> { CondtionEqualUserId(userId), CondtionEqualState() };
        }
        #endregion

    }
}
