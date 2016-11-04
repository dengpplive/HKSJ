using System;
using System.Collections.Generic;

namespace HKSJ.WBVV.Api.OpenPlatForm.Common
{
    public class AuthOption
    {
        #region //属性
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string ApiUrlBase { get; set; }
        public string AuthorizeUrl { get; set; }
        public string AccessTokenUrl { get; set; }
        public string CallbackUrl { get; set; }

        public string Display { get; set; }
        public string State { get; set; }
        public string Scope { get; set; }

        public IDictionary<string, string> Urls { get; set; }
        #endregion

        #region //构造方法
        public AuthOption()
        {
            this.Urls = new Dictionary<string, string>();
            this.Display = "default ";
        }
        #endregion
    }
}
