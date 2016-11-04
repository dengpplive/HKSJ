using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel.Manage;

namespace HKSJ.WBVV.Business.Interface
{
    public interface ISystemMessageBusiness : IBaseBusiness
    {
        int CreateSystemMessage(short userByType, string userBy, string messageDesc);
        SystemMessageUserView GetUsers(string userBy);
        SysMessage GetSystemMessage(int id);
        PageResult GetSystemMessagePageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
    }
}
