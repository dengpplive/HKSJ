using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Api.OpenPlatForm.Common;

namespace HKSJ.WBVV.Api.OpenPlatForm.OauthClient
{
    public abstract class OAuthClient : IOAuthClient
    {
        public AuthOption Option
        {
            get { throw new NotImplementedException(); }
        }

        public AuthToken Token
        {
            get { throw new NotImplementedException(); }
        }

        public IUserInterface User
        {
            get { throw new NotImplementedException(); }
        }

        public string GetAuthorizeUrl(Common.ResponseType responseType)
        {
            throw new NotImplementedException();
        }

        public AuthToken GetAccessTokenByAuthorizationCode(string code)
        {
            throw new NotImplementedException();
        }

        public AuthToken GetAccessTokenByPassword(string passport, string password)
        {
            throw new NotImplementedException();
        }

        public AuthToken GetAccessTokenByRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public string Get(string url, params Common.RequestOption[] options)
        {
            throw new NotImplementedException();
        }

        public string Post(string url, params Common.RequestOption[] options)
        {
            throw new NotImplementedException();
        }
    }
}
