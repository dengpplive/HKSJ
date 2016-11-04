using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.Enums
{
    public enum CommentEnum
    {
        [Description("用户空间")]
        User = 1,
        [Description("视频")]
        Video = 2
    }
    public enum CommentStateEnum
    {
        [Description("审核不通过")]
        Rejected = -2,
        [Description("删除")]
        Deleted = -1,
        [Description("待审核")]
        Waiting = 0,
        [Description("审核通过")]
        Approved = 1
    }
}
