using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Entity.Response.App;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{
    public class CommentBusiness : BaseBusiness, ICommentBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IPraisesRepository _praisesRepository;
        private readonly ICommentsRepository _commentsRepository;
        public CommentBusiness( IUserRepository userRepository, IVideoRepository videoRepository, IPraisesRepository praisesRepository, ICommentsRepository commentsRepository)
        {
            this._userRepository = userRepository;
            this._videoRepository = videoRepository;
            this._praisesRepository = praisesRepository;
            _commentsRepository = commentsRepository;
        }


        #region 视频

        #region 视频评论列表
        /// <summary>
        /// 视频下的根级评论列表
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <returns></returns>
        public IQueryable<CommentView> GetParentCommentVideos(int videoId)
        {
            if (videoId <= 0)
            {
                return null;
            }
            IQueryable<CommentView> parentComments = (from c in this._commentsRepository.GetEntityList()
                                                      join f in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals f.Id
                                                      join v in this._videoRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals (int)v.Id
                                                      where c.EntityType == (int)CommentEnum.Video
                                                            && c.EntityId == videoId
                                                            && c.State >= (int)CommentStateEnum.Waiting
                                                            && c.ParentId == 0
                                                            && v.VideoState == 3
                                                      orderby c.LocalPath + c.CreateTime descending
                                                      select new CommentView()
                                                      {
                                                          Id = c.Id,
                                                          Content = c.Content,
                                                          FromUser = new UserSimpleView()
                                                          {
                                                              Id = f.Id,
                                                              NickName = f.NickName,
                                                              Picture = f.Picture
                                                          },
                                                          CreateTime = TimeSpan(c.CreateTime),
                                                          IsPraised = IsPraises(UserId, c, this._praisesRepository.GetEntityList()),
                                                          State = c.State,
                                                          ReplyNum = c.ReplyNum,
                                                          PraisesNum = c.PraisesNum
                                                      });

            return parentComments;
        }
        /// <summary>
        /// 视频下的根级评论的子列表
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <param name="parentComment">父级评论编号</param>
        /// <returns></returns>
        public IQueryable<CommentView> GetChildCommentVideos(int videoId, Comments parentComment)
        {
            if (parentComment == null || videoId <= 0)
            {
                return null;
            }
            IQueryable<CommentView> childComments = (from c in this._commentsRepository.GetEntityList()
                                                     join f in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals f.Id
                                                     join t in this._userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals t.Id
                                                     join v in this._videoRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals (int)v.Id
                                                     where c.EntityType == (int)CommentEnum.Video
                                                          && c.State >= (int)CommentStateEnum.Waiting
                                                          && c.EntityId == videoId
                                                          && c.ParentId > 0
                                                          && c.LocalPath.StartsWith(parentComment.LocalPath)
                                                          && v.VideoState == 3
                                                     orderby c.LocalPath + c.CreateTime descending
                                                     select new CommentView()
                                                     {
                                                         Id = c.Id,
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
                                                         CreateTime = TimeSpan(c.CreateTime),
                                                         IsPraised = IsPraises(UserId,c,this._praisesRepository.GetEntityList()),
                                                         State = c.State,
                                                         ReplyNum = c.ReplyNum,
                                                         PraisesNum = c.PraisesNum
                                                     });
            return childComments;
        }

        /// <summary>
        /// 视频下的根级评论列表
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <param name="pId">父级评论编号</param>
        /// <returns></returns>
        public IQueryable<CommentView> GetChildCommentVideos(int videoId, int pId)
        {
            var parentComment = (from c in this._commentsRepository.GetEntityList()
                                 where c.ParentId == 0
                                 && c.Id == pId
                                 && c.EntityId == videoId
                                 && c.EntityType == (int)CommentEnum.Video
                                 && c.State >= (int)CommentStateEnum.Waiting
                                 select c).FirstOrDefault();
            return GetChildCommentVideos(videoId, parentComment);
        }
        /// <summary>
        /// 视频下的根级评论的子列表
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public IList<CommentView> GetVideoComments(int videoId, int pId)
        {
            var childComments = GetChildCommentVideos(videoId, pId);
            return childComments != null && childComments.Any() ? childComments.ToList() : new List<CommentView>();
        }

        /// <summary>
        /// 视频下的根级评论的子列表分页
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="pId"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public PageResult GetVideoComments(int videoId, int pId, int size, int index)
        {
            var childComments = GetChildCommentVideos(videoId, pId);

            #region 子评论分页
            int pageSize = size <= 0 ? 8 : size;
            int totalCount = (childComments != null && childComments.Any()) ? childComments.Count() : 0;
            int totalIndex = totalCount % pageSize == 0 ? (totalCount / pageSize) : (totalCount / pageSize + 1);
            int pageIndex = index <= 0 ? 1 : (index >= totalIndex ? totalIndex : index);
            var pageParentComments = (childComments != null && childComments.Any()) ? (childComments.Skip((pageIndex - 1) * pageSize).Take(pageSize)).ToList() : new List<CommentView>();
            #endregion

            var pageResult = new PageResult()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalIndex = totalIndex,
                Data = pageParentComments
            };
            return pageResult;
        }
        /// <summary>
        /// 视频分页评论列表
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <param name="size">子评论显示条数</param>
        /// <param name="index">子评论显示页数</param>
        /// <returns></returns>
        public PageResult GetVideoComments(int videoId, int size, int index)
        {
            var pageResult = new PageResult();
            IList<CommentsView> commentvideos = new List<CommentsView>();
            var parentComments = GetParentCommentVideos(videoId);
            if (parentComments != null && parentComments.Any())
            {
                #region 根级评论分页
                int pageSize = this.PageSize <= 0 ? 8 : this.PageSize;
                int totalCount = parentComments.Any() ? parentComments.Count() : 0;
                int totalIndex = totalCount % pageSize == 0 ? (totalCount / pageSize) : (totalCount / pageSize + 1);
                int pageIndex = this.PageIndex <= 0 ? 1 : (this.PageIndex >= totalIndex ? totalIndex : this.PageIndex);
                pageResult.PageIndex = pageIndex;
                pageResult.PageSize = pageSize;
                pageResult.PageIndex = pageIndex;
                pageResult.TotalCount = totalCount;
                //指定视频最上级的评论分页视图
                var pageParentComments = parentComments.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                #endregion

                foreach (var parentComment in pageParentComments)
                {
                    var childCommentVideos = GetVideoComments(videoId, parentComment.Id, size, index);
                    var commentvideo = new CommentsView()
                    {
                        ParentComment = parentComment,
                        ChildComments = childCommentVideos
                    };
                    commentvideos.Add(commentvideo);
                }
            }
            pageResult.Data = commentvideos;
            return pageResult;
        }
        #endregion

        #region 发表视频评论
        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="commentContent">评论内容</param>
        /// <param name="videoId">评论视频编号</param>
        /// <returns></returns>
        public int CreateVideoComment(int videoId, string commentContent)
        {
            return this._commentsRepository.CreateVideoComment(this.UserId, videoId, commentContent);
        }
        #endregion

        #region 回复视频评论
        /// <summary>
        /// 回复视频评论
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="commentId"></param>
        /// <param name="commentContent"></param>
        /// <returns></returns>
        public int ReplyVideoComment(int videoId, int commentId, string commentContent)
        {
            return this._commentsRepository.ReplyVideoComment(this.UserId, videoId, commentId, commentContent);
        }
        #endregion

        #region 删除视频评论
        /// <summary>
        /// 删除视频评论
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public bool DeleteVideoComment(int videoId, int commentId)
        {
            return this._commentsRepository.DeleteVideoComment(this.UserId, videoId, commentId);
        }
        /// <summary>
        /// 删除视频评论
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public bool DeleteVideoComment(int commentId)
        {
            return this._commentsRepository.DeleteVideoComment(this.UserId, commentId);
        }
        #endregion

        #endregion

        #region 空间留言

        #region 用户评论列表
        /// <summary>
        /// 用户空间下的根级留言列表
        /// </summary>
        /// <param name="ownerUserId">用户空间编号</param>
        /// <returns></returns>
        public IQueryable<CommentView> GetParentCommentUsers(int ownerUserId)
        {
            if (ownerUserId <= 0)
            {
                return null;
            }
            IQueryable<CommentView> parentComments = (from c in this._commentsRepository.GetEntityList()
                                                      join f in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals f.Id
                                                      join u in this._userRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals u.Id
                                                      where c.EntityType == (int)CommentEnum.User
                                                                    && c.EntityId == ownerUserId
                                                                    && c.ParentId == 0
                                                                    && c.State >= (int)CommentStateEnum.Waiting
                                                                    && u.Id == ownerUserId
                                                      orderby c.LocalPath + c.CreateTime descending
                                                      select new CommentView()
                                                      {
                                                          Id = c.Id,
                                                          Content = c.Content,
                                                          FromUser = new UserSimpleView()
                                                          {
                                                              Id = f.Id,
                                                              NickName = f.NickName,
                                                              Picture = f.Picture
                                                          },
                                                          ToUser = new UserSimpleView()
                                                          {
                                                              Id = u.Id,
                                                              NickName = u.NickName,
                                                              Picture = u.Picture
                                                          },
                                                          CreateTime = TimeSpan(c.CreateTime),
                                                          IsPraised = IsPraises(UserId, c, this._praisesRepository.GetEntityList()),
                                                          State = c.State,
                                                          ReplyNum = c.ReplyNum,
                                                          PraisesNum = c.PraisesNum
                                                      });

            return parentComments;
        }

        /// <summary>
        /// 用户空间下的根级留言的子列表
        /// </summary>
        /// <param name="ownerUserId">用户空间编号</param>
        /// <param name="parentComment"></param>
        /// <returns></returns>
        public IQueryable<CommentView> GetChildCommentUsers(int ownerUserId, Comments parentComment)
        {
            if (parentComment == null || ownerUserId<=0)
            {
                return null;
            }
            IQueryable<CommentView> childComments = (from c in this._commentsRepository.GetEntityList()
                                                     join f in this._userRepository.GetEntityList(CondtionEqualState()) on c.FromUserId equals f.Id
                                                     join t in this._userRepository.GetEntityList(CondtionEqualState()) on c.ToUserId equals t.Id
                                                     join u in this._userRepository.GetEntityList(CondtionEqualState()) on c.EntityId equals u.Id
                                                     where c.EntityType == (int)CommentEnum.User
                                                          && c.EntityId == ownerUserId
                                                          && c.ParentId > 0
                                                          && c.State >= (int)CommentStateEnum.Waiting
                                                          && c.LocalPath.StartsWith(parentComment.LocalPath)
                                                          && u.Id == ownerUserId
                                                     orderby c.LocalPath + c.CreateTime descending
                                                     select new CommentView()
                                                     {
                                                         Id = c.Id,
                                                         Content = c.Content,
                                                         FromUser = new UserSimpleView()
                                                         {
                                                             Id = f.Id,
                                                             NickName = f.NickName,
                                                             Picture = f.Picture
                                                         },
                                                         ToUser = new UserSimpleView()
                                                         {
                                                             Id = u.Id,
                                                             NickName = u.NickName,
                                                             Picture = u.Picture
                                                         },
                                                         CreateTime = TimeSpan(c.CreateTime),
                                                         IsPraised = IsPraises(UserId, c, this._praisesRepository.GetEntityList()),
                                                         State = c.State,
                                                         ReplyNum = c.ReplyNum,
                                                         PraisesNum = c.PraisesNum
                                                     });
            return childComments;
        }
        /// <summary>
        /// 用户空间下的根级留言的子列表
        /// </summary>
        /// <param name="ownerUserId">用户空间编号</param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public IQueryable<CommentView> GetChildCommentUsers(int ownerUserId, int pId)
        {
            var parentComment = (from c in this._commentsRepository.GetEntityList()
                                 where c.ParentId == 0
                                 && c.Id == pId
                                 && c.State >= (int)CommentStateEnum.Waiting
                                 && c.EntityId == ownerUserId
                                 && c.EntityType == (int)CommentEnum.User
                                 select c).FirstOrDefault();
            return GetChildCommentUsers(ownerUserId, parentComment);
        }
        /// <summary>
        /// 用户空间下的根级留言的子列表
        /// </summary>
        /// <param name="ownerUserId">用户空间编号</param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public IList<CommentView> GetSpaceComments(int ownerUserId, int pId)
        {
            var childComments = GetChildCommentUsers(ownerUserId, pId);
            return (childComments != null && childComments.Any()) ? childComments.ToList() : new List<CommentView>();
        }
        /// <summary>
        /// 用户空间下的根级留言的子列表分页
        /// </summary>
        /// <param name="ownerUserId">用户空间编号</param>
        /// <param name="pId"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public PageResult GetSpaceComments(int ownerUserId, int pId, int size, int index)
        {
            var childComments = GetChildCommentUsers(ownerUserId, pId);

            #region 子评论分页
            int pageSize = size <= 0 ? 8 : size;
            int totalCount = (childComments != null && childComments.Any()) ? childComments.Count() : 0;
            int totalIndex = totalCount % pageSize == 0 ? (totalCount / pageSize) : (totalCount / pageSize + 1);
            int pageIndex = index <= 0 ? 1 : (index >= totalIndex ? totalIndex : index);
            var pageParentComments = (childComments != null && childComments.Any()) ? (childComments.Skip((pageIndex - 1) * pageSize).Take(pageSize)).ToList() : new List<CommentView>();
            #endregion

            var pageResult = new PageResult()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalIndex = totalIndex,
                Data = pageParentComments
            };
            return pageResult;
        }

        /// <summary>
        /// 用户空间下分页留言列表
        /// </summary>
        /// <param name="ownerUserId">用户空间编号</param>
        /// <param name="size">子评论显示条数</param>
        /// <param name="index">子评论显示页数</param>
        /// <returns></returns>
        public PageResult GetSpaceComments(int ownerUserId, int size, int index)
        {
            var pageResult = new PageResult();
            IList<CommentsView> commentvideos = new List<CommentsView>();
            var parentComments = GetParentCommentUsers(ownerUserId);
            if (parentComments != null && parentComments.Any())
            {
                #region 根级评论分页
                int pageSize = this.PageSize <= 0 ? 8 : this.PageSize;
                int totalCount = parentComments.Any() ? parentComments.Count() : 0;
                int totalIndex = totalCount % pageSize == 0 ? (totalCount / pageSize) : (totalCount / pageSize + 1);
                int pageIndex = this.PageIndex <= 0 ? 1 : (this.PageIndex >= totalIndex ? totalIndex : this.PageIndex);
                pageResult.PageIndex = pageIndex;
                pageResult.PageSize = pageSize;
                pageResult.PageIndex = pageIndex;
                pageResult.TotalCount = totalCount;
                //指定视频最上级的评论分页视图
                var pageParentComments = parentComments.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                #endregion

                foreach (var parentComment in pageParentComments)
                {
                    var childCommentVideos = GetSpaceComments(ownerUserId, parentComment.Id, size, index);
                    var commentvideo = new CommentsView()
                    {
                        ParentComment = parentComment,
                        ChildComments = childCommentVideos
                    };
                    commentvideos.Add(commentvideo);
                }
            }
            pageResult.Data = commentvideos;
            return pageResult;
        }

        #endregion

        #region 用户发表留言

        /// <summary>
        /// 用户发表评论
        /// </summary>
        /// <param name="ownerUserId">评论的空间</param>
        /// <param name="commentContent">评论内容</param>
        /// <returns></returns>
        public int CreateSpaceComment(int ownerUserId, string commentContent)
        {
            return this._commentsRepository.CreateSpaceComment(this.UserId, ownerUserId, commentContent);
        }
        #endregion

        #region 用户回复留言

        /// <summary>
        /// 用户回复评论
        /// </summary>
        /// <param name="commentContent">评论内容</param>
        /// <param name="ownerUserId">评论的空间</param>
        /// <param name="commentId">评论编号</param>
        /// <returns></returns>
        public int ReplySpaceComment(int ownerUserId, int commentId, string commentContent)
        {
            return this._commentsRepository.ReplySpaceComment(this.UserId, ownerUserId, commentId, commentContent);
        }
        #endregion

        #region 空间用户删除留言

        /// <summary>
        /// 空间用户删除留言
        /// </summary>
        /// <param name="ownerUserId">评论的空间</param>
        /// <param name="commentId">评论编号</param>
        /// <returns></returns>
        public bool DeleteSpaceComment(int ownerUserId, int commentId)
        {
            return this._commentsRepository.DeleteSpaceComment(this.UserId, ownerUserId, commentId);
        }

        /// <summary>
        /// 空间用户删除留言
        /// </summary>
        /// <param name="commentId">评论编号</param>
        /// <returns></returns>
        public bool DeleteSpaceComment(int commentId)
        {
            return this._commentsRepository.DeleteSpaceComment(this.UserId,commentId);
        }
        #endregion

        #endregion

        #region 传入参数检测
        /// <summary>
        /// 检测评论内容不能为空
        /// </summary>
        /// <param name="commentContent"></param>
        private void CheckCommentContent(string commentContent)
        {
            AssertUtil.NotNullOrWhiteSpace(commentContent, LanguageUtil.Translate("api_Business_Comment_CheckCommentContent"));
        }

        /// <summary>
        /// 检测UserId不能小于0
        /// </summary>
        /// <param name="userId"></param>
        private void CheckUserId(int userId)
        {
            AssertUtil.AreBigger(userId, 0, LanguageUtil.Translate("api_Business_Comment_CheckUserId"));
        }
        /// <summary>
        /// 检测UserId是否存在
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
            AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Business_Comment_CheckUserId_user"));
        }

        private void CheckParentUserId(int parentUserId, out User user)
        {
            IList<Condtion> condtions = new List<Condtion>()
            {
                CondtionEqualState(),
                CondtionEqualParentId(parentUserId)
            };
            user = this._userRepository.GetEntity(condtions);
            AssertUtil.IsNull(user, LanguageUtil.Translate("api_Business_Comment_CheckParentUserId"));
        }

        /// <summary>
        /// 检测videoId不能小于0
        /// </summary>
        /// <param name="videoId"></param>
        private void CheckVideoId(int videoId)
        {
            AssertUtil.AreBigger(videoId, 0, LanguageUtil.Translate("api_Business_Comment_CheckVideoId"));
        }
        /// <summary>
        /// 检测UserId是否存在
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="video"></param>
        private void CheckVideoId(int videoId, out Video video)
        {
            IList<Condtion> condtions = new List<Condtion>()
            {
                CondtionEqualState(),
                ConditionEqualId(videoId)
            };
            video = this._videoRepository.GetEntity(condtions);
            AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Business_Comment_CheckVideoId_video"));
        }
        /// <summary>
        /// 检测CommentId不能小于0
        /// </summary>
        /// <param name="commentId"></param>
        private void CheckCommentId(int commentId)
        {
            AssertUtil.AreBigger(commentId, 0, LanguageUtil.Translate("api_Business_Comment_CheckVideoId_CheckCommentId"));
        }
        #endregion

        #region 传入参数
        /// <summary>
        /// 比较上级编号相等
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualParentId(int parentId)
        {
            var condtion = new Condtion()
            {
                FiledName = "ParentId",
                FiledValue = parentId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        #endregion

        #region 排序参数


        #endregion
    }
}
