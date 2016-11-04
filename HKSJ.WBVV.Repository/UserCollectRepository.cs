using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Model.Context;
using HKSJ.WBVV.Repository.Base;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Repository
{

    public class UserCollectRepository : BaseRepository, IUserCollectRepository
    {
        public IQueryable<UserCollect> GetEntityList()
        {
            return base.GetEntityList<UserCollect>();
        }

        public IQueryable<UserCollect> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserCollect>(orderCondtion);
        }

        public IQueryable<UserCollect> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserCollect>(orderCondtions);
        }

        public IQueryable<UserCollect> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserCollect>(condtion);
        }

        public IQueryable<UserCollect> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserCollect>(condtion, orderCondtion);
        }

        public IQueryable<UserCollect> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserCollect>(condtion, orderCondtions);
        }

        public IQueryable<UserCollect> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserCollect>(condtions);
        }

        public IQueryable<UserCollect> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserCollect>(condtions, orderCondtion);
        }

        public IQueryable<UserCollect> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserCollect>(condtions, orderCondtions);
        }
        public UserCollect GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserCollect>(condtion);
        }

        public UserCollect GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserCollect>(condtions);
        }
        public bool CreateEntity(UserCollect entity)
        {
            return base.CreateEntity<UserCollect>(entity);
        }

        public void CreateEntitys(IList<UserCollect> entitys)
        {
            base.CreateEntitys<UserCollect>(entitys);
        }

        public bool UpdateEntity(UserCollect entity)
        {
            return base.UpdateEntity<UserCollect>(entity);
        }

        public void UpdateEntitys(IList<UserCollect> entitys)
        {
            base.UpdateEntitys<UserCollect>(entitys);
        }

        public bool DeleteEntity(UserCollect entity)
        {
            return base.DeleteEntity<UserCollect>(entity);
        }

        public void DeleteEntitys(IList<UserCollect> entitys)
        {
            base.DeleteEntitys<UserCollect>(entitys);
        }

        public bool UnCollectVideoTransaction(UserCollect userCollect, Video video)
        {
            var success = Execute(db =>
            {
                db.Entry(userCollect).State = EntityState.Modified;
                //TODO　刘强　修改，统一SaveChanges，保证一次性提交
                db.Entry(video).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserCollect>();
                CreateCache<Video>();
            }
            return success;
        }

        /// <summary>
        /// 收藏视频
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CollectVideo(int videoId, int userId)
        {
            var success = Execute<bool>((db) =>
            {
                AssertUtil.AreBigger(userId, 0, LanguageUtil.Translate("api_Repository_UserCollect_CollectVideo_userId"));
                AssertUtil.AreBigger(videoId, 0, LanguageUtil.Translate("api_Repository_UserCollect_CollectVideo_videoId"));
                var user = db.User.FirstOrDefault(u => u.State == false && u.Id == userId);
                AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Repository_UserCollect_CollectVideo_user"));
                var video = db.Video.FirstOrDefault(v => v.State == false && v.Id == videoId && v.VideoState == 3);
                AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Repository_UserCollect_CollectVideo_video"));
                var collect = db.UserCollect.FirstOrDefault(c => c.UserId == user.Id && c.VideoId == video.Id);
                if (collect == null)
                {
                    var newCollect = new UserCollect
                    {
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        CreateUserId = user.Id,
                        UpdateUserId = user.Id,
                        UserId = userId,
                        State = false,
                        VideoId = (int)video.Id
                    };
                    db.UserCollect.Add(newCollect);
                    video.CollectionCount = video.CollectionCount + 1;
                    db.Entry<Video>(video).State = EntityState.Modified;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    var result = db.SaveChanges() > 0;
                    if (result)
                    {
                        //喜欢人与上传用户不一致
                        if (user.Id!=video.CreateManageId)
                        {
                            //添加消息通知
                            var sysMessage = new SysMessage()
                            {
                                Content = "表示喜欢了你的内容",
                                CreateManageId = 1,
                                CreateTime = DateTime.Now,
                                EntityId = newCollect.Id,
                                EntityType = (int)SysMessageEnum.UserCollect,
                                SendType = (int)SystemMessageEnum.ByUser,
                                ToUserIds = video.CreateManageId + ""//通知上传者
                            };
                            db.SysMessage.Add(sysMessage);
                            result = db.SaveChanges() > 0;
                        }
                    }
                    return result;
                }
                else
                {
                    AssertUtil.AreNotEqual(collect.State, false, LanguageUtil.Translate("api_Repository_UserCollect_CollectVideo_collectState"));
                    collect.UpdateUserId = user.Id;
                    collect.UpdateTime = DateTime.Now;
                    collect.State = false;
                    db.Entry(collect).State = EntityState.Modified;
                    video.CollectionCount = video.CollectionCount + 1;
                    db.Entry<Video>(video).State = EntityState.Modified;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    var result = db.SaveChanges() > 0;
                    if (result)
                    {
                        //喜欢人与上传用户不一致
                        if (user.Id != video.CreateManageId)
                        {
                            //添加消息通知
                            var sysMessage = new SysMessage()
                            {
                                Content = "表示喜欢了你的内容",
                                CreateManageId = 1,
                                CreateTime = DateTime.Now,
                                EntityId = collect.Id,
                                EntityType = (int)SysMessageEnum.UserCollect,
                                SendType = (int)SystemMessageEnum.ByUser,
                                ToUserIds = video.CreateManageId + ""//通知上传者
                            };
                            db.SysMessage.Add(sysMessage);
                            result = db.SaveChanges() > 0;
                        }
                    }
                    return result;
                }
             
            });
            if (success)
            {
                CreateCache<UserCollect>();
                CreateCache<Video>();
                CreateCache<SysMessage>();
            }
            return success;
        }

        /// <summary>
        /// 取消收藏视频
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public bool UnCollectVideo(int id, int userId, int videoId)
        {
            var success = Execute<bool>((db) =>
            {
                AssertUtil.AreBigger(userId, 0, LanguageUtil.Translate("api_Repository_UserCollect_UnCollectVideo_userId"));
                AssertUtil.AreBigger(videoId, 0, LanguageUtil.Translate("api_Repository_UserCollect_UnCollectVideo_videoId"));
                AssertUtil.AreBigger(id, 0, LanguageUtil.Translate("api_Repository_UserCollect_UnCollectVideo_id"));
                var user = db.User.FirstOrDefault(u => u.State == false && u.Id == userId);
                AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Repository_UserCollect_UnCollectVideo_user"));
                var video = db.Video.FirstOrDefault(v => v.State == false && v.Id == videoId && v.VideoState == 3);
                AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Repository_UserCollect_UnCollectVideo_video"));
                var collect = db.UserCollect.FirstOrDefault(c => c.Id == id);
                AssertUtil.IsNotNull(collect, LanguageUtil.Translate("api_Repository_UserCollect_UnCollectVideo_collect"));
                AssertUtil.AreNotEqual(collect.State, true, LanguageUtil.Translate("api_Repository_UserCollect_UnCollectVideo_collectState"));
                collect.UpdateUserId = user.Id;
                collect.UpdateTime = DateTime.Now;
                collect.State = true;
                db.Entry(collect).State = EntityState.Modified;
                video.CollectionCount = video.CollectionCount - 1;
                db.Entry<Video>(video).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserCollect>();
                CreateCache<Video>();
            }
            return success;
        }

        /// <summary>
        /// 取消收藏所有视频
        /// Author : AxOne
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DelAllCollectVideo(int userId)
        {
            var success = Execute(db =>
            {
                AssertUtil.AreBigger(userId, 0, LanguageUtil.Translate("api_Repository_UserCollect_DelAllCollectVideo_userId"));
                var user = db.User.FirstOrDefault(u => u.State == false && u.Id == userId);
                AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Repository_UserCollect_DelAllCollectVideo_user"));
                var collects = db.UserCollect.Where(c => c.UserId == userId && c.State==false).ToList();
                AssertUtil.IsNotNull(collects, LanguageUtil.Translate("api_Repository_UserCollect_DelAllCollectVideo_collects"));
                foreach (var collect in collects)
                {
                    var video = db.Video.FirstOrDefault(v => v.State == false && v.Id == collect.VideoId && v.VideoState == 3);
                    AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Repository_UserCollect_DelAllCollectVideo_video"));
                    collect.UpdateUserId = user.Id;
                    collect.UpdateTime = DateTime.Now;
                    collect.State = true;
                    db.Entry(collect).State = EntityState.Modified;
                    video.CollectionCount = video.CollectionCount - 1;
                    db.Entry(video).State = EntityState.Modified;
                }
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserCollect>();
                CreateCache<Video>();
            }
            return success;
        }

        /// <summary>
        /// 取消收藏视频,无ID编号时
        /// Author : xuzhoujie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public bool UnCollectVideo(int userId, int videoId)
        {
            var success = Execute<bool>((db) =>
            {
                AssertUtil.AreBigger(userId, 0, LanguageUtil.Translate("api_Repository_UserCollect_UnCollectVideo_userId_notId"));
                AssertUtil.AreBigger(videoId, 0, LanguageUtil.Translate("api_Repository_UserCollect_UnCollectVideo_videoId_notId"));
                var user = db.User.FirstOrDefault(u => u.State == false && u.Id == userId);
                AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Repository_UserCollect_UnCollectVideo_user_notId"));
                var video = db.Video.FirstOrDefault(v => v.State == false && v.Id == videoId && v.VideoState == 3);
                AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Repository_UserCollect_UnCollectVideo_video_notId"));
                var collect = db.UserCollect.FirstOrDefault(c => c.UserId == userId && c.VideoId == videoId && c.State == false);
                AssertUtil.IsNotNull(collect, LanguageUtil.Translate("api_Repository_UserCollect_UnCollectVideo_collect_notId"));
                AssertUtil.AreNotEqual(collect.State, true, LanguageUtil.Translate("api_Repository_UserCollect_UnCollectVideo_collectState_notId"));
                collect.UpdateUserId = user.Id;
                collect.UpdateTime = DateTime.Now;
                collect.State = true;
                db.Entry(collect).State = EntityState.Modified;
                video.CollectionCount = video.CollectionCount - 1;
                db.Entry<Video>(video).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserCollect>();
                CreateCache<Video>();
            }
            return success;
        }
    }
}