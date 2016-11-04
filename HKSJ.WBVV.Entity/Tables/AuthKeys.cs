using System;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// API授权验证表
    /// </summary>
    [Serializable]
    public class AuthKeys
    {
        #region 私有字段
        private Int32 _id;
        private Int32 _userId;
        private Int32 _userType;
        private string _publicKey;
        private string _privateKey;
        private string _accessToken;
        private string _refreshToken;
        private int? _expireIn;
        private DateTime _createTime;
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

        public Int32 UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// 用户类型(1:管理员  2:普通用户)
        /// </summary>
        public Int32 UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        public string PublicKey
        {
            get { return _publicKey; }
            set { _publicKey = value; }
        }

        public string PrivateKey
        {
            get { return _privateKey; }
            set { _privateKey = value; }
        }

        public string AccessToken
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }

        public string RefreshToken
        {
            get { return _refreshToken; }
            set { _refreshToken = value; }
        }
        public Int32? ExpireIn
        {
            get { return _expireIn; }
            set { _expireIn = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        #endregion
    }
}