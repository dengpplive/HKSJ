using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Business.Interface.APP;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Entity.Response.App;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Repository.Interface;

namespace HKSJ.WBVV.Business.APP
{
    /// <summary>
    /// 精选
    /// </summary>
    public class ChoicenessBusiness : BaseBusiness, IChoicenessBusiness
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserCollectRepository _userCollectRepository;
        private readonly IUserFansRepository _userFansRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly IPlateVideoRepository _plateVideoRepository;
        private readonly IPlateRepository _plateRepository;
        private readonly IUserReportRepository _userReportRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ChoicenessBusiness(IVideoRepository videoRepository, IUserRepository userRepository,  IUserCollectRepository userCollectRepository, IUserFansRepository userFansRepository,  ICommentsRepository commentsRepository, IPlateVideoRepository plateVideoRepository, IPlateRepository plateRepository, IUserReportRepository userReportRepository, ICategoryRepository categoryRepository)
        {
            _videoRepository = videoRepository;
            _userCollectRepository = userCollectRepository;
            _userFansRepository = userFansRepository;
            _commentsRepository = commentsRepository;
            _plateVideoRepository = plateVideoRepository;
            _plateRepository = plateRepository;
            _userReportRepository = userReportRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        #region 是否存在视频
        /// <summary>
        /// 是否存在视频
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <returns>存在（true）不存在（false）</returns>
        private bool IsExistVideo(int videoId)
        {
            if (videoId <= 0)
            {
                return false;
            }
            var video = (from v in this._videoRepository.GetEntityList(CondtionEqualState())
                         where v.VideoSource
                               && v.VideoState == 3
                               && v.Id == videoId
                         select v).FirstOrDefault();
            return video != null;
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
            IList<AppCommentView> comments = (from c in this._commentsRepository.GetEntityList()
                                              join v in this._videoRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals v.Id
                                              join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                              join fu in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals fu.Id
                                              join tu in this._userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals tu.Id
                                              into tuJoin
                                              from toUser in tuJoin.DefaultIfEmpty()
                                              where c.State >= 0
                                                  && c.EntityType == (int)CommentEnum.Video
                                                  && c.EntityId == videoId
                                                  && c.ParentId == 0
                                                  && v.VideoState == 3
                                                  && v.VideoSource == true
                                              select CommentView(c, fu, u)).ToList();
            return comments;
        }

        #endregion

        #region 精选视频

        /// <summary>
        /// 精选视频
        /// </summary>
        /// <param name="loginUserId">登录用户编号（可空）</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        public AppChoicenesssView ChoicenessVideos(int? loginUserId, int pageSize, int pageIndex)
        {
            AppChoicenesssView choicenesss = new AppChoicenesssView()
            {
                Choiceness = new List<AppChoicenessView>()
            };
            var userId = loginUserId.HasValue ? Convert.ToInt32(loginUserId) : 0;
            var isLogin = IsLogin(this._userRepository.GetEntityList(), userId);
            IQueryable<AppChoicenessView> choicenessVideos = (from pv in this._plateVideoRepository.GetEntityList()
                                                              join p in this._plateRepository.GetEntityList() on pv.PlateId equals p.Id
                                                              join v in this._videoRepository.GetEntityList(CondtionEqualState()) on pv.VideoId equals v.Id
                                                              join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                                              where v.VideoSource == true
                                                                   && v.VideoState == 3
                                                              orderby v.UpdateTime descending
                                                              select new AppChoicenessView()
                                                              {
                                                                  IsSubed = isLogin && IsSubed(this._userFansRepository.GetEntityList(), u.Id, userId),
                                                                  IsCollect = isLogin && IsCollect(this._userCollectRepository.GetEntityList(), (int)v.Id, userId),
                                                                  UserInfo = UserEasyView(u),
                                                                  VideoInfo = VideoView(v),
                                                                  Comments = ParentComments((int)v.Id)
                                                              });
            choicenesss.Choiceness = PageList(choicenessVideos, pageSize, pageIndex);
            return choicenesss;
        }
        #endregion

        #region 朋友视频

        /// <summary>
        /// 朋友视频
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        public AppChoicenesssView FriendVideos(int loginUserId, int pageSize, int pageIndex)
        {
            AppChoicenesssView choicenesss = new AppChoicenesssView()
            {
                Choiceness = new List<AppChoicenessView>()
            };
            var isLogin = IsLogin(this._userRepository.GetEntityList(), loginUserId);
            IList<AppChoicenessView> friendVideos = new List<AppChoicenessView>();
            if (isLogin)
            {
                //登录用户关注的用户
                var userFans = (from uf in this._userFansRepository.GetEntityList(CondtionEqualState())
                                join u in this._userRepository.GetEntityList(CondtionEqualState()) on uf.SubscribeUserId equals u.Id
                                where uf.CreateUserId == loginUserId
                                orderby uf.CreateTime descending
                                select uf).AsQueryable();
                if (userFans.Any())
                {
                    foreach (var userFan in userFans)
                    {
                        //获取关注的用户上传的最新视频的第一条
                        var userVideo = (from v in this._videoRepository.GetEntityList(CondtionEqualState())
                                         join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                         where u.Id == userFan.SubscribeUserId
                                                && v.VideoState == 3
                                                && v.VideoSource
                                         orderby v.UpdateTime descending  //审核时间降序
                                         select new AppChoicenessView()
                                         {
                                             IsSubed = IsSubed(this._userFansRepository.GetEntityList(), u.Id, loginUserId),
                                             IsCollect = IsCollect(this._userCollectRepository.GetEntityList(), (int)v.Id, loginUserId),
                                             UserInfo = UserEasyView(u),
                                             VideoInfo = VideoView(v),
                                             Comments = ParentComments((int)v.Id)
                                         }).FirstOrDefault();
                        if (userVideo != null)
                        {
                            friendVideos.Add(userVideo);
                        }
                    }
                }
            }

            #region  视频上传时间降序

            var friendVideosSort = (from v in friendVideos
                                    orderby v.VideoInfo.CreateTime descending //视频上传时间降序
                                    select v).AsQueryable();
            #endregion

            choicenesss.Choiceness = PageList(friendVideosSort, pageSize, pageIndex);
            return choicenesss;
        }

        #endregion

        #region 精选--评论
        /// <summary>
        ///  精选——评论列表
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <param name="pageSize">显示多少行</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        public AppComments VideoComments(int videoId, int pageSize, int pageIndex)
        {
            AppComments videoComment = new AppComments()
           {
               TotalCount = 0,
               Comments = new List<AppCommentsView>()
           };
            if (IsExistVideo(videoId))
            {
                IQueryable<AppCommentView> comments = (from c in this._commentsRepository.GetEntityList()
                                                       join v in this._videoRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals v.Id
                                                       join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                                       join fu in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals fu.Id
                                                       join tu in this._userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals tu.Id
                                                       into tuJoin
                                                       from toUser in tuJoin.DefaultIfEmpty()
                                                       orderby c.CreateTime descending
                                                       where c.State >= 0
                                                          && c.EntityType == (int)CommentEnum.Video
                                                          && c.EntityId == videoId
                                                          && c.ParentId == 0
                                                          && v.VideoState == 3
                                                          && v.VideoSource
                                                       select CommentView(c, fu, u));//不要视频信息和接收用户信息
                videoComment.TotalCount = comments.Count();
                var data = PageList(comments, pageSize, pageIndex);
                foreach (var parentCommnet in data)
                {
                    AppCommentsView commentView = new AppCommentsView()
                    {
                        ParentComment = parentCommnet,
                        ChildComments = (from c in this._commentsRepository.GetEntityList()
                                         join v in this._videoRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals v.Id
                                         join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                         join fu in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals fu.Id
                                         join tu in this._userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals tu.Id
                                         orderby c.CreateTime descending
                                         where c.State >= 0
                                            && c.EntityType == (int)CommentEnum.Video
                                            && c.EntityId == videoId
                                            && c.LocalPath.StartsWith(parentCommnet.LocalPath)
                                            && v.VideoState == 3
                                            && v.VideoSource
                                         select CommentView(c, fu, u)).Take(2).ToList()
                    };
                    videoComment.Comments.Add(commentView);
                }
            }
            return videoComment;
        }
        #endregion

        #region 精选--评论--评论列表详情

        /// <summary>
        ///  精选--评论列表详情
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <param name="pid">发表评论的编号</param>
        /// <param name="pageSize">显示多少行</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        public AppCommentsView VideoComments(int videoId, int pid, int pageSize, int pageIndex)
        {
            AppCommentsView videoComment = new AppCommentsView()
            {
                ParentComment = new AppCommentView(),
                ChildComments = new List<AppCommentView>()
            };
            if (IsExistVideo(videoId))
            {
                var parentComment = (from c in this._commentsRepository.GetEntityList()
                                     join v in this._videoRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals v.Id
                                     join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                     join fu in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals fu.Id
                                     join tu in this._userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals tu.Id
                                         into tuJoin
                                     from toUser in tuJoin.DefaultIfEmpty()
                                     orderby c.CreateTime descending
                                     where c.State >= 0
                                           && c.EntityType == (int)CommentEnum.Video
                                           && c.EntityId == videoId
                                           && c.ParentId == 0
                                           && c.Id == pid
                                           && v.VideoState == 3
                                           && v.VideoSource
                                     select CommentView(c, fu, u)).FirstOrDefault();
                if (parentComment!=null)
                {
                    videoComment.ParentComment = parentComment;
                    IQueryable<AppCommentView> comments = (from c in this._commentsRepository.GetEntityList()
                                                           join v in this._videoRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals v.Id
                                                           join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                                           join fu in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals fu.Id
                                                           join tu in this._userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals tu.Id
                                                           into tuJoin
                                                           from toUser in tuJoin.DefaultIfEmpty()
                                                           orderby c.CreateTime descending
                                                           where c.State >= 0
                                                              && c.EntityType == (int)CommentEnum.Video
                                                              && c.EntityId == videoId
                                                              && c.LocalPath.StartsWith(parentComment.LocalPath)
                                                              && v.VideoState == 3
                                                              && v.VideoSource
                                                           select CommentView(c, fu, u));
                    videoComment.ChildComments = PageList(comments, pageSize, pageIndex);
                }
            }
            return videoComment;
        }
        #endregion

        #region 精选--喜欢
        /// <summary>
        ///  精选——喜欢列表
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <param name="pageSize">显示多少行</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        public AppUserCollectionsView VideoCollections(int videoId, int pageSize, int pageIndex)
        {
            AppUserCollectionsView videoCollections = new AppUserCollectionsView()
            {
                TotalCount = 0,
                UserCollections = new List<AppUserCollectionView>()
            };
            if (IsExistVideo(videoId))
            {
                IQueryable<AppUserCollectionView> userCollections = (from uc in this._userCollectRepository.GetEntityList(CondtionEqualState())
                                                                     join u in this._userRepository.GetEntityList(CondtionEqualState()) on uc.UserId equals u.Id
                                                                     join v in this._videoRepository.GetEntityList(CondtionEqualState()) on uc.VideoId equals v.Id
                                                                     join uploadUser in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals uploadUser.Id
                                                                     orderby uc.CreateTime descending
                                                                     where uc.VideoId == videoId
                                                                           && v.VideoSource
                                                                           && v.VideoState == 3
                                                                     select UserCollectionView(uc, u, v));
                videoCollections.TotalCount = userCollections.Count();
                videoCollections.UserCollections = PageList(userCollections, pageSize, pageIndex);
            }
            return videoCollections;
        }
        #endregion

        #region 频道——分类视频

        /// <summary>
        /// 频道——分类视频
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="cateId">分类编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少行</param>
        /// <returns></returns>
        public AppChoicenesssView Videos(int? loginUserId, int cateId, int pageSize, int pageIndex)
        {
            AppChoicenesssView choicenesss = new AppChoicenesssView()
            {
                Choiceness = new List<AppChoicenessView>()
            };
            var userId = loginUserId.HasValue ? Convert.ToInt32(loginUserId) : 0;
            var category = this._categoryRepository.GetEntityList(CondtionEqualState()).FirstOrDefault(c => c.Id == cateId);
            if (category!=null)
            {
                var isLogin = IsLogin(this._userRepository.GetEntityList(), userId);
                IQueryable<AppChoicenessView> choicenessVideos = (from v in this._videoRepository.GetEntityList(CondtionEqualState())
                                                                  join c in this._categoryRepository.GetEntityList(CondtionEqualState()) on v.CategoryId equals c.Id
                                                                  join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                                                  where v.VideoSource == true
                                                                       && v.VideoState == 3
                                                                       && c.LocationPath.StartsWith(category.LocationPath)
                                                                  orderby v.UpdateTime descending
                                                                  select new AppChoicenessView()
                                                                  {
                                                                      IsSubed = isLogin && IsSubed(this._userFansRepository.GetEntityList(), u.Id, userId),
                                                                      IsCollect = isLogin && IsCollect(this._userCollectRepository.GetEntityList(), (int)v.Id, userId),
                                                                      UserInfo = UserEasyView(u),
                                                                      VideoInfo = VideoView(v),
                                                                      Comments = ParentComments((int)v.Id)
                                                                  });
                choicenesss.Choiceness = PageList(choicenessVideos, pageSize, pageIndex);
            }
            return choicenesss;
        }
        #endregion

        #region 精选--评论--举报
        /// <summary>
        /// 举报评论
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="commentId">评论编号</param>
        /// <returns></returns>
        public bool ReportComment(int loginUserId, int commentId)
        {
            return this._userReportRepository.ReportComment(loginUserId, commentId);
        }
        #endregion

        #region 精选--关注用户

        /// <summary>
        /// 关注用户
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="subscribeUserId">关注用户编号</param>
        /// <returns></returns>
        public bool UserSubscribe(int loginUserId, int subscribeUserId)
        {
            return this._userFansRepository.UserSubscribe(loginUserId, subscribeUserId);
        }
        #endregion

        #region 精选--取消关注用户

        /// <summary>
        /// 取消关注用户
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="subscribeUserId">关注用户编号</param>
        /// <returns></returns>
        public bool UserCancelSubscribe(int loginUserId, int subscribeUserId)
        {
            return this._userFansRepository.UserCancelSubscribe(loginUserId, subscribeUserId);
        }
        #endregion

        #region 精选--发表视频评论
        /// <summary>
        /// 发表视频评论
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="videoId">视频编号</param>
        /// <param name="content">评论内容</param>
        /// <returns></returns>
        public int CreateVideoComment(int loginUserId, int videoId, string content)
        {
            return this._commentsRepository.CreateVideoComment(loginUserId, videoId, content);
        }

        #endregion

        #region 精选--回复视频评论
        /// <summary>
        /// 回复视频评论
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="videoId">视频编号</param>
        /// <param name="commentId">评论编号</param>
        /// <param name="content">评论内容</param>
        /// <returns></returns>
        public int ReplyVideoComment(int loginUserId, int videoId, int commentId, string content)
        {
            return this._commentsRepository.ReplyVideoComment(loginUserId, videoId, commentId, content);
        }

        #endregion

        #region 精选--喜欢视频
        /// <summary>
        /// 喜欢视频
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="videoId">视频编号</param>
        /// <returns></returns>
        public bool CollectVideo(int loginUserId, int videoId)
        {
            return this._userCollectRepository.CollectVideo(videoId, loginUserId);
        }
        #endregion

        #region 精选--取消喜欢
        /// <summary>
        /// 取消喜欢
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="videoId">视频编号</param>
        /// <returns></returns>
        public bool UnCollectVideo(int loginUserId, int videoId)
        {
            return this._userCollectRepository.UnCollectVideo(loginUserId, videoId);
        }
        #endregion
    }
}
