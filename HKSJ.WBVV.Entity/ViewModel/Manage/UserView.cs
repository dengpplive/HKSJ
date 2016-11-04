using System;

namespace HKSJ.WBVV.Entity.ViewModel.Manage
{
    /// <summary>
    /// 用户
    /// </summary>
    [Serializable]
    public class UserView
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
        /// 出生日期
        /// </summary>
        /// <returns></returns>
        public string Birthdate { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        /// <returns></returns>
        public int? Age { get; set; }
        /// <summary>
        /// 性别(1:男0:女)
        /// </summary>
        /// <returns></returns>
        public string Gender { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        /// <returns></returns>
        public string Email { get; set; }
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
        /// 个性签名
        /// </summary>
        public string Bardian { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 用户被订阅数
        /// </summary>
        public int SubscribeNum { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        /// <returns></returns>
        public string IDCardNo { get; set; }
        /// <summary>
        /// 用户等级
        /// </summary>
        /// <returns></returns>
        public short Level { get; set; }
        /// <summary>
        /// 生肖(1鼠	2牛3虎4兔5龙6蛇7马8羊9猴10鸡11狗12猪)
        /// </summary>
        /// <returns></returns>
        public string Zodiac { get; set; }
        /// <summary>
        /// 星座
        /// </summary>
        /// <returns></returns>
        public string Constellation { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        /// <returns></returns>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        /// <returns></returns>
        public string Province { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        /// <returns></returns>
        public string City { get; set; }
        /// <summary>
        /// 上次登录时间
        /// </summary>
        /// <returns></returns>
        public string LastLoginTime { get; set; }
        /// <summary>
        /// 用户状态（0:启用1:禁用）
        /// </summary>
        public string State { get; set; }
    }
}