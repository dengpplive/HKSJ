using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IManageBusiness : IBaseBusiness
    {
        bool ChangePwd(string oldPwd, string newPwd, string confirmPwd);
        ManageView GetManageView(string loginName, string password);
        ManageView GetManageView(string loginName);
    }
}
