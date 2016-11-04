using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Common.Extender.LinqExtender;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IUserRecommendBusiness : IBaseBusiness
    {
        PageResult UserRecommendPagerList(int userId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
        IList<UserRecommendView> GetUserRecommendList(int userId,int loginUserId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);

        ResultView<bool> CreateUserRecommend(IList<UserRecommend> userRecommends);
    }
}
