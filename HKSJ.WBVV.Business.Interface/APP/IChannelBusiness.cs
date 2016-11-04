using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity.Response.App;

namespace HKSJ.WBVV.Business.Interface.APP
{
    public interface IChannelBusiness : IBaseBusiness
    {
        AppCategorysView Category(int pageSize, int pageIndex);
        AppChannelView Channel(int? loginUserId);
    }
}
