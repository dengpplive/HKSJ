using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Common.Email.Enum
{
    public enum EmailEnum
    {
        [Description("我播——验证激活账号")]
        Register = 1,
        [Description("我播——验证找回密码")]
        FindPwd = 2,
        [Description("我播——验证修改邮箱")]
        UpdateEmail = 3,
    }
}
