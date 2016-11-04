using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface.APP;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Entity.Response.App;
using HKSJ.WBVV.Repository.Interface;

namespace HKSJ.WBVV.Business.APP
{
    /// <summary>
    /// 用户空间
    /// </summary>
    public class UserSpaceBusiness : BaseBusiness, IUserSpaceBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IUserFansRepository _userFansRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly IUserCollectRepository _userCollectRepository;
        private readonly IUserVisitLogRepository _userVisitLogRepository;
        private readonly IMessageReadRepository _messageReadRepository;
        private readonly ISysMessageRepository _sysMessageRepository;
        public UserSpaceBusiness(IUserRepository userRepository, IVideoRepository videoRepository, IUserFansRepository userFansRepository, ICommentsRepository commentsRepository, IUserCollectRepository userCollectRepository, IUserVisitLogRepository userVisitLogRepository, IMessageReadRepository messageReadRepository, ISysMessageRepository sysMessageRepository)
        {
            _userRepository = userRepository;
            _videoRepository = videoRepository;
            _userFansRepository = userFansRepository;
            _commentsRepository = commentsRepository;
            _userCollectRepository = userCollectRepository;
            _userVisitLogRepository = userVisitLogRepository;
            _messageReadRepository = messageReadRepository;
            _sysMessageRepository = sysMessageRepository;
        }

        #region 统计未读空间留言集合
        /// <summary>
        /// 统计未读空间留言集合
        /// </summary>
        /// <param name="user">浏览的用户</param>
        /// <returns></returns>
        private int UnreadSpaceCommentsCount(User user)
        {
            IQueryable<int> unreadVideoComments = (from sm in this._sysMessageRepository.GetEntityList(CondtionEqualState())
                                                   join c in this._commentsRepository.GetEntityList() on sm.EntityId equals c.Id
                                                   where IsDelete(this._messageReadRepository.GetEntityList(), user.Id, sm.Id) == false //未删除
                                                       && sm.EntityType == (int)SysMessageEnum.SpaceComment
                                                       && sm.SendType == (int)SystemMessageEnum.ByUser
                                                       && SplitUserBy(sm.ToUserIds).Contains(user.Id)
                                                       && IsRead(this._messageReadRepository.GetEntityList(), user.Id, sm.Id) == false
                                                       && c.State >= (int)CommentStateEnum.Waiting
                                                       && c.EntityType == (int)CommentEnum.User
                                                   select sm.Id);

            return unreadVideoComments.Count();
        }
        #endregion

        #region 统计用户上次浏览空间到现在的上传视频数量
        /// <summary>
        /// 统计用户上次浏览空间到现在的上传视频数量
        /// </summary>
        /// <param name="loginUser">登录用户或者被浏览的用户</param>
        /// <param name="subscribeUser">关注用户</param>
        /// <returns></returns>
        private int UploadCount(User loginUser, User subscribeUser)
        {
            int uploadCount = 0;
            if (loginUser != null) //登录
            {
                if (subscribeUser != null)
                {
                    if (loginUser.Id != subscribeUser.Id) //浏览他的空间
                    {
                        var userVisitLog = (from uv in this._userVisitLogRepository.GetEntityList()
                                            where uv.CreateUserId == loginUser.Id && uv.VisitedUserId == subscribeUser.Id
                                            select uv).FirstOrDefault();
                        if (userVisitLog != null)//访问过别人
                        {
                            uploadCount = (from v in this._videoRepository.GetEntityList(CondtionEqualState())
                                           join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId
                                               equals
                                               u.Id
                                           where v.VideoSource
                                                 && v.VideoState == 3
                                                 && v.CreateManageId == subscribeUser.Id
                                                 &&
                                                 Convert.ToDateTime(v.UpdateTime).Ticks >=
                                                 Convert.ToDateTime(userVisitLog.UpdateTime ?? userVisitLog.CreateTime).Ticks
                                           select v.Id).Count();

                        }
                        else//没有访问过别人
                        {
                            uploadCount = (from v in this._videoRepository.GetEntityList(CondtionEqualState())
                                           join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId
                                               equals
                                               u.Id
                                           where v.VideoSource
                                                 && v.VideoState == 3
                                                 && v.CreateManageId == subscribeUser.Id
                                           select v.Id).Count();
                        }
                    }
                    else//浏览自己空间
                    {
                        uploadCount = (from v in this._videoRepository.GetEntityList(CondtionEqualState())
                                       join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId
                                           equals
                                           u.Id
                                       where v.VideoSource
                                             && v.VideoState == 3
                                             && v.CreateManageId == loginUser.Id
                                       select v.Id).Count();
                    }
                }
            }
            else//用户未登录
            {
                if (subscribeUser != null)//浏览他的空间
                {
                    uploadCount = (from v in this._videoRepository.GetEntityList(CondtionEqualState())
                                   join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                   where v.VideoSource
                                         && v.VideoState == 3
                                         && v.CreateManageId == subscribeUser.Id
                                   select v.Id).Count();
                }
            }
            return uploadCount;
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

        #region 用户空间——他的空间
        /// <summary>
        /// 用户空间或者他的空间
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="userId">被浏览用户编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        public AppUserSpaceView UserSpace(int loginUserId, int userId, int pageSize, int pageIndex)
        {
            AppUserSpaceView userSpace = new AppUserSpaceView()
            {
                UserInfo = new AppUserView(),
                VideoInfos = new List<AppChoicenessView>()
            };
            User user;
            if (IsLogin(this._userRepository.GetEntityList(), loginUserId, out user))//登录
            {
                if (user.Id == userId || userId <= 0)//浏览我的空间
                {
                    userSpace.UserInfo = UserView(user.Id, UnreadSpaceCommentsCount(user), user, this._videoRepository.GetEntityList(), null);
                    IQueryable<AppChoicenessView> videos = (from v in this._videoRepository.GetEntityList(CondtionEqualState())
                                                            join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                                            where v.VideoSource
                                                                  && v.CreateManageId == loginUserId
                                                            orderby v.UpdateTime descending
                                                            select new AppChoicenessView()
                                                            {
                                                                IsSubed = false,
                                                                IsCollect = IsCollect(this._userCollectRepository.GetEntityList(), (int)v.Id, loginUserId),
                                                                UserInfo = UserEasyView(u),
                                                                VideoInfo = VideoView(v),
                                                                Comments = ParentComments((int)v.Id)
                                                            });
                    userSpace.VideoInfos = PageList(videos, pageSize, pageIndex);
                }
                else//浏览他的空间
                {
                    userSpace.UserInfo = (from u in this._userRepository.GetEntityList(CondtionEqualState())
                                          where u.Id == userId
                                          select UserView(loginUserId, UnreadSpaceCommentsCount(u), u, this._videoRepository.GetEntityList(), this._userFansRepository.GetEntityList())).FirstOrDefault();
                    IQueryable<AppChoicenessView> videos = (from v in this._videoRepository.GetEntityList(CondtionEqualState())
                                                            join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals
                                                                u.Id
                                                            where v.VideoSource
                                                                  && v.VideoState == 3
                                                                  && v.CreateManageId == userId
                                                            orderby v.UpdateTime descending
                                                            select new AppChoicenessView()
                                                            {
                                                                IsSubed = IsSubed(this._userFansRepository.GetEntityList(), u.Id, user.Id),
                                                                IsCollect = IsCollect(this._userCollectRepository.GetEntityList(), (int)v.Id, loginUserId),
                                                                UserInfo = UserEasyView(u),
                                                                VideoInfo = VideoView(v),
                                                                Comments = ParentComments((int)v.Id)
                                                            });
                    userSpace.VideoInfos = PageList(videos, pageSize, pageIndex);
                }
            }
            else//未登录
            {
                if (userId > 0)//浏览他的空间
                {
                    userSpace.UserInfo = (from u in this._userRepository.GetEntityList(CondtionEqualState())
                                          where u.Id == userId
                                          select UserView(u, UnreadSpaceCommentsCount(u), this._videoRepository.GetEntityList())).FirstOrDefault();
                    IQueryable<AppChoicenessView> videos = (from v in this._videoRepository.GetEntityList(CondtionEqualState())
                                                            join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                                            where v.VideoSource
                                                                && v.VideoState == 3
                                                                && v.CreateManageId == userId
                                                            orderby v.UpdateTime descending
                                                            select new AppChoicenessView()
                                                            {
                                                                IsSubed = false,
                                                                IsCollect = false,
                                                                UserInfo = UserEasyView(u),
                                                                VideoInfo = VideoView(v),
                                                                Comments = ParentComments((int)v.Id)
                                                            });

                    userSpace.VideoInfos = PageList(videos, pageSize, pageIndex);
                }
            }
            return userSpace;
        }

        #endregion

        #region 用户空间——关注
        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="userId">被浏览空间用户编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        public AppUserFanssView UserSubscribe(int loginUserId, int userId, int pageSize, int pageIndex)
        {
            AppUserFanssView userSubscribe = new AppUserFanssView()
            {
                TotalCount = 0,
                UserFanss = new List<AppUserFansView>()
            };
            User user;
            if (IsLogin(this._userRepository.GetEntityList(), loginUserId, out user))//（登录）
            {
                if (userId <= 0 || userId == user.Id)//浏览自己空间
                {
                    IQueryable<AppUserFansView> userSubscribes = (from uf in this._userFansRepository.GetEntityList(CondtionEqualState())
                                                                  join u in this._userRepository.GetEntityList(CondtionEqualState()) on uf.CreateUserId equals u.Id
                                                                  join subscribe in this._userRepository.GetEntityList(CondtionEqualState()) on uf.SubscribeUserId equals subscribe.Id
                                                                  where uf.CreateUserId == user.Id
                                                                  select UserFansView(UploadCount(user, u), uf, u, subscribe));
                    userSubscribe.TotalCount = userSubscribes.Count();
                    userSubscribe.UserFanss = PageList(userSubscribes, pageSize, pageIndex);
                }
                else //浏览他的空间
                {
                    IQueryable<AppUserFansView> userSubscribes = (from uf in this._userFansRepository.GetEntityList(CondtionEqualState())
                                                                  join u in this._userRepository.GetEntityList(CondtionEqualState()) on uf.CreateUserId equals u.Id
                                                                  join subscribe in this._userRepository.GetEntityList(CondtionEqualState()) on uf.SubscribeUserId equals subscribe.Id
                                                                  where uf.CreateUserId == userId && u.Id != loginUserId //看不到自己
                                                                  select UserFansView(user.Id, UploadCount(user, u), uf, u, subscribe, this._userFansRepository.GetEntityList()));
                    userSubscribe.TotalCount = userSubscribes.Count();
                    userSubscribe.UserFanss = PageList(userSubscribes, pageSize, pageIndex);
                }

            }
            else//（未登录）
            {
                if (userId > 0)
                {
                    IQueryable<AppUserFansView> userSubscribes = (from uf in this._userFansRepository.GetEntityList(CondtionEqualState())
                                                                  join u in this._userRepository.GetEntityList(CondtionEqualState()) on uf.CreateUserId equals u.Id
                                                                  join subscribe in this._userRepository.GetEntityList(CondtionEqualState()) on uf.SubscribeUserId equals subscribe.Id
                                                                  where uf.CreateUserId == userId
                                                                  select UserFansView(UploadCount(null, u), uf, u, subscribe));
                    userSubscribe.TotalCount = userSubscribes.Count();
                    userSubscribe.UserFanss = PageList(userSubscribes, pageSize, pageIndex);
                }
            }
            return userSubscribe;
        }
        #endregion

        #region 用户空间——粉丝

        /// <summary>
        /// 用户空间——粉丝
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="userId">被浏览的用户编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        public AppUserFanssView UserFans(int loginUserId, int userId, int pageSize, int pageIndex)
        {
            AppUserFanssView userFans = new AppUserFanssView()
            {
                TotalCount = 0,
                UserFanss = new List<AppUserFansView>()
            };
            User user;
            if (IsLogin(this._userRepository.GetEntityList(), loginUserId, out user)) //登录
            {
                if (userId <= 0 || userId == user.Id) //浏览我的空间
                {
                    IQueryable<AppUserFansView> userFanss = (from uf in this._userFansRepository.GetEntityList(CondtionEqualState())
                                                             join u in this._userRepository.GetEntityList(CondtionEqualState()) on uf.CreateUserId equals
                                                                 u.Id
                                                             join subscribe in this._userRepository.GetEntityList(CondtionEqualState()) on uf.SubscribeUserId
                                                                 equals subscribe.Id
                                                             where uf.SubscribeUserId == user.Id
                                                             select UserFansView(user.Id, uf, u, subscribe, this._userFansRepository.GetEntityList()));
                    userFans.TotalCount = userFanss.Count();
                    userFans.UserFanss = PageList(userFanss, pageSize, pageIndex);

                }
                else//浏览他的空间
                {
                    IQueryable<AppUserFansView> userFanss = (from uf in this._userFansRepository.GetEntityList(CondtionEqualState())
                                                             join u in this._userRepository.GetEntityList(CondtionEqualState()) on uf.CreateUserId equals
                                                                 u.Id
                                                             join subscribe in this._userRepository.GetEntityList(CondtionEqualState()) on uf.SubscribeUserId
                                                                 equals subscribe.Id
                                                             where uf.SubscribeUserId == userId && subscribe.Id!=user.Id//看不到自己
                                                             select UserFansView(loginUserId,uf, u, subscribe, this._userFansRepository.GetEntityList()));
                    userFans.TotalCount = userFanss.Count();
                    userFans.UserFanss = PageList(userFanss, pageSize, pageIndex);
                }
             
            }
            else//未登录
            {
                if (userId>0)
                {
                    IQueryable<AppUserFansView> userFanss = (from uf in this._userFansRepository.GetEntityList(CondtionEqualState())
                                                             join u in this._userRepository.GetEntityList(CondtionEqualState()) on uf.CreateUserId equals
                                                                 u.Id
                                                             join subscribe in this._userRepository.GetEntityList(CondtionEqualState()) on uf.SubscribeUserId
                                                                 equals subscribe.Id
                                                             where uf.SubscribeUserId == userId
                                                             select UserFansView(0,uf, u, subscribe,null));
                    userFans.TotalCount = userFanss.Count();
                    userFans.UserFanss = PageList(userFanss, pageSize, pageIndex);
                }
            }
            return userFans;
        }
        #endregion

        #region 用户空间--我的留言
        /// <summary>
        ///  精选——评论列表
        /// </summary>
        /// <param name="userId">登录用户编号或者浏览用户编号</param>
        /// <param name="pageSize">显示多少行</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        public AppComments SpaceComments(int userId, int pageSize, int pageIndex)
        {
            AppComments spaceComment = new AppComments()
            {
                TotalCount = 0,
                Comments = new List<AppCommentsView>()
            };
            if (userId>0)
            {
                IQueryable<AppCommentView> comments = (from c in this._commentsRepository.GetEntityList()
                                                       join u in this._userRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals u.Id
                                                       join fu in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals fu.Id
                                                       join tu in this._userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals tu.Id
                                                           into tuJoin
                                                       from toUser in tuJoin.DefaultIfEmpty()
                                                       orderby c.CreateTime descending
                                                       where c.State >= (int)CommentStateEnum.Waiting
                                                             && c.EntityType == (int)CommentEnum.User
                                                             && c.EntityId == userId
                                                             && c.ParentId == 0
                                                       select CommentView(c, fu, u)); //不要视频信息和接收用户信息
                spaceComment.TotalCount = comments.Count();
                var data = PageList(comments, pageSize, pageIndex);
                foreach (var parentCommnet in data)
                {
                    AppCommentsView commentView = new AppCommentsView()
                    {
                        ParentComment = parentCommnet,
                        ChildComments = (from c in this._commentsRepository.GetEntityList()
                                         join u in this._userRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals
                                             u.Id
                                         join fu in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId
                                             equals fu.Id
                                         join tu in this._userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals
                                             tu.Id
                                         orderby c.CreateTime descending
                                         where c.State >= (int)CommentStateEnum.Waiting
                                               && c.EntityType == (int)CommentEnum.User
                                               && c.EntityId == userId
                                               && c.LocalPath.StartsWith(parentCommnet.LocalPath)
                                         select CommentView(c, fu, u)).Take(2).ToList()
                    };
                    spaceComment.Comments.Add(commentView);
                }
            }
            return spaceComment;
        }
        #endregion

        #region 用户空间--我的留言--留言列表详情

        /// <summary>
        ///  我的留言--留言列表详情
        /// </summary>
        /// <param name="userId">被浏览用户编号</param>
        /// <param name="pid">发表评论的编号</param>
        /// <param name="pageSize">显示多少行</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        public AppCommentsView SpaceComments(int userId, int pid, int pageSize, int pageIndex)
        {
            AppCommentsView videoComment = new AppCommentsView()
            {
                ParentComment = new AppCommentView(),
                ChildComments = new List<AppCommentView>()
            };
            if (userId > 0)
            {
                var parentComment = (from c in this._commentsRepository.GetEntityList()
                                     join u in this._userRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals u.Id
                                     join fu in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals fu.Id
                                     join tu in this._userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals tu.Id
                                         into tuJoin
                                     from toUser in tuJoin.DefaultIfEmpty()
                                     orderby c.CreateTime descending
                                     where c.State >= (int)CommentStateEnum.Waiting
                                           && c.EntityType == (int)CommentEnum.User
                                           && c.EntityId == userId
                                           && c.ParentId == 0
                                           && c.Id == pid
                                     select CommentView(c, fu, u)).FirstOrDefault();
                if (parentComment != null)
                {
                    videoComment.ParentComment = parentComment;
                    IQueryable<AppCommentView> comments = (from c in this._commentsRepository.GetEntityList()
                                                           join u in this._userRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals u.Id
                                                           join fu in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals fu.Id
                                                           join tu in this._userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals tu.Id
                                                               into tuJoin
                                                           from toUser in tuJoin.DefaultIfEmpty()
                                                           orderby c.CreateTime descending
                                                           where c.State >= (int)CommentStateEnum.Waiting
                                                                 && c.EntityType == (int)CommentEnum.User
                                                                 && c.EntityId == userId
                                                                 && c.LocalPath.StartsWith(parentComment.LocalPath)
                                                           select CommentView(c, fu, u));
                    videoComment.ChildComments = PageList(comments, pageSize, pageIndex);
                }
            }
            return videoComment;
        }
        #endregion

        #region 用户空间--我的消息
        /// <summary>
        /// 我的消息
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        public AppMessageView Message(int loginUserId, int pageSize, int pageIndex)
        {
            AppMessageView message = new AppMessageView()
            {
                UnreadCollectionCount = 0,
                UnreadCommentCount = 0,
                SystemMessages = new List<AppSystemMessageView>()
            };
            User user;
            if (IsLogin(this._userRepository.GetEntityList(), loginUserId, out user))
            {
                message.UnreadCommentCount = (from sm in this._sysMessageRepository.GetEntityList(CondtionEqualState())
                                              join uc in this._userCollectRepository.GetEntityList(CondtionEqualState()) on sm.EntityId equals uc.Id
                                              where IsRead(this._messageReadRepository.GetEntityList(), user.Id, sm.Id) == false //未读
                                                    && IsDelete(this._messageReadRepository.GetEntityList(), user.Id, sm.Id) == false //未删除
                                                    && sm.EntityType == (int)SysMessageEnum.UserCollect
                                                    && sm.SendType == (int)SystemMessageEnum.ByUser
                                                    && SplitUserBy(sm.ToUserIds).Contains(user.Id)
                                              select sm).Count();
                message.UnreadCommentCount = (from sm in this._sysMessageRepository.GetEntityList(CondtionEqualState())
                                              join c in this._commentsRepository.GetEntityList(CondtionEqualState()) on sm.EntityId equals c.Id
                                              where IsRead(this._messageReadRepository.GetEntityList(), user.Id, sm.Id) == false //未读
                                                    && IsDelete(this._messageReadRepository.GetEntityList(), user.Id, sm.Id) == false//未删除
                                                    && sm.EntityType == (int)SysMessageEnum.VideoComment
                                                    && sm.SendType == (int)SystemMessageEnum.ByUser
                                                    && SplitUserBy(sm.ToUserIds).Contains(user.Id)
                                                    && c.EntityType == (int)CommentEnum.Video
                                                    && c.State >= 0
                                              select sm).Count();
                var sysMessages = (from sm in this._sysMessageRepository.GetEntityList(CondtionEqualState())
                                   where sm.CreateTime.Ticks >= user.CreateTime.Ticks //注册之后的系统消息
                                 && IsDelete(this._messageReadRepository.GetEntityList(), user.Id, sm.Id) == false //未删除
                                 && sm.EntityType == (int)SysMessageEnum.SystemMessage //消息类型为系统消息
                                 && (sm.SendType == (int)SystemMessageEnum.ByUser && SplitUserBy(sm.ToUserIds).Contains(UserId) || (sm.SendType == (int)SystemMessageEnum.AllUser))//全部用户
                                   select new AppSystemMessageView()
                                   {
                                       CreateTime = TimeSpan(sm.CreateTime),
                                       Content = sm.Content,
                                       Id = sm.Id
                                   });
                message.SystemMessages = PageList(sysMessages, pageSize, pageIndex);
            }
            return message;
        }
        #endregion

        #region 用户空间--我的消息——评论
        /// <summary>
        /// 我的消息--评论
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示多少页数</param>
        /// <returns></returns>
        public AppVideoCommentsView VideoComments(int loginUserId, int pageSize, int pageIndex)
        {
            AppVideoCommentsView comment = new AppVideoCommentsView()
            {
                TotalCount = 0,
                Comments = new List<AppVideoCommentView>()
            };
            if (IsLogin(this._userRepository.GetEntityList(), loginUserId))
            {
                IQueryable<AppVideoCommentView> comments = (from sm in this._sysMessageRepository.GetEntityList(CondtionEqualState())
                                                            join c in this._commentsRepository.GetEntityList() on sm.EntityId equals c.Id
                                                            join f in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals f.Id
                                                            join t in this._userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals t.Id
                                                            into tJoin
                                                            from toUser in tJoin.DefaultIfEmpty() //ToUserId为0的情况下
                                                            join v in this._videoRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals (int)v.Id
                                                            join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                                            where v.VideoState == 3
                                                                && c.State >= (int)CommentStateEnum.Waiting
                                                                && c.EntityType == (int)CommentEnum.Video
                                                                && IsDelete(this._messageReadRepository.GetEntityList(), loginUserId, sm.Id) == false //未删除
                                                                && sm.EntityType == (int)SysMessageEnum.VideoComment
                                                                && sm.SendType == (int)SystemMessageEnum.ByUser
                                                                && SplitUserBy(sm.ToUserIds).Contains(loginUserId)
                                                            select new AppVideoCommentView()
                                                            {
                                                                Id = sm.Id,
                                                                Content = sm.Content,
                                                                CreateTime = TimeSpan(sm.CreateTime),
                                                                IsRead = IsRead(this._messageReadRepository.GetEntityList(), loginUserId, sm.Id),
                                                                FromUser = new AppUserSimpleView()
                                                                {
                                                                    Id = f.Id,
                                                                    NickName = f.NickName,
                                                                    Picture = f.Picture
                                                                },
                                                                ToUser = toUser == null ? new AppUserSimpleView() : new AppUserSimpleView()
                                                                {
                                                                    Id = toUser.Id,
                                                                    NickName = toUser.NickName,
                                                                    Picture = toUser.Picture
                                                                }
                                                            });
                comment.TotalCount = comments.Count();
                var data = PageList(comments, pageSize, pageIndex);

                #region 添加已读记录
                var commentState = (from c in data
                                    where c.IsRead == false
                                    select new MessageRead()
                                    {
                                        MessageId = c.Id,
                                        UserId = loginUserId
                                    }).ToList();
                if (commentState.Count > 0)
                {
                    this._messageReadRepository.CreateEntitys(commentState);
                }
                #endregion

                comment.Comments = data;
            }
            return comment;
        }
        #endregion

        #region 用户空间--我的消息——喜欢
        /// <summary>
        ///  我的消息——评论
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="pageSize">显示多少行</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        public AppUserCollectsView VideoCollections(int loginUserId, int pageSize, int pageIndex)
        {
            AppUserCollectsView comment = new AppUserCollectsView()
            {
                TotalCount = 0,
                Comments = new List<AppUserCollectView>()
            };
            if (IsLogin(this._userRepository.GetEntityList(), loginUserId))
            {
                IQueryable<AppUserCollectView> comments = (from sm in this._sysMessageRepository.GetEntityList(CondtionEqualState())
                                                           join uc in this._userCollectRepository.GetEntityList(CondtionEqualState()) on sm.EntityId equals uc.Id
                                                           join v in this._videoRepository.GetEntityList(CondtionEqualState()) on uc.VideoId equals v.Id
                                                           join u in this._userRepository.GetEntityList(CondtionEqualState()) on uc.CreateUserId equals u.Id
                                                           where
                                                                IsDelete(this._messageReadRepository.GetEntityList(), loginUserId, sm.Id) == false//未删除
                                                                && sm.EntityType == (int)SysMessageEnum.UserCollect
                                                                && sm.SendType == (int)SystemMessageEnum.ByUser
                                                                && SplitUserBy(sm.ToUserIds).Contains(loginUserId)
                                                           select new AppUserCollectView()
                                                         {
                                                             Id = sm.Id,
                                                             Content = sm.Content,
                                                             CreateTime = TimeSpan(sm.CreateTime),
                                                             IsRead = IsRead(this._messageReadRepository.GetEntityList(), loginUserId, sm.Id),
                                                             FromUser = new AppUserSimpleView()
                                                             {
                                                                 Id = u.Id,
                                                                 NickName = u.NickName,
                                                                 Picture = u.Picture
                                                             },
                                                             Video = new AppVideoSimpleView()
                                                             {
                                                                 Id = (int)v.Id,
                                                                 Title = v.Title
                                                             }
                                                         });
                comment.TotalCount = comments.Count();
                var data = PageList(comments, pageSize, pageIndex);

                #region 添加已读记录
                var commentState = (from c in data
                                    where c.IsRead == false
                                    select new MessageRead()
                                    {
                                        MessageId = c.Id,
                                        UserId = loginUserId
                                    }).ToList();
                if (commentState.Count > 0)
                {
                    this._messageReadRepository.CreateEntitys(commentState);
                }
                #endregion

                comment.Comments = data;
            }
            return comment;
        }
        #endregion
    }
}
