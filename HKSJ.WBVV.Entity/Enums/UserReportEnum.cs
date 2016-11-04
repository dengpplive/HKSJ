using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.Enums
{
    /// <summary>
    /// 举报类型枚举
    /// </summary>
    public enum UserReportEnum
    {
        [Description("评论")]
        Comment = 1,
        [Description("视频")]
        Video = 2
    }
}
