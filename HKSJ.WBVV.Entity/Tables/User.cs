


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public class User
    {
        #region 私有字段
        private Int32 _id;
        private string _account;
        private string _password;
        private string _nickName;
        private Int16 _userState = 0;
        private bool _state = false;
        private DateTime? _birthdate;
        private Int32? _age;
        private bool? _gender;
        private string _email;
        private string _phone;
        private Int32 _bB = 0;
        private Int32 _playCount = 0;
        private Int32 _fansCount = 0;
        private string _bannerImage;
        private string _picture;
        private string _iDCardNo;
        private Int16 _level = 0;
        private Int16? _zodiac;
        private Int16? _constellation;
        private string _bardian;
        private DateTime _createTime;
        private Int32? _updateUserId;
        private DateTime? _updateTime;
        private Int32 _subscribeNum = 0;
        private string _province;
        private Int32? _provinceCode;
        private string _city;
        private Int32? _cityCode;
        private Int32 _useBB = 0;
        private DateTime? _lastLoginTime;
        private DateTime? _currentLoginTime;
        private string _lastLoginIp;
        private string _currentLoginIp;
        #endregion
        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [Key]
        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 帐号
        /// </summary>
        /// <returns></returns>
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        /// <returns></returns>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// 昵称
        /// </summary>
        /// <returns></returns>
        public string NickName
        {
            get { return _nickName; }
            set { _nickName = value; }
        }

        /// <summary>
        /// 用户状态（0:离线1:在线2:.隐身）
        /// </summary>
        /// <returns></returns>
        public Int16 UserState
        {
            get { return _userState; }
            set { _userState = value; }
        }

        /// <summary>
        /// 用户状态（0:启用1:禁用）
        /// </summary>
        /// <returns></returns>
        public bool State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 出生日期
        /// </summary>
        /// <returns></returns>
        public DateTime? Birthdate
        {
            get { return _birthdate; }
            set { _birthdate = value; }
        }

        /// <summary>
        /// 年龄
        /// </summary>
        /// <returns></returns>
        public Int32? Age
        {
            get { return _age; }
            set { _age = value; }
        }

        /// <summary>
        /// 性别(1:男0:女)
        /// </summary>
        /// <returns></returns>
        public bool? Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        /// <returns></returns>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// 手机号
        /// </summary>
        /// <returns></returns>
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        /// <summary>
        /// 用户播币
        /// </summary>
        /// <returns></returns>
        public Int32 BB
        {
            get { return _bB; }
            set { _bB = value; }
        }

        /// <summary>
        /// 播放数量
        /// </summary>
        /// <returns></returns>
        public Int32 PlayCount
        {
            get { return _playCount; }
            set { _playCount = value; }
        }

        /// <summary>
        /// 粉丝数量
        /// </summary>
        /// <returns></returns>
        public Int32 FansCount
        {
            get { return _fansCount; }
            set { _fansCount = value; }
        }

        /// <summary>
        /// 空间背景图片
        /// </summary>
        /// <returns></returns>
        public string BannerImage
        {
            get { return _bannerImage; }
            set { _bannerImage = value; }
        }

        /// <summary>
        /// 头像
        /// </summary>
        /// <returns></returns>
        public string Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        /// <summary>
        /// 身份证号
        /// </summary>
        /// <returns></returns>
        public string IDCardNo
        {
            get { return _iDCardNo; }
            set { _iDCardNo = value; }
        }

        /// <summary>
        /// 用户等级
        /// </summary>
        /// <returns></returns>
        public Int16 Level
        {
            get { return _level; }
            set { _level = value; }
        }

        /// <summary>
        /// 生肖(1鼠	2牛3虎4兔5龙6蛇7马8羊9猴10鸡11狗12猪)
        /// </summary>
        /// <returns></returns>
        public Int16? Zodiac
        {
            get { return _zodiac; }
            set { _zodiac = value; }
        }

        /// <summary>
        /// 星座
        /// </summary>
        /// <returns></returns>
        public Int16? Constellation
        {
            get { return _constellation; }
            set { _constellation = value; }
        }

        /// <summary>
        /// 个性签名
        /// </summary>
        /// <returns></returns>
        public string Bardian
        {
            get { return _bardian; }
            set { _bardian = value; }
        }

        /// <summary>
        /// 注册时间
        /// </summary>
        /// <returns></returns>
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        /// <summary>
        /// 修改用户编号
        /// </summary>
        /// <returns></returns>
        public Int32? UpdateUserId
        {
            get { return _updateUserId; }
            set { _updateUserId = value; }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }

        /// <summary>
        /// 用户主动订阅数(订阅别人)
        /// </summary>
        /// <returns></returns>
        public Int32 SubscribeNum
        {
            get { return _subscribeNum; }
            set { _subscribeNum = value; }
        }

        /// <summary>
        /// 省份名称
        /// </summary>
        /// <returns></returns>
        public string Province
        {
            get { return _province; }
            set { _province = value; }
        }

        /// <summary>
        /// 省份代码
        /// </summary>
        /// <returns></returns>
        public Int32? ProvinceCode
        {
            get { return _provinceCode; }
            set { _provinceCode = value; }
        }

        /// <summary>
        /// 城市名称
        /// </summary>
        /// <returns></returns>
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        /// <summary>
        /// 城市代码
        /// </summary>
        /// <returns></returns>
        public Int32? CityCode
        {
            get { return _cityCode; }
            set { _cityCode = value; }
        }

        /// <summary>
        /// 提现播币
        /// </summary>
        /// <returns></returns>
        public Int32 UseBB
        {
            get { return _useBB; }
            set { _useBB = value; }
        }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        /// <returns></returns>
        public DateTime? LastLoginTime
        {
            get { return _lastLoginTime; }
            set { _lastLoginTime = value; }
        }

        /// <summary>
        /// 当前登录时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CurrentLoginTime
        {
            get { return _currentLoginTime; }
            set { _currentLoginTime = value; }
        }

        /// <summary>
        /// 上次登录IP
        /// </summary>
        /// <returns></returns>
        public string LastLoginIp
        {
            get { return _lastLoginIp; }
            set { _lastLoginIp = value; }
        }

        /// <summary>
        /// 当前登录IP
        /// </summary>
        /// <returns></returns>
        public string CurrentLoginIp
        {
            get { return _currentLoginIp; }
            set { _currentLoginIp = value; }
        }
        /// <summary>
        /// 关联的皮肤编号
        /// </summary>
        public int SkinId { get; set; }

        #endregion
    }
}