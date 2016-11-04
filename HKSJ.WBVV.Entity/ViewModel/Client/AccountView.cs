using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Extender;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 用户账户信息
    /// </summary>
    [Serializable]
    public class AccountView
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int Id { get; set; }
        public int Gender { get; set; }
        public string Birthdate { get; set; }
        public string Province { get; set; }
        public int? ProvinceCode { get; set; }
        public string City { get; set; }
        public string Pwd { get; set; }
        public int? CityCode { get; set; }
        /// <summary>
        /// 生肖
        /// </summary>
        public short? Zodiac { get; set; }
        /// <summary>
        /// 星座
        /// </summary>
        public short? Constellation { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Bardian { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CurrentLoginIp { get; set; }
        public string LastLoginIp { get; set; }
        public string NickName { get; set; }
    }
}