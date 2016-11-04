using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKSJ.WBVV.MVC.Client.Models
{
    public class UserModel
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户加密后的密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户播币
        /// </summary>
        public int BB { get; set; }
        /// <summary>
        /// 使用播币
        /// </summary>
        public int UseBB { get; set; }
        /// <summary>
        /// 播放次数
        /// </summary>
        public int PlayCount { get; set; }
        /// <summary>
        /// 粉丝数量
        /// </summary>
        public int FansCount { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 用户被订阅数
        /// </summary>
        public int SubscribeNum { get; set; }

        /// <summary>
        /// 用户状态（0:启用1:禁用）
        /// </summary>
        public bool State { get; set; }
        /// <summary>
        /// 用户皮肤id
        /// </summary>
        public int SkinId { get; set; }
    }
}