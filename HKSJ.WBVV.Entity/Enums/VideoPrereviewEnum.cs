using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.Enums
{
    /// <summary>
    /// 审片状态
    /// </summary>
    public enum VideoPrereviewEnum
    {
        [Description("必火")]
        Good = 1,
        [Description("差评")]
        Bad = 2
      
    }
}
