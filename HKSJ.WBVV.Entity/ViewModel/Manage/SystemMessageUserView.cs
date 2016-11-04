using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Manage
{
    public class SystemMessageUserView
    {
        /// <summary>
        /// 推送的用户
        /// </summary>
        public IList<SystemMessageUser> SelectUser { get; set; }
        /// <summary>
        /// 剩余的用户
        /// </summary>
        public IList<SystemMessageUser> SurplusUser { get; set; }
    }
    /// <summary>
    /// 系统消息用户
    /// </summary>
    public class SystemMessageUser
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
    }
}
