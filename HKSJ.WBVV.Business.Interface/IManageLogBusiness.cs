using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IManageLogBusiness : IBaseBusiness
    {
        bool CreateManageLog(int? logCode, string logContent, string descc );
    }
}
