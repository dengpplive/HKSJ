using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Base;
using HKSJ.WBVV.Repository.Interface;

namespace HKSJ.WBVV.Repository
{
  
     public class UserRecommendRepository : BaseRepository, IUserRecommendRepository
    {
        public IQueryable<UserRecommend> GetEntityList()
        {
            return base.GetEntityList<UserRecommend>();
        }

        public IQueryable<UserRecommend> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserRecommend>(orderCondtion);
        }

        public IQueryable<UserRecommend> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserRecommend>(orderCondtions);
        }

        public IQueryable<UserRecommend> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserRecommend>(condtion);
        }

        public IQueryable<UserRecommend> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserRecommend>(condtion, orderCondtion);
        }

        public IQueryable<UserRecommend> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserRecommend>(condtion, orderCondtions);
        }

        public IQueryable<UserRecommend> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserRecommend>(condtions);
        }

        public IQueryable<UserRecommend> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserRecommend>(condtions, orderCondtion);
        }

        public IQueryable<UserRecommend> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserRecommend>(condtions, orderCondtions);
        }
        public UserRecommend GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserRecommend>(condtion);
        }

        public UserRecommend GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserRecommend>(condtions);
        }
        public bool CreateEntity(UserRecommend entity)
        {
            return base.CreateEntity<UserRecommend>(entity);
        }

        public void CreateEntitys(IList<UserRecommend> entitys)
        {
            base.CreateEntitys<UserRecommend>(entitys);
        }

        public bool UpdateEntity(UserRecommend entity)
        {
            return base.UpdateEntity<UserRecommend>(entity);
        }

        public void UpdateEntitys(IList<UserRecommend> entitys)
        {
            base.UpdateEntitys<UserRecommend>(entitys);
        }

        public bool DeleteEntity(UserRecommend entity)
        {
            return base.DeleteEntity<UserRecommend>(entity);
        }

        public void DeleteEntitys(IList<UserRecommend> entitys)
        {
            base.DeleteEntitys<UserRecommend>(entitys);
        }
    }
}