using System;

namespace HKSJ.WBVV.Entity.ApiParaModel
{
    /// <summary>
    /// 账户修改请求参数
    /// </summary>
    [Serializable]
    public class AccountPara
    {
        public int Id { get; set; }
        public int? Gender { get; set; }
        public string Birthdate { get; set; }
        public string Province { get; set; }
        public int? ProvinceCode { get; set; }
        public string City { get; set; }
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
        public string NickName { get; set; }

    }
}
