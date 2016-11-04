using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity.Response.App;

namespace HKSJ.WBVV.Business.Interface.APP
{
    public interface IUserSpaceBusiness : IBaseBusiness
    {
        AppUserSpaceView UserSpace(int loginUserId, int userId, int pageSize, int pageIndex);
        AppUserFanssView UserFans(int loginUserId, int userId, int pageSize, int pageIndex);
        AppUserFanssView UserSubscribe(int loginUserId, int userId, int pageSize, int pageIndex);
        AppMessageView Message(int loginUserId, int pageSize, int pageIndex);
        AppVideoCommentsView VideoComments(int loginUserId, int pageSize, int pageIndex);
        AppUserCollectsView VideoCollections(int loginUserId, int pageSize, int pageIndex);
        AppComments SpaceComments(int userId, int pageSize, int pageIndex);
        AppCommentsView SpaceComments(int userId, int pid, int pageSize, int pageIndex);
    }
}
