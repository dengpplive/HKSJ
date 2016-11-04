using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HKSJ.Utilities;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Repository.Interface;


namespace HKSJ.WBVV.Business
{
    /// <summary>
    /// 用户推荐
    /// </summary>
    public class UserRecommendBusiness : BaseBusiness, IUserRecommendBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRecommendRepository _userRecommendRepository;
        private readonly IUserFansRepository _userFansRepository;
        public UserRecommendBusiness(IUserRepository userRepository, IUserRecommendRepository userRecommendRepository, IUserFansRepository userFansRepository)
        {
            this._userRepository = userRepository;
            this._userRecommendRepository = userRecommendRepository;
            _userFansRepository = userFansRepository;
        }



        public PageResult UserRecommendPagerList(int userId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            throw new NotImplementedException();
        }

        public IList<UserRecommendView> GetUserRecommendList(int userId, int loginUserId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            IQueryable<UserRecommendView> userRecommend = null;
            if (loginUserId <= 0)
            {
                userRecommend = (from u in this._userRepository.GetEntityList(CondtionEqualState())
                                 orderby u.FansCount descending
                                 select new UserRecommendView()
                                 {
                                     UserView = new UserView()
                                     {
                                         Id = u.Id,
                                         NickName = u.NickName,
                                         Picture = u.Picture
                                     }
                                 }).AsQueryable();
            }
            else
            {
                userRecommend = (from u in this._userRepository.GetEntityList(CondtionEqualState())
                                 where IsSubed(this._userFansRepository.GetEntityList(), u.Id, loginUserId) == false&&u.Id!=loginUserId
                                 orderby u.FansCount descending
                                 select new UserRecommendView()
                                 {
                                     UserView = new UserView()
                                     {
                                         Id = u.Id,
                                         NickName = u.NickName,
                                         Picture = u.Picture
                                     }
                                 }).AsQueryable();
            }
            var user = userRecommend.Take(this.PageSize).ToList();
            return user;
        }
        public ResultView<bool> CreateUserRecommend(IList<UserRecommend> userRecommends)
        {
            throw new NotImplementedException();
        }
    }
}
