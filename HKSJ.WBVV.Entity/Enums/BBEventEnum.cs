using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.Enums
{
    public enum AuthUserType
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Admin = 1,
        /// <summary>
        /// 普通用户
        /// </summary>
        [Description("普通用户")]
        General = 2,
        /// <summary>
        /// 全局
        /// </summary>
        [Description("全局")]
        Application = 3
    }
}
