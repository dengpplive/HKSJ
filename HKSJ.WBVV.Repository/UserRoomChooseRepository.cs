using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Base;
using HKSJ.WBVV.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Repository
{
    public class UserRoomChooseRepository : BaseRepository, IUserRoomChooseRepository
    {
        public IQueryable<UserRoomChoose> GetEntityList()
        {
            return base.GetEntityList<UserRoomChoose>();
        }

        public IQueryable<UserRoomChoose> GetEntityList(OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserRoomChoose>(orderCondtion);
        }

        public IQueryable<UserRoomChoose> GetEntityList(IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserRoomChoose>(orderCondtions);
        }

        public IQueryable<UserRoomChoose> GetEntityList(Condtion condtion)
        {
            return base.GetEntityList<UserRoomChoose>(condtion);
        }

        public IQueryable<UserRoomChoose> GetEntityList(Condtion condtion, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserRoomChoose>(condtion, orderCondtion);
        }

        public IQueryable<UserRoomChoose> GetEntityList(Condtion condtion, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserRoomChoose>(condtion, orderCondtions);
        }

        public IQueryable<UserRoomChoose> GetEntityList(IList<Condtion> condtions)
        {
            return base.GetEntityList<UserRoomChoose>(condtions);
        }

        public IQueryable<UserRoomChoose> GetEntityList(IList<Condtion> condtions, OrderCondtion orderCondtion)
        {
            return base.GetEntityList<UserRoomChoose>(condtions, orderCondtion);
        }

        public IQueryable<UserRoomChoose> GetEntityList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            return base.GetEntityList<UserRoomChoose>(condtions, orderCondtions);
        }
        public UserRoomChoose GetEntity(Condtion condtion)
        {
            return base.GetEntity<UserRoomChoose>(condtion);
        }

        public UserRoomChoose GetEntity(IList<Condtion> condtions)
        {
            return base.GetEntity<UserRoomChoose>(condtions);
        }
        public bool CreateEntity(UserRoomChoose entity)
        {
            return base.CreateEntity<UserRoomChoose>(entity);
        }

        public void CreateEntitys(IList<UserRoomChoose> entitys)
        {
            base.CreateEntitys<UserRoomChoose>(entitys);
        }

        public bool UpdateEntity(UserRoomChoose entity)
        {
            return base.UpdateEntity<UserRoomChoose>(entity);
        }

        public void UpdateEntitys(IList<UserRoomChoose> entitys)
        {
            base.UpdateEntitys<UserRoomChoose>(entitys);
        }

        public bool DeleteEntity(UserRoomChoose entity)
        {
            return base.DeleteEntity<UserRoomChoose>(entity);
        }

        public void DeleteEntitys(IList<UserRoomChoose> entitys)
        {
            base.DeleteEntitys<UserRoomChoose>(entitys);
        }
    }
}
