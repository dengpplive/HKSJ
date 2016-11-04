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
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Repository
{

    public class PraisesRepository : BaseRepository, IPraisesRepository
    {
        public IQueryable<Praises> GetEntityList()
        {
            return base.GetEntityList<Praises>();
        }

        public IQueryable<Praises> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Praises>(orderCondtion);
        }

        public IQueryable<Praises> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Praises>(orderCondtions);
        }

        public IQueryable<Praises> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<Praises>(condtion);
        }

        public IQueryable<Praises> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Praises>(condtion, orderCondtion);
        }

        public IQueryable<Praises> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Praises>(condtion, orderCondtions);
        }

        public IQueryable<Praises> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<Praises>(condtions);
        }

        public IQueryable<Praises> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<Praises>(condtions, orderCondtion);
        }

        public IQueryable<Praises> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<Praises>(condtions, orderCondtions);
        }
        public Praises GetEntity(Condtion condtion)
        {
            return base.GetEntity<Praises>(condtion);
        }

        public Praises GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<Praises>(condtions);
        }
        public bool CreateEntity(Praises entity)
        {
            return base.CreateEntity<Praises>(entity);
        }

        public void CreateEntitys(IList<Praises> entitys)
        {
            base.CreateEntitys<Praises>(entitys);
        }

        public bool UpdateEntity(Praises entity)
        {
            return base.UpdateEntity<Praises>(entity);
        }

        public void UpdateEntitys(IList<Praises> entitys)
        {
            base.UpdateEntitys<Praises>(entitys);
        }

        public bool DeleteEntity(Praises entity)
        {
            return base.DeleteEntity<Praises>(entity);
        }

        public void DeleteEntitys(IList<Praises> entitys)
        {
            base.DeleteEntitys<Praises>(entitys);
        }
        /// <summary>
        /// 赞评论
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public int PraisesComment(int userId, int commentId)
        {
            var success = false;
            var id = Execute<int>((db) =>
            {
                AssertUtil.AreBigger(userId, 0, LanguageUtil.Translate("api_Repository_Praises_PraisesComment_userId"));
                AssertUtil.AreBigger(commentId, 0, LanguageUtil.Translate("api_Repository_Praises_PraisesComment_commentId"));
                var user = db.User.FirstOrDefault(u => u.State == false && u.Id == userId);
                AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Repository_Praises_PraisesComment_user"));
                Comments comment = db.Comments.FirstOrDefault(c => c.State >= 0 && c.Id == commentId && c.EntityType == (int)CommentEnum.Video);
                AssertUtil.IsNotNull(comment, LanguageUtil.Translate("api_Repository_Praises_PraisesComment_comment"));
                AssertUtil.AreNotEqual(user.Id, comment.CreateUserId, LanguageUtil.Translate("api_Repository_Praises_PraisesComment_notPraisesSelf"));
                var praises = db.Praises.FirstOrDefault(p => p.State == true && p.CreateUserId == user.Id && p.ThemeId == comment.Id && p.ThemeTypeId == 2);
                AssertUtil.IsNull(praises, LanguageUtil.Translate("api_Repository_Praises_PraisesComment_praises"));
                var newpraises = new Praises()
                {
                    CreateTime = DateTime.Now,
                    CreateUserId = userId,
                    ThemeId = comment.Id,
                    ThemeTypeId = 2,
                    State = true
                };
                db.Praises.Add(newpraises);
                comment.PraisesNum = comment.PraisesNum + 1;//点赞次数加一
                db.Entry<Comments>(comment).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;

                success = db.SaveChanges() > 0;
                return newpraises.Id;
            });
            if (success)
            {
                CreateCache<Praises>();
                CreateCache<Comments>();
            }
            return id;
        }
        /// <summary>
        /// 取消赞评论
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public bool CancelPraisesComment(int userId, int commentId)
        {
            var success = Execute<bool>((db) =>
            {
                AssertUtil.AreBigger(userId, 0, LanguageUtil.Translate("api_Repository_Praises_CancelPraisesComment_userId"));
                AssertUtil.AreBigger(commentId, 0, LanguageUtil.Translate("api_Repository_Praises_CancelPraisesComment_commentId"));
                var user = db.User.FirstOrDefault(u => u.State == false && u.Id == userId);
                AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Repository_Praises_CancelPraisesComment_user"));
                Comments comment = db.Comments.FirstOrDefault(c => c.State >= 0 && c.Id == commentId && c.EntityType == (int)CommentEnum.Video);
                AssertUtil.IsNotNull(comment, LanguageUtil.Translate("api_Repository_Praises_CancelPraisesComment_comment"));
                AssertUtil.AreNotEqual(user.Id, comment.CreateUserId, LanguageUtil.Translate("api_Repository_Praises_CancelPraisesComment_notPraisesSelf"));
                var cancelPraises = db.Praises.FirstOrDefault(p => p.State == false && p.CreateUserId == user.Id && p.ThemeId == comment.Id && p.ThemeTypeId == 2);
                AssertUtil.IsNull(cancelPraises, LanguageUtil.Translate("api_Repository_Praises_CancelPraisesComment_cancelPraises"));
                var praises = db.Praises.FirstOrDefault(p => p.State == true && p.CreateUserId == user.Id && p.ThemeId == comment.Id && p.ThemeTypeId == 2);
                AssertUtil.IsNotNull(praises, LanguageUtil.Translate("api_Repository_Praises_CancelPraisesComment_praises"));
                db.Entry<Praises>(praises).State = EntityState.Deleted;
                comment.PraisesNum = comment.PraisesNum - 1;//点赞次数减一
                db.Entry<Comments>(comment).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            });
            if (success)
            {
                CreateCache<Praises>();
                CreateCache<Comments>();
            }
            return success;
        }

        /// <summary>
        /// 赞视频
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vedioId"></param>
        /// <returns></returns>
        public int PraisesVedio(int userId, int vedioId)
        {
            var success = false;
            var id = Execute<int>((db) =>
            {
                AssertUtil.AreBigger(userId, 0, LanguageUtil.Translate("api_Repository_Praises_PraisesVedio_userId"));
                AssertUtil.AreBigger(vedioId, 0, LanguageUtil.Translate("api_Repository_Praises_PraisesVedio_vedioId"));
                var user = db.User.FirstOrDefault(u => u.State == false && u.Id == userId);
                AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Repository_Praises_PraisesVedio_user"));
                var video = db.Video.FirstOrDefault(v => v.State == false && v.Id == vedioId);
                AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Repository_Praises_PraisesVedio_video"));
                var praises = db.Praises.FirstOrDefault(p => p.State == true && p.CreateUserId == user.Id && p.ThemeId == video.Id && p.ThemeTypeId == 2);
                AssertUtil.IsNull(praises, LanguageUtil.Translate("api_Repository_Praises_PraisesVedio_praises"));
                if (video.VideoSource)
                {
                    AssertUtil.AreNotEqual(user.Id, video.CreateManageId, LanguageUtil.Translate("api_Repository_Praises_PraisesVedio_notPraisesSelf"));
                }
                var newpraises = new Praises()
                {
                    CreateTime = DateTime.Now,
                    CreateUserId = userId,
                    ThemeId = (int)video.Id,
                    ThemeTypeId = 2,
                    State = true
                };
                db.Praises.Add(newpraises);
                video.PraiseCount = video.PraiseCount + 1;//点赞次数加一
                db.Entry<Video>(video).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                success = db.SaveChanges() > 0;
                return newpraises.Id;
            });
            if (success)
            {
                CreateCache<Praises>();
                CreateCache<Video>();
            }
            return id;
        }
    }
}