using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.Enums
{
    public enum IntegrationDetailEnum
    {
        [Description("播币获取")]
        Income= 1,
        [Description("播币提现")]
        Expenditure = 2,
        [Description("播币转入")]
        Into = 3
    }
}
