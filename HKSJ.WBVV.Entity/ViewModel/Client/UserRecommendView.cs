using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 用户推荐视图
    /// </summary>
    /// 
    [Serializable]
    public class UserRecommendView
    {
        /// <summary>
        /// true 该用户已订阅 false 未订阅
        /// </summary>
        public bool State { get; set; }
        /// <summary>
        /// 推荐信息
        /// </summary>
        public UserRecommend UserRecommend { get; set; }
        /// <summary>
        /// 推荐的用户信息
        /// </summary>
        public UserView UserView { get; set; }
    }
}
