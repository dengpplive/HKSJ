using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.Enums
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum SystemMessageEnum
    {
        [Description("指定用户")]
        ByUser = 1,
        [Description("指定用户等级")]
        UserLeve = 2,
        [Description("全体用户")]
        AllUser = 3
    }
    /// <summary>
    /// 消息通知类型
    /// </summary>
    public enum SysMessageEnum
    {
        [Description("系统消息")]
        SystemMessage = 1,
        [Description("评论")]
        VideoComment = 2,
        [Description("留言")]
        SpaceComment = 3,
        [Description("喜欢")]
        UserCollect = 4,
    }
    /// <summary>
    /// 消息状态
    /// </summary>
    public enum SystemMessageStateEnum
    {
        [Description("已读")]
        Read = 0,
        [Description("删除")]
        Deleted = 1
    }
}
