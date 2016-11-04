using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 我的粉丝视图数据
    /// </summary>
    [Serializable]
    public class UserFansView
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///  true 已订阅 false 未订阅
        /// </summary>
        public bool State { get; set; }
        /// <summary>
        /// 当前登录人对这个用户的订阅状态  true 已订阅 false 未订阅
        /// </summary>
        public bool LoginSubState { get; set; }
        /// <summary>
        /// 该粉丝的视频数
        /// </summary>
        public int VideoCount { get; set; }
        /// <summary>
        /// 动态
        /// </summary>
        public int DynamicCount { get; set; }
        /// <summary>
        /// 粉丝数
        /// </summary>
        public int FansCount { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserView UserView { get; set; }

        /// <summary>
        /// 粉丝更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}
