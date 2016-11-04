using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Api.OpenPlatForm.Common;

namespace HKSJ.WBVV.Api.OpenPlatForm
{
    public interface IOAuthClient
    {
        AuthOption Option { get; }
        AuthToken Token { get; }
        IUserInterface User { get; }

        string GetAuthorizeUrl(ResponseType responseType);
        AuthToken GetAccessTokenByAuthorizationCode(string code);
        AuthToken GetAccessTokenByPassword(string passport, string password);
        AuthToken GetAccessTokenByRefreshToken(string refreshToken);
        string Get(string url, params RequestOption[] options);
        string Post(string url, params RequestOption[] options);

    }
}
