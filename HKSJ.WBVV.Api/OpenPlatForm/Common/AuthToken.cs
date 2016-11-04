using System;

namespace HKSJ.WBVV.Api.OpenPlatForm.Common
{
    public class AuthToken
    {
        #region //属性
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string OAuthId { get; set; }
        public int ExpiresIn { get; set; }
        public int ReExpiresIn { get; set; }

        public UserProfile User { get; set; }
        public string TraceInfo { get; set; }
        #endregion

        #region //构造方法
        public AuthToken()
        {
            User = new UserProfile();
        }
        #endregion
    }
}
