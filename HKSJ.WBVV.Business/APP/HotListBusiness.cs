using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Business.Interface.APP;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Entity.Response.App;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Repository.Interface;

namespace HKSJ.WBVV.Business.APP
{
    /// <summary>
    /// 热榜
    /// </summary>
    public class HotListBusiness : BaseBusiness, IHotListBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFansRepository _userFansRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly IVideoPrereviewRepository _videoPrereviewRepository;
        private readonly IUserReportRepository _userReportRepository;
        public HotListBusiness(IUserRepository userRepository, IUserFansRepository userFansRepository, IVideoRepository videoRepository, ICommentsRepository commentsRepository, IVideoPrereviewRepository videoPrereviewRepository, IUserReportRepository userReportRepository)
        {
            _userRepository = userRepository;
            _userFansRepository = userFansRepository;
            _videoRepository = videoRepository;
            _commentsRepository = commentsRepository;
            _videoPrereviewRepository = videoPrereviewRepository;
            _userReportRepository = userReportRepository;
        }

        #region 是否审核
        /// <summary>
        /// 是否审核过
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="videoId">视频编号</param>
        /// <returns>审核过（true）,未审核过（false）</returns>
        private bool IsVideoPrereview(int loginUserId, int videoId)
        {
            if (videoId <= 0)
            {
                return false;
            }
            var videoPrereview = (from vp in this._videoPrereviewRepository.GetEntityList(CondtionEqualState())
                                  join v in this._videoRepository.GetEntityList(CondtionEqualState()) on vp.VideoId equals v.Id
                                  join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                  where v.VideoSource
                                        && vp.UserId == loginUserId
                                        && vp.VideoId == videoId
                                  select vp).Any();
            return videoPrereview;
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

        #region 星播客

        /// <summary>
        /// 星播客(上传视频排行)
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        public AppUsersView UploadRank(int? loginUserId, int pageSize, int pageIndex)
        {
            AppUsersView users = new AppUsersView()
            {
                UserInfos = new List<AppUserView>()
            };
            var userId = loginUserId.HasValue ? Convert.ToInt32(loginUserId) : 0;
            IQueryable<AppUserView> uploadRank = from user in this._userRepository.GetEntityList(CondtionEqualState())
                                                 join temp in
                                                     (from u in this._userRepository.GetEntityList(CondtionEqualState())
                                                      join v in this._videoRepository.GetEntityList(CondtionEqualState()) on u.Id equals v.CreateManageId
                                                      where v.VideoSource
                                                            && v.VideoState == 3
                                                      group new { u, v } by new { u.Id } into g
                                                      orderby g.Count() descending
                                                      select new
                                                      {
                                                          Id = g.Key.Id,
                                                          c = g.Count()
                                                      })
                                                  on user.Id equals temp.Id
                                                 select UserView(userId, user, this._userFansRepository.GetEntityList());
            users.UserInfos = PageList(uploadRank, pageSize, pageIndex);
            return users;
        }
        #endregion

        #region 搜索结果

        /// <summary>
        /// 搜索结果
        /// </summary>
        /// <param name="content">搜索内容</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        public AppSerachView Serach(string content, int pageSize, int pageIndex)
        {
            AppSerachView serach = new AppSerachView()
            {
                UserInfos = new List<AppUserSimpleView>(),
                VideoInfos = new List<AppVideoView>()
            };
            IQueryable<AppUserSimpleView> users = (from u in this._userRepository.GetEntityList(CondtionEqualState())
                                                   where (u.Account.Contains(content) || (u.NickName != null && u.NickName.Contains(content)))
                                                   orderby u.FansCount descending
                                                   select UserEasyView(u));
            IQueryable<AppVideoView> videos = (from v in this._videoRepository.GetEntityList(CondtionEqualState())
                                               join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                               where v.VideoSource
                                                     && v.VideoState == 3
                                                     && ((v.Tags != null && v.Tags.Contains(content)) || (v.Title != null && v.Title.Contains(content)))
                                               select VideoView(v));
            serach.UserInfos = PageList(users, 10, 1);
            serach.VideoInfos = PageList(videos, pageSize, pageIndex);
            return serach;
        }

        #endregion

        #region 搜索--视频详情
        /// <summary>
        /// 搜索--视频详情
        /// </summary>
        /// <param name="content">搜索内容</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        public AppSerachVideoView SerachVideo(string content, int pageSize, int pageIndex)
        {
            AppSerachVideoView serach = new AppSerachVideoView()
            {
                TotalCount = 0,
                VideoInfos = new List<AppVideoView>()
            };
            IQueryable<AppVideoView> videos = (from v in this._videoRepository.GetEntityList(CondtionEqualState())
                                               join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                               where v.VideoSource
                                                     && v.VideoState == 3
                                                     && ((v.Tags != null && v.Tags.Contains(content)) || (v.Title != null && v.Title.Contains(content)))
                                               select VideoView(v));
            serach.TotalCount = videos.Count();
            serach.VideoInfos = PageList(videos, pageSize, pageIndex);
            return serach;
        }
        #endregion

        #region 搜索--用户详情

        /// <summary>
        /// 搜索--用户详情
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="content">搜索内容</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        public AppSerachUserView SerachUser(int? loginUserId,string content, int pageSize, int pageIndex)
        {
            AppSerachUserView serach = new AppSerachUserView()
            {
                TotalCount = 0,
                UserInfos = new List<AppUserView>()
            };
            if (loginUserId.HasValue)
            {
                var userId = Convert.ToInt32(loginUserId);
                IQueryable<AppUserView> users = (from u in this._userRepository.GetEntityList(CondtionEqualState())
                                                 where (u.Account.Contains(content) || (u.NickName != null && u.NickName.Contains(content)))
                                                 orderby u.FansCount descending
                                                 select UserSimpleView(userId, u, this._userFansRepository.GetEntityList()));
                serach.TotalCount = users.Count();
                serach.UserInfos = PageList(users, pageSize, pageIndex);
            }
            else
            {
                IQueryable<AppUserView> users = (from u in this._userRepository.GetEntityList(CondtionEqualState())
                                                 where (u.Account.Contains(content) || (u.NickName != null && u.NickName.Contains(content)))
                                                 orderby u.FansCount descending
                                                 select UserSimpleView(u));
                serach.TotalCount = users.Count();
                serach.UserInfos = PageList(users, pageSize, pageIndex);
            }
            return serach;
        }
        #endregion

        #region 审片

        /// <summary>
        /// 审片
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <returns></returns>
        public AppVideoView VideoPrereview(int loginUserId)
        {
            AppVideoView video = new AppVideoView();
            var isLogin = IsLogin(this._userRepository.GetEntityList(), loginUserId);
            if (isLogin)
            {
                IQueryable<AppVideoView> queryVideos = (from v in this._videoRepository.GetEntityList(CondtionEqualState())
                                                        join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                                        where v.VideoSource == true
                                                             && v.VideoState == 2 //正在审核中
                                                             && u.Id != loginUserId //自己的视频不显示
                                                             && IsVideoPrereview(loginUserId, (int)v.Id) //未审核过的视频
                                                        orderby v.UpdateTime descending
                                                        select VideoView(v, u));
                video = queryVideos.FirstOrDefault();
            }
            return video;
        }
        #endregion

        #region 差评
        /// <summary>
        /// 差评
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="videoId">视频编号</param>
        /// <returns></returns>
        public bool Bad(int loginUserId, int videoId)
        {
            return this._videoPrereviewRepository.Bad(loginUserId, videoId);
        }

        #endregion

        #region 必火
        /// <summary>
        /// 必火
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="videoId">视频编号</param>
        /// <returns></returns>
        public bool Good(int loginUserId, int videoId)
        {
            return this._videoPrereviewRepository.Good(loginUserId, videoId);
        }
        #endregion

        #region 举报视频
        /// <summary>
        /// 举报视频
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="videoId">视频编号</param>
        /// <returns></returns>
        public bool ReportVideo(int loginUserId, int videoId)
        {
            return this._userReportRepository.ReportVideo(loginUserId, videoId);
        }
        #endregion
    }
}
