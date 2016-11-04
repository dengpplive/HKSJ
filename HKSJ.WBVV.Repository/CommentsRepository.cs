


using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Base;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Repository
{

    public class CommentsRepository : BaseRepository, ICommentsRepository
    {
        public IQueryable<Comments> GetEntityList()
        {
            return base.GetEntityList<Comments>();
        }

        public IQueryable<Comments> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Comments>(orderCondtion);
        }

        public IQueryable<Comments> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Comments>(orderCondtions);
        }

        public IQueryable<Comments> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<Comments>(condtion);
        }

        public IQueryable<Comments> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Comments>(condtion, orderCondtion);
        }

        public IQueryable<Comments> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Comments>(condtion, orderCondtions);
        }

        public IQueryable<Comments> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<Comments>(condtions);
        }

        public IQueryable<Comments> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Comments>(condtions, orderCondtion);
        }

        public IQueryable<Comments> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Comments>(condtions, orderCondtions);
        }
        public Comments GetEntity(Condtion condtion)
        {
            return base.GetEntity<Comments>(condtion);
        }

        public Comments GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<Comments>(condtions);
        }
        public bool CreateEntity(Comments entity)
        {
            return base.CreateEntity<Comments>(entity);
        }

        public void CreateEntitys(IList<Comments> entitys)
        {
            base.CreateEntitys<Comments>(entitys);
        }

        public bool UpdateEntity(Comments entity)
        {
            return base.UpdateEntity<Comments>(entity);
        }

        public void UpdateEntitys(IList<Comments> entitys)
        {
            base.UpdateEntitys<Comments>(entitys);
        }

        public bool DeleteEntity(Comments entity)
        {
            return base.DeleteEntity<Comments>(entity);
        }

        public void DeleteEntitys(IList<Comments> entitys)
        {
            base.DeleteEntitys<Comments>(entitys);
        }

        #region 获取LocalPath

        /// <summary>
        /// 获取评论的LocalPath,最大长度为9位
        /// </summary>
        /// <param name="prevLocalPath">上次位置</param>
        /// <param name="commentId">当前评论编号</param>
        /// <returns></returns>
        protected string GetLocalPath(string prevLocalPath, int commentId)
        {
            var currLocalPath = "";
            if (commentId < 10)
            {
                currLocalPath = "00000000" + commentId;
            }
            else if (commentId >= 10 && commentId < 100)
            {
                currLocalPath = "0000000" + commentId;
            }
            else if (commentId >= 100 & commentId < 1000)
            {
                currLocalPath = "000000" + commentId;
            }
            else if (commentId >= 1000 & commentId < 10000)
            {
                currLocalPath = "00000" + commentId;
            }
            else if (commentId >= 10000 & commentId < 100000)
            {
                currLocalPath = "0000" + commentId;
            }
            else if (commentId >= 100000 & commentId < 1000000)
            {
                currLocalPath = "000" + commentId;
            }
            else if (commentId >= 1000000 & commentId < 10000000)
            {
                currLocalPath = "00" + commentId;
            }
            else if (commentId >= 10000000 & commentId < 100000000)
            {
                currLocalPath = "0" + commentId;
            }
            else
            {
                currLocalPath = commentId.ToString();
            }
            if (string.IsNullOrEmpty(prevLocalPath))
            {
                return currLocalPath;
            }
            else
            {
                return prevLocalPath + "," + currLocalPath;
            }
        }
        #endregion

        #region 视频
        /// <summary>
        /// 发表视频评论
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="commentContent"></param>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public int CreateVideoComment(int loginUserId, int videoId, string commentContent)
        {
            var success = false;

            #region 验证参数是否正确
            AssertUtil.AreBigger(loginUserId, 0, LanguageUtil.Translate("api_Repository_Comments_CreateVideoComment_loginUserId"));
            AssertUtil.AreBigger(videoId, 0, LanguageUtil.Translate("api_Repository_Comments_CreateVideoComment_videoId"));
            AssertUtil.NotNullOrWhiteSpace(commentContent, LanguageUtil.Translate("api_Repository_Comments_CreateVideoComment_commentContent"));
            AssertUtil.AreSmallerOrEqual(commentContent.Length, 140, LanguageUtil.Translate("api_Repository_Comments_CreateVideoComment_commentContentLength"));
            #endregion

            var commentId = Execute<int>((db) =>
            {
                #region 验证数据库是否存在
                var loginUser = db.User.FirstOrDefault(u => u.State == false && u.Id == loginUserId);
                AssertUtil.IsNotNull(loginUser, LanguageUtil.Translate("api_Repository_Comments_CreateVideoComment_loginUser"));
                var video = db.Video.FirstOrDefault(v => v.State == false && v.Id == videoId);
                AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Repository_Comments_CreateVideoComment_video"));
                #endregion

                var comment = new Comments()
                {
                    FromUserId = loginUser.Id,
                    ToUserId = 0,
                    EntityType = (int)CommentEnum.Video,
                    EntityId = (int)video.Id,
                    Content = commentContent,
                    CreateUserId = loginUser.Id,
                    CreateTime = DateTime.Now,
                    LocalPath = ""//发表视频评论的位置为最大""
                };
                db.Comments.Add(comment);
                //视频评论次数加一
                video.CommentCount = video.CommentCount + 1;
                db.Entry<Video>(video).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                success = db.SaveChanges() > 0;
                if (success)
                {
                    comment.LocalPath = GetLocalPath(comment.LocalPath, comment.Id);
                    db.Entry<Comments>(comment).State = EntityState.Modified;
                    //发表评论的人不是上传者
                    if (video.CreateManageId != loginUser.Id)
                    {
                        //添加消息通知
                        var sysMessage = new SysMessage()
                        {
                            Content = comment.Content,
                            CreateManageId = 1,
                            CreateTime = DateTime.Now,
                            EntityId = comment.Id,
                            EntityType = (int)SysMessageEnum.VideoComment,
                            SendType = (int)SystemMessageEnum.ByUser,
                            ToUserIds = video.CreateManageId + ""//通知上传者
                        };
                        db.SysMessage.Add(sysMessage);
                    }
                    db.Configuration.ValidateOnSaveEnabled = false;
                    success = db.SaveChanges() > 0;
                }
                return comment.Id;
            });
            if (success)
            {
                CreateCache<Video>();
                CreateCache<Comments>();
                CreateCache<SysMessage>();
            }
            return commentId;
        }
        /// <summary>
        /// 回复视频评论
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="commentContent"></param>
        /// <param name="videoId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public int ReplyVideoComment(int loginUserId, int videoId, int commentId, string commentContent)
        {
            var success = false;

            #region 验证参数是否正确
            AssertUtil.AreBigger(loginUserId, 0, LanguageUtil.Translate("api_Repository_Comments_ReplyVideoComment_loginUserId"));
            AssertUtil.AreBigger(videoId, 0, LanguageUtil.Translate("api_Repository_Comments_ReplyVideoComment_videoId"));
            AssertUtil.AreBigger(commentId, 0, LanguageUtil.Translate("api_Repository_Comments_ReplyVideoComment_commentId"));
            AssertUtil.NotNullOrWhiteSpace(commentContent, LanguageUtil.Translate("api_Repository_Comments_ReplyVideoComment_commentContent"));
            AssertUtil.AreSmallerOrEqual(commentContent.Length, 140, LanguageUtil.Translate("api_Repository_Comments_ReplyVideoComment_commentContentLength"));
            #endregion

            var id = Execute<int>((db) =>
            {
                #region 验证数据库是否存在
                var loginUser = db.User.FirstOrDefault(u => u.State == false && u.Id == loginUserId);
                AssertUtil.IsNotNull(loginUser, LanguageUtil.Translate("api_Repository_Comments_ReplyVideoComment_loginUser"));
                var video = db.Video.FirstOrDefault(v => v.State == false && v.Id == videoId);
                AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Repository_Comments_ReplyVideoComment_video"));
                var parentComment = db.Comments.FirstOrDefault(c => c.State >= 0
                                                             && c.Id == commentId
                                                             && c.EntityType == (int)CommentEnum.Video
                                                             && c.EntityId == video.Id);
                AssertUtil.IsNotNull(parentComment, LanguageUtil.Translate("api_Repository_Comments_ReplyVideoComment_parentComment"));
                #endregion

                AssertUtil.AreNotEqual(loginUser.Id, parentComment.FromUserId, LanguageUtil.Translate("api_Repository_Comments_ReplyVideoComment_notReplySelf"));
                var comment = new Comments()
                {
                    FromUserId = loginUser.Id,
                    ToUserId = parentComment.FromUserId,
                    EntityType = (int)CommentEnum.Video,
                    EntityId = (int)video.Id,
                    Content = commentContent,
                    CreateUserId = loginUser.Id,
                    CreateTime = DateTime.Now,
                    ParentId = parentComment.Id,
                    LocalPath = GetLocalPath(parentComment.LocalPath, parentComment.Id)
                };
                db.Comments.Add(comment);
                //回复次数加一
                parentComment.ReplyNum++;
                db.Entry<Comments>(parentComment).State = EntityState.Modified;
                video.CommentCount = video.CommentCount + 1;
                db.Entry<Video>(video).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                success = db.SaveChanges() > 0;
                if (success)
                {
                    comment.LocalPath = GetLocalPath(parentComment.LocalPath, comment.Id);
                    db.Entry<Comments>(comment).State = EntityState.Modified;
                    //添加消息通知
                    var sysMessage = new SysMessage()
                    {
                        Content = comment.Content,
                        CreateManageId = 1,
                        CreateTime = DateTime.Now,
                        EntityId = comment.Id,
                        EntityType = (int) SysMessageEnum.VideoComment,
                        SendType = (int) SystemMessageEnum.ByUser
                    };
                    //回复的人不是上传者
                    if (video.CreateManageId != loginUser.Id && parentComment.FromUserId != video.CreateManageId)
                    {
                        sysMessage.ToUserIds = video.CreateManageId + "|" + parentComment.FromUserId; //通知上传者和接受者,上传者在自己视频下回复不通知
                    }
                    else
                    {
                        sysMessage.ToUserIds = parentComment.FromUserId + ""; //通知接受者
                    }
                    db.SysMessage.Add(sysMessage);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    success = db.SaveChanges() > 0;
                }
                return comment.Id;
            });
            if (success)
            {
                CreateCache<Video>();
                CreateCache<Comments>();
                CreateCache<SysMessage>();
            }
            return id;
        }
        /// <summary>
        /// 删除视频评论
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="videoId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public bool DeleteVideoComment(int loginUserId, int videoId, int commentId)
        {

            #region 验证参数是否正确
            AssertUtil.AreBigger(loginUserId, 0, LanguageUtil.Translate("api_Repository_Comments_DeleteVideoComment_loginUserId"));
            AssertUtil.AreBigger(videoId, 0, LanguageUtil.Translate("api_Repository_Comments_DeleteVideoComment_videoId"));
            AssertUtil.AreBigger(commentId, 0, LanguageUtil.Translate("api_Repository_Comments_DeleteVideoComment_commentId"));
            #endregion

            var success = Execute<bool>((db) =>
            {
                #region 验证数据库是否存在
                var loginUser = db.User.FirstOrDefault(u => u.State == false && u.Id == loginUserId);
                AssertUtil.IsNotNull(loginUser, LanguageUtil.Translate("api_Repository_Comments_DeleteVideoComment_loginUser"));
                var video = db.Video.FirstOrDefault(v => v.State == false && v.Id == videoId);
                AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Repository_Comments_DeleteVideoComment_video"));
                var comment = db.Comments.FirstOrDefault(c => c.State >= 0
                                                             && c.Id == commentId
                                                             && c.EntityType == (int)CommentEnum.Video
                                                             && c.EntityId == video.Id);
                AssertUtil.IsNotNull(comment, LanguageUtil.Translate("api_Repository_Comments_DeleteVideoComment_comment"));
                #endregion

                AssertUtil.AreEqual(loginUser.Id, video.CreateManageId, LanguageUtil.Translate("api_Repository_Comments_DeleteVideoComment_videoUpdateDelete"));

                var childCommnets = db.Comments.Where(c => c.State >= (int)CommentStateEnum.Waiting && c.EntityType == (int)CommentEnum.Video && c.LocalPath.StartsWith(comment.LocalPath));
                foreach (var childCommnet in childCommnets)
                {
                    db.Entry<Comments>(childCommnet).State = EntityState.Deleted;
                }
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<Comments>();
            }
            return success;
        }

        public bool DeleteVideoComment(int loginUserId, int commentId)
        {
            #region 验证参数是否正确
            AssertUtil.AreBigger(loginUserId, 0, "用户未登录");
            AssertUtil.AreBigger(commentId, 0, "视频评论不存在");
            #endregion

            var success = Execute<bool>((db) =>
            {
                #region 验证数据库是否存在
                var loginUser = db.User.FirstOrDefault(u => u.State == false && u.Id == loginUserId);
                AssertUtil.IsNotNull(loginUser, "用户不存在或者被禁用");
                var comment = db.Comments.FirstOrDefault(c => c.State >= (int)CommentStateEnum.Waiting
                                                             && c.Id == commentId
                                                             && c.EntityType == (int)CommentEnum.Video);
                AssertUtil.IsNotNull(comment, "视频评论被删除或者审核不通过");
                var video = db.Video.FirstOrDefault(v => v.State == false && v.Id == comment.EntityId);
                AssertUtil.IsNotNull(video, "评论的视频不存在或者被删除");
                #endregion

                AssertUtil.AreEqual(loginUser.Id, video.CreateManageId, "只有视频上传者才能删除评论");

                var childCommnets =db.Comments.Where(c => c.State >= (int)CommentStateEnum.Waiting && c.EntityType == (int)CommentEnum.Video &&c.LocalPath.StartsWith(comment.LocalPath));
                foreach (var childCommnet in childCommnets)
                {
                    db.Entry<Comments>(childCommnet).State = EntityState.Deleted;
                }
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<Comments>();
            }
            return success;
        }

        #endregion

        #region 用户空间

        /// <summary>
        /// 用户空间留言
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="ownerUserId">留言空间用户编号</param>
        /// <param name="commentContent">留言内容</param>
        /// <returns></returns>
        public int CreateSpaceComment(int loginUserId, int ownerUserId, string commentContent)
        {
            var success = false;

            #region 验证输入
            AssertUtil.AreBigger(loginUserId, 0, LanguageUtil.Translate("api_Repository_Comments_CreateSpaceComment_loginUserId"));
            AssertUtil.AreBigger(ownerUserId, 0, LanguageUtil.Translate("api_Repository_Comments_CreateSpaceComment_ownerUserId"));
            AssertUtil.NotNullOrWhiteSpace(commentContent, LanguageUtil.Translate("api_Repository_Comments_CreateSpaceComment_commentContent"));
            AssertUtil.AreSmallerOrEqual(commentContent.Length, 140, LanguageUtil.Translate("api_Repository_Comments_CreateSpaceComment_commentContentLength"));
            #endregion

            var id = Execute<int>((db) =>
            {
                #region 验证数据库是否存在
                var loginUser = db.User.FirstOrDefault(u => u.State == false && u.Id == loginUserId);
                AssertUtil.IsNotNull(loginUser, LanguageUtil.Translate("api_Repository_Comments_CreateSpaceComment_loginUser"));
                var toUser = db.User.FirstOrDefault(u => u.State == false && u.Id == ownerUserId);
                AssertUtil.IsNotNull(toUser, LanguageUtil.Translate("api_Repository_Comments_CreateSpaceComment_toUser"));
                #endregion

                var comment = new Comments()
                {
                    FromUserId = loginUser.Id,
                    ToUserId = toUser.Id,
                    EntityType = (int)CommentEnum.User,
                    EntityId = (int)toUser.Id,
                    Content = commentContent,
                    CreateUserId = loginUser.Id,
                    CreateTime = DateTime.Now,
                    LocalPath = ""//发表视频评论的位置为最大""
                };
                db.Comments.Add(comment);
                success = db.SaveChanges() > 0;
                if (success)
                {
                    comment.LocalPath = GetLocalPath(comment.LocalPath, comment.Id);
                    db.Entry<Comments>(comment).State = EntityState.Modified;
                    //留言用户不是自己
                    if (toUser.Id != loginUser.Id)
                    {
                        //添加消息通知
                        var sysMessage = new SysMessage()
                        {
                            Content = comment.Content,
                            CreateManageId = 1,
                            CreateTime = DateTime.Now,
                            EntityId = comment.Id,
                            EntityType = (int)SysMessageEnum.SpaceComment,
                            SendType = (int)SystemMessageEnum.ByUser,
                            ToUserIds = toUser.Id + "" //通知空间所属者
                        };
                        db.SysMessage.Add(sysMessage);
                    }
                    db.Configuration.ValidateOnSaveEnabled = false;
                    success = db.SaveChanges() > 0;
                }
                return comment.Id;
            });
            if (success)
            {
                CreateCache<Comments>();
                CreateCache<SysMessage>();
            }
            return id;
        }

        /// <summary>
        /// 回复用户留言
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="commentContent">留言内容</param>
        /// <param name="ownerUserId">留言的用户空间用户编号</param>
        /// <param name="commentId">留言编号</param>
        /// <returns></returns>
        public int ReplySpaceComment(int loginUserId, int ownerUserId, int commentId, string commentContent)
        {
            var success = false;

            #region 验证输入
            AssertUtil.AreBigger(loginUserId, 0, LanguageUtil.Translate("api_Repository_Comments_ReplySpaceComment_loginUserId"));
            AssertUtil.AreBigger(ownerUserId, 0, LanguageUtil.Translate("api_Repository_Comments_ReplySpaceComment_ownerUserId"));
            AssertUtil.AreBigger(commentId, 0, LanguageUtil.Translate("api_Repository_Comments_ReplySpaceComment_commentId"));
            AssertUtil.NotNullOrWhiteSpace(commentContent, LanguageUtil.Translate("api_Repository_Comments_ReplySpaceComment_commentContent"));
            AssertUtil.AreSmallerOrEqual(commentContent.Length, 140, LanguageUtil.Translate("api_Repository_Comments_ReplySpaceComment_commentContentLength"));
            #endregion

            var id = Execute<int>((db) =>
            {
                #region 验证数据库是否存在
                var loginUser = db.User.FirstOrDefault(u => u.State == false && u.Id == loginUserId);
                AssertUtil.IsNotNull(loginUser, LanguageUtil.Translate("api_Repository_Comments_ReplySpaceComment_loginUser"));
                var toUser = db.User.FirstOrDefault(u => u.State == false && u.Id == ownerUserId);
                AssertUtil.IsNotNull(toUser, LanguageUtil.Translate("api_Repository_Comments_ReplySpaceComment_toUser"));
                var parentComment = db.Comments.FirstOrDefault(c => c.State >= 0
                                                               && c.Id == commentId
                                                               && c.EntityType == (int)CommentEnum.User
                                                               && c.EntityId == toUser.Id);
                AssertUtil.IsNotNull(parentComment, LanguageUtil.Translate("api_Repository_Comments_ReplySpaceComment_parentComment"));
                #endregion

                AssertUtil.AreNotEqual(loginUser.Id, parentComment.FromUserId, LanguageUtil.Translate("api_Repository_Comments_ReplySpaceComment_notReplySelf"));
                var comment = new Comments()
                {
                    FromUserId = loginUser.Id,
                    ToUserId = parentComment.FromUserId,
                    EntityType = (int)CommentEnum.User,
                    EntityId = toUser.Id,
                    Content = commentContent,
                    CreateUserId = loginUser.Id,
                    CreateTime = DateTime.Now,
                    ParentId = parentComment.Id,
                    LocalPath = GetLocalPath(parentComment.LocalPath, parentComment.Id)
                };
                db.Comments.Add(comment);
                parentComment.ReplyNum = parentComment.ReplyNum + 1;
                db.Entry(parentComment).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                success = db.SaveChanges() > 0;
                if (success)
                {
                    comment.LocalPath = GetLocalPath(parentComment.LocalPath, comment.Id);
                    db.Entry<Comments>(comment).State = EntityState.Modified;
                    //添加消息通知
                    var sysMessage = new SysMessage()
                    {
                        Content = comment.Content,
                        CreateManageId = 1,
                        CreateTime = DateTime.Now,
                        EntityId = comment.Id,
                        EntityType = (int)SysMessageEnum.SpaceComment,
                        SendType = (int)SystemMessageEnum.ByUser
                    };
                    //回复留言不是空间所属者
                    if (toUser.Id != loginUser.Id && parentComment.FromUserId != toUser.Id)
                    {
                        sysMessage.ToUserIds = toUser.Id + "|" + parentComment.FromUserId;//通知空间所属者和接受者
                    }
                    else
                    {
                        sysMessage.ToUserIds = parentComment.FromUserId + "";////通知接受者
                    }
                    db.SysMessage.Add(sysMessage);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    success = db.SaveChanges() > 0;
                }
                return comment.Id;
            });
            if (success)
            {
                CreateCache<Comments>();
                CreateCache<SysMessage>();
            }
            return id;
        }

        /// <summary>
        /// 删除空间留言
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="ownerUserId">留言空间用户编号</param>
        /// <param name="commentId">留言内容</param>
        /// <returns></returns>
        public bool DeleteSpaceComment(int loginUserId, int ownerUserId, int commentId)
        {

            #region 验证参数是否正确
            AssertUtil.AreBigger(loginUserId, 0, LanguageUtil.Translate("api_Repository_Comments_DeleteSpaceComment_loginUserId"));
            AssertUtil.AreBigger(ownerUserId, 0, LanguageUtil.Translate("api_Repository_Comments_DeleteSpaceComment_ownerUserId"));
            AssertUtil.AreBigger(commentId, 0, LanguageUtil.Translate("api_Repository_Comments_DeleteSpaceComment_commentId"));
            #endregion

            var success = Execute<bool>((db) =>
            {
                #region 验证数据库是否存在
                var loginUser = db.User.FirstOrDefault(u => u.State == false && u.Id == loginUserId);
                AssertUtil.IsNotNull(loginUser, LanguageUtil.Translate("api_Repository_Comments_DeleteSpaceComment_loginUser"));
                var toUser = db.User.FirstOrDefault(u => u.State == false && u.Id == ownerUserId);
                AssertUtil.IsNotNull(toUser, LanguageUtil.Translate("api_Repository_Comments_DeleteSpaceComment_toUser"));
                var comment = db.Comments.FirstOrDefault(c => c.State >= 0
                                                               && c.Id == commentId
                                                               && c.EntityType == (int)CommentEnum.User
                                                               && c.EntityId == toUser.Id);
                AssertUtil.IsNotNull(comment, LanguageUtil.Translate("api_Repository_Comments_DeleteSpaceComment_comment"));
                #endregion

                AssertUtil.AreEqual(loginUser.Id, comment.EntityId, LanguageUtil.Translate("api_Repository_Comments_DeleteSpaceComment_spaceUserDelete"));

                var childCommnets = db.Comments.Where(c => c.State >= (int)CommentStateEnum.Waiting && c.EntityType == (int)CommentEnum.User && c.LocalPath.StartsWith(comment.LocalPath));
                foreach (var childCommnet in childCommnets)
                {
                    db.Entry<Comments>(childCommnet).State = EntityState.Deleted;
                }
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<Comments>();
            }
            return success;
        }

        /// <summary>
        /// 删除空间留言
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="commentId">留言内容</param>
        /// <returns></returns>
        public bool DeleteSpaceComment(int loginUserId, int commentId)
        {
            #region 验证参数是否正确
            AssertUtil.AreBigger(loginUserId, 0, "用户未登录");
            AssertUtil.AreBigger(commentId, 0, "回复留言不存在");
            #endregion

            var success = Execute<bool>((db) =>
            {
                #region 验证数据库是否存在
                var loginUser = db.User.FirstOrDefault(u => u.State == false && u.Id == loginUserId);
                AssertUtil.IsNotNull(loginUser, "用户不存在或者被禁用");
                var comment = db.Comments.FirstOrDefault(c => c.State >= 0
                                                               && c.Id == commentId
                                                               && c.EntityType == (int)CommentEnum.User);
                AssertUtil.IsNotNull(comment, "回复的留言可能已经被删除或者审核不通过");
                #endregion

                AssertUtil.AreEqual(loginUser.Id, comment.EntityId, "只有空间用户才能删除留言");

                var childCommnets = db.Comments.Where(c => c.State >= (int)CommentStateEnum.Waiting && c.EntityType == (int)CommentEnum.User && c.LocalPath.StartsWith(comment.LocalPath));
                foreach (var childCommnet in childCommnets)
                {
                    db.Entry<Comments>(childCommnet).State = EntityState.Deleted;
                }
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<Comments>();
            }
            return success;
        }
        #endregion
    }
}