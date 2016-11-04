using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 我的订阅视图数据
    /// </summary>
    /// 
    [Serializable]
    public class UserSubscribeView
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
        /// 专辑
        /// </summary>
        public SpecialDetailView SpecialView { get; set; }

        /// <summary>
        /// 视频
        /// </summary>
        public List<VideoView> Videos { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserView UserView { get; set; }

    }
}
