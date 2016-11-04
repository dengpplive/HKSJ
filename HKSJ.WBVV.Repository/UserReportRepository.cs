



using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Repository.Base;
using HKSJ.WBVV.Repository.Interface;
namespace HKSJ.WBVV.Repository
{

    public class UserReportRepository : BaseRepository, IUserReportRepository
    {
        public IQueryable<UserReport> GetEntityList()
        {
            return base.GetEntityList<UserReport>();
        }

        public IQueryable<UserReport> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserReport>(orderCondtion);
        }

        public IQueryable<UserReport> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserReport>(orderCondtions);
        }

        public IQueryable<UserReport> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserReport>(condtion);
        }

        public IQueryable<UserReport> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserReport>(condtion, orderCondtion);
        }

        public IQueryable<UserReport> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserReport>(condtion, orderCondtions);
        }

        public IQueryable<UserReport> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserReport>(condtions);
        }

        public IQueryable<UserReport> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserReport>(condtions, orderCondtion);
        }

        public IQueryable<UserReport> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserReport>(condtions, orderCondtions);
        }
        public UserReport GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserReport>(condtion);
        }

        public UserReport GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserReport>(condtions);
        }
        public bool CreateEntity(UserReport entity)
        {
            return base.CreateEntity<UserReport>(entity);
        }

        public void CreateEntitys(IList<UserReport> entitys)
        {
            base.CreateEntitys(entitys);
        }

        public bool UpdateEntity(UserReport entity)
        {
            return base.UpdateEntity(entity);
        }

        public void UpdateEntitys(IList<UserReport> entitys)
        {
            base.UpdateEntitys<UserReport>(entitys);
        }

        public bool DeleteEntity(UserReport entity)
        {
            return base.DeleteEntity<UserReport>(entity);
        }

        public void DeleteEntitys(IList<UserReport> entitys)
        {
            base.DeleteEntitys<UserReport>(entitys);
        }

        #region 举报视频
        /// <summary>
        /// 举报视频
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="videoId">视频编号</param>
        /// <returns></returns>
        public bool ReportVideo(int loginUserId, int videoId)
        {
            var success = Execute<bool>((db) =>
            {
                var user = db.User.FirstOrDefault(u => u.State == false && u.Id == loginUserId);
                AssertUtil.IsNotNull(user, "用户不存在或者被禁用");
                var video = db.Video.FirstOrDefault(v => v.VideoSource & (v.VideoState == 3 || v.VideoState==2) && v.Id == videoId);
                AssertUtil.IsNotNull(video, "视频不存在或者审核不通过");
                var userReport = db.UserReport.FirstOrDefault(v => v.State == false && v.UserId == user.Id && v.EntityId == video.Id&& v.EntityType == (int)UserReportEnum.Video);
                AssertUtil.IsNull(userReport, "您已经举报过该视频");
                var newUserReport = new UserReport()
                {
                    CreateTime = DateTime.Now,
                    CreateUserId = user.Id,
                    EntityType = (int)UserReportEnum.Video,
                    UserId = user.Id,
                    EntityId = (int)video.Id
                };
                db.UserReport.Add(newUserReport);
                video.ReportCount++;
                db.Entry(video).State= EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserReport>();
                CreateCache<Video>();
            }
            return success;
        }
        #endregion

        #region 举报评论
        /// <summary>
        /// 举报评论
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="commentId">评论编号</param>
        /// <returns></returns>
        public bool ReportComment(int loginUserId, int commentId)
        {
            var success = Execute<bool>((db) =>
            {
                var user = db.User.FirstOrDefault(u => u.State == false && u.Id == loginUserId);
                AssertUtil.IsNotNull(user, "用户不存在或者被禁用");
                var comment = db.Comments.FirstOrDefault(c => c.State >= 0 & c.Id == commentId);
                AssertUtil.IsNotNull(comment, "评论不存在或者审核不通过");
                var userReport = db.UserReport.FirstOrDefault(v => v.State == false && v.UserId == user.Id && v.EntityId == comment.Id && v.EntityType == (int)UserReportEnum.Comment);
                AssertUtil.IsNull(userReport, "您已经举报过该评论");
                var newUserReport = new UserReport()
                {
                    CreateTime = DateTime.Now,
                    CreateUserId = user.Id,
                    EntityType = (int)UserReportEnum.Comment,
                    UserId = user.Id,
                    EntityId = comment.Id
                };
                db.UserReport.Add(newUserReport);
                comment.ReportCount++;
                db.Entry(comment).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<UserReport>();
                CreateCache<Comments>();
            }
            return success;
        }
        #endregion
    }
}