using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Manage
{
    public class SystemMessageView
    {
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        public Int32 Id { get; set; }
        /// <summary>
        /// 系统通知内容
        /// </summary>
        /// <returns></returns>
        public string MessageDesc { get; set; }
        /// <summary>
        /// 用户发送类型(1:选中用户,2:选中用户类型3:全体用户（默认）)
        /// </summary>
        /// <returns></returns>
        public int UserByType { get; set; }
        /// <summary>
        /// 接受用户
        /// </summary>
        /// <returns></returns>
        public IList<string> UserBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public string CreateTime { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        /// <returns></returns>
        public string LoginName { get; set; }
    }
}
