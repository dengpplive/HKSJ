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
    public interface IUserVisitLogBusiness : IBaseBusiness
    {
        PageResult UserVisitLogPagerList(int userId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);

        IList<UserVisitView> GetUserVisitLogList(int userId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);

        ResultView<bool> CreateAndUpdateUserVisitLog(int CreateUserId, int VisitorUserId, int VisitedUserId);

        ResultView<bool> DeleteUserVisitLogs(List<UserVisitLog> userVisitLogs);

        ResultView<bool> DeleteUserVisitLog(int Id);

    }
}
