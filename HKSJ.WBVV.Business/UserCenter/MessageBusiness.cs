using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Business.Interface.UserCenter;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Entity.ViewModel;
using System.Web;
using Autofac;
using HKSJ.WBVV.Common.Language;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Entity.Response.App;
using Lucene.Net.Messages;

namespace HKSJ.WBVV.Business.UserCenter
{
    /// <summary>
    /// 消息中心
    /// </summary>
    public class MessageBusiness : BaseBusiness, IMessageBusiness
    {
        private readonly IMessageReadRepository _messageReadRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly ISysMessageRepository _sysMessageRepository;
        public MessageBusiness(IMessageReadRepository messageReadRepository, IVideoRepository videoRepository, IUserRepository userRepository, ICommentsRepository commentsRepository, ISysMessageRepository sysMessageRepository)
        {
            _messageReadRepository = messageReadRepository;
            _videoRepository = videoRepository;
            _userRepository = userRepository;
            _commentsRepository = commentsRepository;
            _sysMessageRepository = sysMessageRepository;
        }
        /// <summary>
        /// 登录用户的所有消息
        /// </summary>
        /// <param name="user">登录用户</param>
        /// <returns></returns>
        private IQueryable<SysMessage> Sysmessages(User user)
        {
            IQueryable<SysMessage> sysMessages = (from sm in this._sysMessageRepository.GetEntityList(CondtionEqualState())
                                                  where (sm.SendType == (int)SystemMessageEnum.ByUser &&
                                                         SplitUserBy(sm.ToUserIds).Contains(user.Id) ||
                                                       (sm.SendType == (int)SystemMessageEnum.AllUser))
                                                  select sm);
            return sysMessages;
        }

        /// <summary> 
        /// 未读消息
        /// </summary>
        /// <param name="sysmessages">登录用户的所有消息</param>
        /// <param name="user">登录用户</param>
        /// <returns></returns>
        private IQueryable<SysMessage> UnreadSysmessage(IQueryable<SysMessage> sysmessages, User user)
        {
            IQueryable<SysMessage> sysMessages = (from sm in sysmessages
                                                  where IsRead(this._messageReadRepository.GetEntityList(), user.Id, sm.Id) == false
                                                  select sm);
            return sysMessages;
        }
        /// <summary>
        /// 未删消息
        /// </summary>
        /// <param name="sysmessages">登录用户的所有消息</param>
        /// <param name="user">登录用户</param>
        /// <returns></returns>
        private IQueryable<SysMessage> UncutSysmessage(IQueryable<SysMessage> sysmessages, User user)
        {
            IQueryable<SysMessage> sysMessages = (from sm in sysmessages
                                                  where IsDelete(this._messageReadRepository.GetEntityList(), user.Id, sm.Id) == false
                                                  select sm);
            return sysMessages;
        }

        #region 评论定位
        /// <summary>
        /// 根定位
        /// </summary>
        /// <param name="parentComment"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private int CommentPageIndex(Comments parentComment, int pageSize)
        {
            int parentPosition = 1;
            if (parentComment == null)
            {
                return parentPosition;
            }
            var commentBusiness = ((Autofac.IContainer)HttpRuntime.Cache["containerKey"]).Resolve<ICommentBusiness>();
            IQueryable<CommentView> parentComments = parentComment.EntityType == (int)CommentEnum.Video ? commentBusiness.GetParentCommentVideos(parentComment.EntityId) : commentBusiness.GetParentCommentUsers(parentComment.EntityId);
            if (parentComments != null)
            {
                foreach (var comment in parentComments)
                {
                    if (comment.Id == parentComment.Id)
                    {
                        break;
                    }
                    else
                    {
                        parentPosition++;
                    }
                }
            }
            return parentPosition % pageSize == 0
                ? (parentPosition / pageSize)
                : (parentPosition / pageSize + 1);
        }

        /// <summary>
        /// 子定位
        /// </summary>
        /// <param name="parentComment"></param>
        /// <param name="commentId"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private int CommentIndex(Comments parentComment, int commentId, int size)
        {
            int childPosition = 1;
            if (parentComment == null)
            {
                return childPosition;
            }
            var commentBusiness = ((Autofac.IContainer)HttpRuntime.Cache["containerKey"]).Resolve<ICommentBusiness>();
            IQueryable<CommentView> childComments = parentComment.EntityType == (int)CommentEnum.Video ? commentBusiness.GetChildCommentVideos(parentComment.EntityId, parentComment) : commentBusiness.GetChildCommentUsers(parentComment.EntityId, parentComment);
            if (childComments != null)
            {
                foreach (var childComment in childComments)
                {
                    if (childComment.Id == commentId)
                    {
                        break;
                    }
                    else
                    {
                        childPosition++;
                    }
                }
            }
            return childPosition % size == 0
                   ? (childPosition / size)
                   : (childPosition / size + 1);
        }
        /// <summary>
        /// 评论定位
        /// </summary>
        /// <param name="comment">评论</param>
        /// <param name="pageSize">父级显示数量</param>
        /// <param name="size">子级显示数量</param>
        /// <returns></returns>
        private dynamic CommentPosition(Comments comment, int pageSize, int size)
        {
            var parentComments = (from c in this._commentsRepository.GetEntityList()
                                  where comment.LocalPath.Contains(c.LocalPath)
                                  orderby c.LocalPath.Length
                                  select c).FirstOrDefault();
            var position = new
            {
                pageindex = CommentPageIndex(parentComments, pageSize),
                index = CommentIndex(parentComments, comment.Id, size),
            };
            return position;
        }
        #endregion

        #region Header消息
        /// <summary>
        ///  Header消息
        /// </summary>
        /// <param name="unreadSysmessage">未读消息</param>
        /// <param name="user">登录用户</param>
        /// <returns></returns>
        private MessageType GetHeaderMessage(IQueryable<SysMessage> unreadSysmessage, User user)
        {
            MessageType headerMessage = new MessageType()
            {
                MessageTypeId = (int)SysMessageEnum.SystemMessage,
                MessageTypeName = EnumHelper.GetEnumDescription(SysMessageEnum.SystemMessage),
                UnreadMessageCount = 0
            };
            if (user != null)
            {
                IQueryable<int> count = (from sm in unreadSysmessage
                                         group sm by sm.EntityType into g
                                         select g.Count());
                if (count.Any())
                {
                    var entityType = (from sm in unreadSysmessage
                                      orderby sm.CreateTime descending
                                      select sm.EntityType).FirstOrDefault();
                    int sum = 0;
                    foreach (var c in count)
                    {
                        sum += c;
                    }
                    headerMessage.UnreadMessageCount = sum;
                    headerMessage.MessageTypeId = entityType;
                    headerMessage.MessageTypeName = EnumHelper.GetEnumDescription((SysMessageEnum)Enum.Parse(typeof(SysMessageEnum), entityType.ToString()));
                    return headerMessage;
                }
            }
            return headerMessage;
        }
        public MessageType GetHeaderMessage()
        {
            User user;
            IsLogin(this._userRepository.GetEntityList(), UserId, out user);
            IQueryable<SysMessage> sysMessages = Sysmessages(user);
            //未读
            IQueryable<SysMessage> unreadSysmessage = UnreadSysmessage(sysMessages, user);
            return GetHeaderMessage(unreadSysmessage, user);
        }

        #endregion

        #region 系统消息,评论,留言
        private IList<MessageType> GetMessages(IQueryable<SysMessage> unreadSysmessage, User user)
        {
            IList<MessageType> messages = new List<MessageType>
            {
				//LanguageUtil.Translate("api_Business_UserCenter_Message_GetMessages_systemMessage")
                            new MessageType
                            {
                            MessageTypeId = (int)SysMessageEnum.SystemMessage,
                            MessageTypeName = EnumHelper.GetEnumDescription(SysMessageEnum.SystemMessage),
                            UnreadMessageCount = 0
                            },
                            new MessageType
                            {
                            MessageTypeId = (int)SysMessageEnum.VideoComment,
                            MessageTypeName = EnumHelper.GetEnumDescription(SysMessageEnum.VideoComment),
                            UnreadMessageCount = 0
                            },
                            new MessageType
                            {
                            MessageTypeId = (int)SysMessageEnum.SpaceComment,
                            MessageTypeName = EnumHelper.GetEnumDescription(SysMessageEnum.SpaceComment),
                            UnreadMessageCount = 0
                            }
            };
            if (user != null)
            {
                var entityTypes = (from sm in unreadSysmessage
                                   group sm by sm.EntityType into g
                                   orderby g.Key
                                   select new
                                   {
                                       EntityType = g.Key,
                                       Count = g.Count()
                                   }).ToList();
                if (entityTypes.Count > 0)
                {
                    IList<MessageType> newMessage = new List<MessageType>();
                    foreach (var message in messages)
                    {
                        var unreadMessage = entityTypes.FirstOrDefault(m => m.EntityType == message.MessageTypeId);
                        if (unreadMessage != null)
                        {
                            message.UnreadMessageCount = unreadMessage.Count;
                        }
                        newMessage.Add(message);
                    }
                    return newMessage;
                }
            }
            return messages;
        }
        public IList<MessageType> GetMessages()
        {
            User user;
            IsLogin(this._userRepository.GetEntityList(), UserId, out user);
            IQueryable<SysMessage> sysMessages = Sysmessages(user);
            //未读
            IQueryable<SysMessage> unreadSysmessage = UnreadSysmessage(sysMessages, user);
            return GetMessages(unreadSysmessage, user);
        }

        #endregion

        #region 系统消息

        #region 系统消息分页
        /// <summary>
        /// 系统消息分页
        /// </summary>
        /// <param name="sysmessages">登录用户所有的未删除的系统消息</param>
        /// <param name="user">登录用户</param>
        /// <returns></returns>
        public PageResult GetPageSystemMessages(IQueryable<SysMessage> sysmessages, User user)
        {
            var pageResult = new PageResult()
            {
                TotalCount = 0,
                Data = new List<SystemMessageView>(),
                PageIndex = this.PageIndex,
                PageSize = this.PageSize,
                TotalIndex = 0
            };
            if (user != null)
            {
                #region 系统消息

                var systemMessageViews = (from sm in sysmessages
                                          orderby sm.CreateTime descending
                                          select new SystemMessageView()
                                          {
                                              Id = sm.Id,
                                              IsRead = IsRead(this._messageReadRepository.GetEntityList(), user.Id, sm.Id),
                                              CreateTime = sm.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                              MessageContent = sm.Content
                                          });

                #endregion

                #region 分页
                int pageSize = this.PageSize <= 0 ? 8 : this.PageSize;
                int totalCount = systemMessageViews.Count();
                int totalIndex = totalCount % pageSize == 0 ? (totalCount / pageSize) : (totalCount / pageSize + 1);
                int pageIndex = this.PageIndex <= 0 ? 1 : (this.PageIndex >= totalIndex ? totalIndex : this.PageIndex);
                pageResult.PageIndex = pageIndex;
                pageResult.PageSize = pageSize;
                pageResult.PageIndex = pageIndex;
                pageResult.TotalCount = totalCount;
                var data = systemMessageViews.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pageResult.Data = data;
                #endregion

                #region 添加已读记录
                var messageRead = (from sm in data
                                   where sm.IsRead == false
                                   select new MessageRead()
                                   {
                                       MessageId = sm.Id,
                                       UserId = user.Id
                                   }).AsQueryable();
                if (messageRead.Any())
                {
                    this._messageReadRepository.CreateEntitys(messageRead.ToList());
                }
                #endregion
            }
            return pageResult;
        }
        public PageResult GetPageSystemMessages()
        {
            User user;
            IsLogin(this._userRepository.GetEntityList(), UserId, out user);
            IQueryable<SysMessage> sysMessages = Sysmessages(user);
            //未删除
            IQueryable<SysMessage> uncutSysmessage = UncutSysmessage(sysMessages, user);
            //未删除的系统消息
            IQueryable<SysMessage> systemMessages = (from sm in uncutSysmessage
                                                     where sm.CreateTime.Ticks >= user.CreateTime.Ticks
                                                           && sm.EntityType == (int)SysMessageEnum.SystemMessage
                                                     select sm);
            return GetPageSystemMessages(systemMessages, user);
        }


        #endregion

        #region 消息中心——切换系统消息
        public MessageView GetSystemMessages()
        {
            var message = new MessageView()
            {
                MessageType = new MessageType(),
                MessageTypes = new List<MessageType>(),
                PageResult = new PageResult()
            };
            User user;
            if (IsLogin(this._userRepository.GetEntityList(), UserId, out user))
            {
                IQueryable<SysMessage> sysMessages = Sysmessages(user);
                //未删除
                IQueryable<SysMessage> uncutSysmessage = UncutSysmessage(sysMessages, user);
                //未删除的系统消息
                IQueryable<SysMessage> systemMessages = (from sm in uncutSysmessage
                                                         where sm.CreateTime.Ticks >= user.CreateTime.Ticks
                                                               && sm.EntityType == (int)SysMessageEnum.SystemMessage
                                                         select sm);
                var pageResult = GetPageSystemMessages(systemMessages, user);
                //未读
                IQueryable<SysMessage> unreadSysmessage = UnreadSysmessage(sysMessages, user);
                var messageTypes = GetMessages(unreadSysmessage, user);
                var messageType = GetHeaderMessage(unreadSysmessage, user);
                message.MessageType = messageType;
                message.MessageTypes = messageTypes;
                message.PageResult = pageResult;
            }
            return message;
        }
        #endregion

        #region 消息中心——删除系统消息

        public bool DeleteSystemMessage(int messageId)
        {
            CheckUserId(this.UserId);
            User user;
            CheckUserId(this.UserId, out user);
            CheckMessageId(messageId);
            SysMessage systemMessage;
            CheckMessageId(messageId, user, out systemMessage);
            var messageRead = (from mr in this._messageReadRepository.GetEntityList()
                               where mr.MessageId == systemMessage.Id && mr.UserId == user.Id
                               select mr).FirstOrDefault();
            if (messageRead != null)
            {
                messageRead.State = (int)SystemMessageStateEnum.Deleted; //表示删除
                return this._messageReadRepository.UpdateEntity(messageRead);
            }
            else
            {
                var newMessageRead = new MessageRead()
                {
                    MessageId = systemMessage.Id,
                    UserId = user.Id,
                    State = (int)SystemMessageStateEnum.Deleted//表示删除
                };
                return this._messageReadRepository.CreateEntity(newMessageRead);
            }
        }

        #endregion

        #region 消息中心——清空系统消息

        public bool ClearSystemMessage()
        {
            CheckUserId(this.UserId);
            User user;
            CheckUserId(this.UserId, out user);
            var systemMessages = (from sm in this._sysMessageRepository.GetEntityList()
                                  where sm.State == false
                                        && sm.EntityType == (int)SysMessageEnum.SystemMessage
                                        && sm.CreateTime.Ticks >= user.CreateTime.Ticks //注册之后的系统消息
                                        && (
                                             (SplitUserBy(sm.ToUserIds).Contains(user.Id) && sm.SendType == (int)SystemMessageEnum.ByUser)
                                          || (string.IsNullOrEmpty(sm.ToUserIds) && sm.SendType == (int)SystemMessageEnum.AllUser)
                                          )
                                  select sm);

            //未读的系统消息
            var insertMessageRead = (from sm in systemMessages
                                     where IsRead(this._messageReadRepository.GetEntityList(), user.Id, sm.Id) == false
                                     select new MessageRead()
                                     {
                                         MessageId = sm.Id,
                                         State = (int)SystemMessageStateEnum.Deleted,
                                         UserId = user.Id
                                     }).ToList();
            //已读的系统消息
            var updateMessageRead = (from sm in systemMessages
                                     where IsRead(this._messageReadRepository.GetEntityList(), user.Id, sm.Id)
                                     select new MessageRead()
                                     {
                                         MessageId = sm.Id,
                                         State = (int)SystemMessageStateEnum.Deleted,
                                         UserId = user.Id
                                     }).ToList();

            if (insertMessageRead.Count > 0)
            {
                this._messageReadRepository.CreateEntitys(insertMessageRead);
            }
            if (updateMessageRead.Count > 0)
            {
                this._messageReadRepository.UpdateEntitys(updateMessageRead);
            }
            return true;
        }

        #endregion

        #endregion

        #region 视频评论

        #region 评论分页
        /// <summary>
        /// 评论分页
        /// </summary>
        /// <param name="videoComments">登录用户未删除的视频评论</param>
        /// <param name="loginUser"></param>
        /// <param name="parentSize"></param>
        /// <param name="childSize"></param>
        /// <returns></returns>
        private PageResult GetPageComments(IQueryable<SysMessage> videoComments, User loginUser, int parentSize, int childSize)
        {
            var pageResult = new PageResult()
            {
                TotalCount = 0,
                Data = new List<ViewCommentView>(),
                PageIndex = this.PageIndex,
                PageSize = this.PageSize,
                TotalIndex = 0
            };
            if (loginUser != null)
            {
                #region 评论列表

                 var videoCommentViews = (from sm in videoComments
                                         join c in this._commentsRepository.GetEntityList() on sm.EntityId equals c.Id
                                         join f in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals f.Id
                                         join t in this._userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals t.Id
                                         into tJoin
                                         from toUser in tJoin.DefaultIfEmpty() //ToUserId为0的情况下
                                         join v in this._videoRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals (int)v.Id
                                         join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                                         where c.State >= (int)CommentStateEnum.Waiting
                                           && c.EntityType == (int)CommentEnum.Video
                                           && v.VideoState == 3
                                           && v.VideoSource
                                         orderby c.CreateTime descending
                                         select new ViewCommentView()
                                         {
                                             Id = sm.Id,
                                             EntityId = sm.EntityId,
                                             Content = c.Content,
                                             FromUser = new UserSimpleView()
                                             {
                                                 Id = f.Id,
                                                 NickName = f.NickName,
                                                 Picture = f.Picture
                                             },
                                             ToUser = toUser == null ? null : new UserSimpleView()
                                             {
                                                 Id = toUser.Id,
                                                 NickName = toUser.NickName,
                                                 Picture = toUser.Picture
                                             },
                                             User = new UserSimpleView()
                                             {
                                                 Id = u.Id,
                                                 NickName = u.NickName,
                                                 Picture = u.Picture
                                             },
                                             Video = new VideoSimpleView()
                                             {
                                                 Id = (int)v.Id,
                                                 Title = v.Title
                                             },
                                             CreateTime = sm.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                             IsRead = IsRead(this._messageReadRepository.GetEntityList(), loginUser.Id, sm.Id),
                                             Position = CommentPosition(c, parentSize, childSize)
                                         });
                #endregion

                #region 分页
                int pageSize = this.PageSize <= 0 ? 8 : this.PageSize;
                int totalCount = videoCommentViews.Count();
                int totalIndex = totalCount % pageSize == 0 ? (totalCount / pageSize) : (totalCount / pageSize + 1);
                int pageIndex = this.PageIndex <= 0 ? 1 : (this.PageIndex >= totalIndex ? totalIndex : this.PageIndex);
                pageResult.PageIndex = pageIndex;
                pageResult.PageSize = pageSize;
                pageResult.PageIndex = pageIndex;
                pageResult.TotalCount = totalCount;
                var data = videoCommentViews.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pageResult.Data = data;
                #endregion

                #region 添加已读记录
                var commentState = (from c in data
                                    where c.IsRead == false
                                    select new MessageRead()
                                    {
                                        MessageId = c.Id,
                                        UserId = loginUser.Id
                                    }).ToList();
                if (commentState.Count > 0)
                {
                    this._messageReadRepository.CreateEntitys(commentState);
                }
                #endregion
            }
            return pageResult;
        }
        public PageResult GetPageComments(int parentSize, int childSize)
        {
            User user;
            IsLogin(this._userRepository.GetEntityList(), UserId, out user);
            IQueryable<SysMessage> sysMessages = Sysmessages(user);
            IQueryable<SysMessage> uncutSysmessage = UncutSysmessage(sysMessages, user);
            IQueryable<SysMessage> videoComments = (from sm in uncutSysmessage
                                                    where sm.EntityType == (int)SysMessageEnum.VideoComment
                                                    select sm);
            return GetPageComments(videoComments,user, parentSize, childSize);
        }

        #endregion

        #region 消息中心——切换评论
        public MessageView GetComments(int parentSize, int childSize)
        {
            var message = new MessageView()
            {
                MessageType = new MessageType(),
                MessageTypes = new List<MessageType>(),
                PageResult = new PageResult()
            };
            User user;
            if (IsLogin(this._userRepository.GetEntityList(), UserId, out user))
            {
                IQueryable<SysMessage> sysMessages = Sysmessages(user);
                IQueryable<SysMessage> uncutSysmessage = UncutSysmessage(sysMessages, user);
                IQueryable<SysMessage> videoComments = (from sm in uncutSysmessage
                                                        where sm.EntityType == (int)SysMessageEnum.VideoComment
                                                        select sm);
                var pageResult = GetPageComments(videoComments,user, parentSize, childSize);
                //未读
                IQueryable<SysMessage> unreadSysmessage = UnreadSysmessage(sysMessages, user);
                var messageTypes = GetMessages(unreadSysmessage, user);
                var messageType = GetHeaderMessage(unreadSysmessage, user);
                message.MessageType = messageType;
                message.MessageTypes = messageTypes;
                message.PageResult = pageResult;
            }
            return message;
        }

        #endregion

        #region 消息中心——删除评论
        public bool DeleteVideoComment(int messageId)
        {
            CheckUserId(this.UserId);
            User user;
            CheckUserId(this.UserId, out user);
            CheckMessageId(messageId);
            SysMessage systemMessage;
            CheckVideoCommentId(messageId, user, out systemMessage);
            var messageRead = (from mr in this._messageReadRepository.GetEntityList()
                               where mr.MessageId == systemMessage.Id && mr.UserId == user.Id
                               select mr).FirstOrDefault();
            if (messageRead != null)
            {
                messageRead.State = (int)SystemMessageStateEnum.Deleted; //表示删除
                return this._messageReadRepository.UpdateEntity(messageRead);
            }
            else
            {
                var newMessageRead = new MessageRead()
                {
                    MessageId = systemMessage.Id,
                    UserId = user.Id,
                    State = (int)SystemMessageStateEnum.Deleted//表示删除
                };
                return this._messageReadRepository.CreateEntity(newMessageRead);
            }
        }

        #endregion

        #endregion

        #region 空间留言

        #region 评论分页
        private PageResult GetPageUserMessages(IQueryable<SysMessage> spaceComments,User loginUser, int parentSize, int childSize)
        {
            var pageResult = new PageResult()
            {
                TotalCount = 0,
                Data = new List<UserMessageView>(),
                PageIndex = this.PageIndex,
                PageSize = this.PageSize,
                TotalIndex = 0
            };
            if (loginUser != null)
            {
                #region 留言列表
                var spaceCommentViews = (from sm in spaceComments
                                         join c in this._commentsRepository.GetEntityList() on sm.EntityId equals c.Id
                                         join f in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals f.Id
                                         join t in this._userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals t.Id
                                         join u in this._userRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals u.Id
                                         where  c.State >= (int)CommentStateEnum.Waiting
                                               && c.EntityType == (int)CommentEnum.User
                                         orderby c.CreateTime descending
                                         select new SpaceCommentView()
                                         {
                                             Id = sm.Id,
                                             EntityId = sm.EntityId,
                                             Content = c.Content,
                                             FromUser = new UserSimpleView()
                                             {
                                                 Id = f.Id,
                                                 NickName = f.NickName,
                                                 Picture = f.Picture
                                             },
                                             ToUser = new UserSimpleView()
                                             {
                                                 Id = t.Id,
                                                 NickName = t.NickName,
                                                 Picture = t.Picture
                                             },
                                             User = new UserSimpleView()
                                             {
                                                 Id = u.Id,
                                                 NickName = u.NickName,
                                                 Picture = u.Picture
                                             },
                                             CreateTime = sm.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                             IsRead = IsRead(this._messageReadRepository.GetEntityList(), loginUser.Id, sm.Id),
                                             Position = CommentPosition(c, parentSize, childSize)
                                         });
                #endregion

                #region 分页
                int pageSize = this.PageSize <= 0 ? 8 : this.PageSize;
                int totalCount = spaceCommentViews.Count();
                int totalIndex = totalCount % pageSize == 0 ? (totalCount / pageSize) : (totalCount / pageSize + 1);
                int pageIndex = this.PageIndex <= 0 ? 1 : (this.PageIndex >= totalIndex ? totalIndex : this.PageIndex);
                pageResult.PageIndex = pageIndex;
                pageResult.PageSize = pageSize;
                pageResult.PageIndex = pageIndex;
                pageResult.TotalCount = totalCount;
                var data = spaceCommentViews.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pageResult.Data = data;
                #endregion

                #region 添加已读记录
                var commentState = (from c in data
                                    where c.IsRead == false
                                    select new MessageRead()
                                    {
                                        MessageId = c.Id,
                                        UserId = loginUser.Id
                                    }).AsQueryable();
                if (commentState.Any())
                {
                    this._messageReadRepository.CreateEntitys(commentState.ToList());
                }
                #endregion
            }


            return pageResult;
        }
        public PageResult GetPageUserMessages(int parentSize, int childSize)
        {
            User user;
            IsLogin(this._userRepository.GetEntityList(), UserId, out user);
            IQueryable<SysMessage> sysMessages = Sysmessages(user);
            IQueryable<SysMessage> uncutSysmessage = UncutSysmessage(sysMessages, user);
            IQueryable<SysMessage> spaceComments = (from sm in uncutSysmessage
                                                    where sm.EntityType == (int)SysMessageEnum.SpaceComment
                                                    select sm);
            return GetPageUserMessages(spaceComments,user, parentSize, childSize);
        }

        #endregion

        #region 消息中心——切换留言

        public MessageView GetUserMessages(int parentSize, int childSize)
        {
            var message = new MessageView()
            {
                MessageType = new MessageType(),
                MessageTypes = new List<MessageType>(),
                PageResult = new PageResult()
            };
            User user;
            if (IsLogin(this._userRepository.GetEntityList(), UserId, out user))
            {
                IQueryable<SysMessage> sysMessages = Sysmessages(user);
                IQueryable<SysMessage> uncutSysmessage = UncutSysmessage(sysMessages, user);
                IQueryable<SysMessage> spaceComments = (from sm in uncutSysmessage
                                                        where sm.EntityType == (int)SysMessageEnum.SpaceComment
                                                        select sm);
                var pageResult = GetPageUserMessages(spaceComments,user, parentSize, childSize);
                //未读
                IQueryable<SysMessage> unreadSysmessage = UnreadSysmessage(sysMessages, user);
                var messageTypes = GetMessages(unreadSysmessage, user);
                var messageType = GetHeaderMessage(unreadSysmessage, user);
                message.MessageType = messageType;
                message.MessageTypes = messageTypes;
                message.PageResult = pageResult;
            }
            return message;
        }

        #endregion

        #region 消息中心——删除留言
        public bool DeleteSpaceComment(int messageId)
        {
            CheckUserId(this.UserId);
            User user;
            CheckUserId(this.UserId, out user);
            CheckMessageId(messageId);
            SysMessage systemMessage;
            CheckSpaceCommentId(messageId, user, out systemMessage);
            var messageRead = (from mr in this._messageReadRepository.GetEntityList()
                               where mr.MessageId == systemMessage.Id && mr.UserId == user.Id
                               select mr).FirstOrDefault();
            if (messageRead != null)
            {
                messageRead.State = (int)SystemMessageStateEnum.Deleted; //表示删除
                return this._messageReadRepository.UpdateEntity(messageRead);
            }
            else
            {
                var newMessageRead = new MessageRead()
                {
                    MessageId = systemMessage.Id,
                    UserId = user.Id,
                    State = (int)SystemMessageStateEnum.Deleted//表示删除
                };
                return this._messageReadRepository.CreateEntity(newMessageRead);
            }
        }

        #endregion

        #endregion

        #region 传入参数检测
        /// <summary>
        /// 检测用户编号不能小于0
        /// </summary>
        /// <param name="userId"></param>
        private void CheckUserId(int userId)
        {
            AssertUtil.AreBigger(userId, 0, LanguageUtil.Translate("api_Business_UserCenter_Message_CheckUserId"));
        }
        /// <summary>
        /// 检测用户是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        private void CheckUserId(int userId, out User user)
        {
            IList<Condtion> condtions = new List<Condtion>()
            {
                CondtionEqualState(),
                ConditionEqualId(userId)
            };
            user = this._userRepository.GetEntity(condtions);
            AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Business_UserCenter_Message_CheckUserId_user"));
        }
        /// <summary>
        /// 检测消息编号不能小于0
        /// </summary>
        /// <param name="messageId"></param>
        private void CheckMessageId(int messageId)
        {
            AssertUtil.AreBigger(messageId, 0, LanguageUtil.Translate("api_Business_UserCenter_Message_CheckMessageId"));
        }

        /// <summary>
        /// 检测消息是否存在
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="user"></param>
        /// <param name="systemMessage"></param>
        private void CheckMessageId(int messageId, User user, out SysMessage systemMessage)
        {
            systemMessage = (from sm in this._sysMessageRepository.GetEntityList()
                             where sm.State == false
                                   && sm.Id == messageId
                                   && sm.EntityType == (int)SysMessageEnum.SystemMessage
                                   && sm.CreateTime.Ticks >= user.CreateTime.Ticks //注册之后的系统消息
                                   && ((SplitUserBy(sm.ToUserIds).Contains(user.Id) && sm.SendType == (int)SystemMessageEnum.ByUser)//指定用户
                                   || (string.IsNullOrEmpty(sm.ToUserIds) && sm.SendType == (int)SystemMessageEnum.AllUser)) //全部用户
                             orderby sm.CreateTime descending
                             select sm).FirstOrDefault();
            AssertUtil.IsNotNull(systemMessage, LanguageUtil.Translate("api_Business_UserCenter_Message_CheckMessageId_systemMessage"));
        }

        private void CheckVideoCommentId(int messageId, User user, out SysMessage systemMessage)
        {
            systemMessage = (from sm in this._sysMessageRepository.GetEntityList()
                             where sm.State == false
                                   && sm.Id == messageId
                                   && sm.EntityType == (int)SysMessageEnum.VideoComment
                                   && SplitUserBy(sm.ToUserIds).Contains(user.Id)
                                   && sm.SendType == (int)SystemMessageEnum.ByUser//指定用户
                             orderby sm.CreateTime descending
                             select sm).FirstOrDefault();
            AssertUtil.IsNotNull(systemMessage, "消息不存在或者被禁用");
        }
        private void CheckSpaceCommentId(int messageId, User user, out SysMessage systemMessage)
        {
            systemMessage = (from sm in this._sysMessageRepository.GetEntityList()
                             where sm.State == false
                                   && sm.Id == messageId
                                   && sm.EntityType == (int)SysMessageEnum.SpaceComment
                                   && SplitUserBy(sm.ToUserIds).Contains(user.Id)
                                   && sm.SendType == (int)SystemMessageEnum.ByUser//指定用户
                             orderby sm.CreateTime descending
                             select sm).FirstOrDefault();
            AssertUtil.IsNotNull(systemMessage, "消息不存在或者被禁用");
        }
        #endregion

    }
}
