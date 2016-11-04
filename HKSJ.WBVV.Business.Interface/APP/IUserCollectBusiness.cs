using System.Collections.Generic;
using HKSJ.WBVV.Entity.Response.App;
using HKSJ.WBVV.Business.Interface.Base;

namespace HKSJ.WBVV.Business.Interface.APP
{
    /// <summary>
    /// 收藏
    /// </summary>
    public interface IUserCollectBusiness : IBaseBusiness
    {
        AppChoicenesssView GetUserCollects(int userId, int pageIndex, int pageSize);


    }
}
