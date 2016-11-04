using System;

namespace HKSJ.WBVV.Api.OpenPlatForm.Common
{
    public class UserProfile
    {
        #region //属性
        public string OAuthId { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string AvatarUrl { get; set; }
        public string Description { get; set; }

        public string Info { get; set; }
        #endregion

        #region //构造方法
        public UserProfile()
        {
            Sex = "2"; //0：女 | 1：男 | 2：保密
        }
        #endregion
    }
}
