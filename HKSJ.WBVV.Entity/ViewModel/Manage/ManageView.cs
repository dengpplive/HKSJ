using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Manage
{
    public class ManageView
    {
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        public Int32 Id { get; set; }
        /// <summary>
        /// 登录账户
        /// </summary>
        /// <returns></returns>
        public string LoginName { get; set; }  
        /// <summary>
        /// 邮箱
        /// </summary>
        /// <returns></returns>
        public string Email{get;set;}
        /// <summary>
        /// 昵称
        /// </summary>
        /// <returns></returns>
        public string NickName { get; set; }
        /// <summary>
        /// 凭证
        /// </summary>
        public string Token { get; set; }
    }
}
