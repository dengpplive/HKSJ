using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;

namespace HKSJ.WBVV.Api.Provider
{
    /// <summary>
    /// Oauth认证，生成token
    /// Author ： AxOne
    /// </summary>
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        /*
        // 可在配置文件配置
        private const string ClientId = "wobo";
        private const string ClientSecret = "5bvv";
        private const string ClaimName = "5bvv.com";

        /// <summary>
        /// 验证客户[client_id 与 client_secret验证]
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var clientId = "";
            var clientSecret = "";
            context.TryGetBasicCredentials(out clientId, out clientSecret);
            if (context.Parameters.Any())
            {
                clientId = context.Parameters["clientId"] ?? "";
                clientSecret = context.Parameters["clientSecret"] ?? "";
            }
            if (clientId == ClientId && clientSecret == ClientSecret)
            {
                context.Validated(clientId);
            }
            else
            {
                context.SetError("invalid_client", "非法客户端");
                context.Rejected();
                return;
            }
            await base.ValidateClientAuthentication(context);
        }

        /// <summary>
        /// 客户端授权[ClientCredentialsGrant 生成 token]
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, ClaimName));
            var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties() {AllowRefresh = true});
            context.Validated(ticket);
            await base.GrantClientCredentials(context);
        }

        /// <summary>
        /// 客户端授权[ResourceOwnerPasswordCredentialsGrant 生成 token]
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //调用后台的登录服务验证用户名与密码
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());
            context.Validated(ticket);
            await base.GrantResourceOwnerCredentials(context);
        }

        /// <summary>
        /// 刷新Token[刷新refresh_token]
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            // 暂不提供
            if (context.Ticket == null || context.Ticket.Identity == null || !context.Ticket.Identity.IsAuthenticated)
            {
                context.SetError("invalid_grant", "token刷新失败");
            }
            else
            {
            }
            await base.GrantRefreshToken(context);
        }

        public override async Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId != ClientId) await Task.FromResult<object>(null);
            var expectedRootUri = new Uri(context.Request.Uri, "/");
            if (expectedRootUri.AbsoluteUri == context.RedirectUri)
            {
                context.Validated();
            }
            await Task.FromResult<object>(null);
        }

        */
    }


    /// <summary>
    /// Resource Owner Password Credentials Grant 授权
    /// </summary>
    public class PasswordAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        /*
        /// <summary>
        /// 验证客户端 [Authorization Basic Base64(clientId:clientSecret)|Authorization: Basic 5zsd8ewF0MqapsWmDwFmQmeF0Mf2gJkW]
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //var generateClientSecret = await _clientAuthorizationService.GenerateOAuthClientSecretAsync();

            //validate client credentials should be stored securely (salted, hashed, iterated)
            string clientId;
            string clientSecret;
            context.TryGetBasicCredentials(out clientId, out clientSecret);
            //手写验证
            var clientValid = clientId == "wobo" && clientSecret == "5bvv";
                //await _clientAuthorizationService.ValidateClientAuthorizationSecretAsync(clientId, clientSecret);
            if (!clientValid)
            {
                //context.Rejected();
                //context.SetError(AbpConstants.InvalidClient, AbpConstants.InvalidClientErrorDescription);
                context.SetError("invalid_clientId_clientSecret", "token获取失败");
                context.Rejected();
                return;
            }
            //need to make the client_id available for later security checks
            context.OwinContext.Set<string>("as:client_id", clientId);
            //context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", _clientAuthorizationProviderService.RefreshTokenLifeTime.ToString());
            context.Validated(clientId);
        }

        /// <summary>
        ///  验证用户名与密码 [Resource Owner Password Credentials Grant[username与password]|grant_type=password&username=irving&password=654321]
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {"*"});
            //validate user credentials (验证用户名与密码)  should be stored securely (salted, hashed, iterated) 
            //手写验证
            var userValid = context.UserName == "irving" && context.Password == "123456";
            //var userValid = await _accountService.ValidateUserNameAuthorizationPwdAsync(context.UserName, context.Password);
            if (!userValid)
            {
                //context.Rejected();
                //context.SetError(AbpConstants.AccessDenied, AbpConstants.AccessDeniedErrorDescription);
                context.SetError("invalid_userName_pwd", "token获取失败");
                context.Rejected();
                return;
            }
            var claimsIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            var ticket = new AuthenticationTicket(claimsIdentity, new AuthenticationProperties());
            context.Validated(ticket);
            /*
            //create identity
            var claimsIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            claimsIdentity.AddClaim(new Claim("sub", context.UserName));
            claimsIdentity.AddClaim(new Claim("role", "user"));
            // create metadata to pass on to refresh token provider
            var props = new AuthenticationProperties(new Dictionary<string, string>
                            {
                                {"as:client_id", context.ClientId }
                            });
            var ticket = new AuthenticationTicket(claimsIdentity, props);
            context.Validated(ticket);
            
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            if (context.Ticket == null || context.Ticket.Identity == null)
            {
                context.Rejected();
                return base.GrantRefreshToken(context);
            }
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.OwinContext.Get<string>("as:client_id");
            // enforce client binding of refresh token
            if (originalClient != currentClient)
            {
                context.Rejected();
                return base.GrantRefreshToken(context);
            }
            // chance to change authentication ticket for refresh token requests
            var claimsIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {"as:client_id", context.ClientId}
            });
            var newTicket = new AuthenticationTicket(claimsIdentity, props);
            context.Validated(newTicket);
            return base.GrantRefreshToken(context);
        }

        */
    }

    /// <summary>
    /// 刷新Token
    /// </summary>
    public class RefreshAuthenticationTokenProvider : AuthenticationTokenProvider
    {
        
        private readonly ConcurrentDictionary<string, string> _authenticationCodes =
            new ConcurrentDictionary<string, string>(StringComparer.Ordinal);

        /// <summary>
        /// 创建refreshToken
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        //public override async Task CreateAsync(AuthenticationTokenCreateContext context)
        //{
        //    if (string.IsNullOrEmpty(context.Ticket.Identity.Name)) return;
        //    var clietId = context.OwinContext.Get<string>("as:client_id");
        //    if (string.IsNullOrEmpty(clietId)) return;
        //    var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");
        //    //if (string.IsNullOrEmpty(refreshTokenLifeTime)) return;

        //    string ip = context.Request.RemoteIpAddress;
        //    int? port = context.Request.RemotePort;
        //    var token = new MyOAuthController.Token()
        //    {
        //        ClientId = clietId,
        //        UserName = context.Ticket.Identity.Name,
        //        IssuedUtc = DateTime.UtcNow,
        //        ExpiresUtc = DateTime.UtcNow.AddSeconds(Convert.ToDouble(refreshTokenLifeTime)),
        //    };
        //    context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
        //    context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;
        //    token.Access_token = context.SerializeTicket();
        //    token.Refresh_token =
        //        Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        //    //if (await _clientAuthorizationService.SaveTokenAsync(token))
        //    //{
        //        context.SetToken(token.Refresh_token);
        //    //}
        //    /*
        //    // maybe only create a handle the first time, then re-use for same client
        //    // copy properties and set the desired lifetime of refresh token
        //    var tokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
        //    {
        //        IssuedUtc = context.Ticket.Properties.IssuedUtc,
        //        ExpiresUtc = context.Ticket.Properties.ExpiresUtc
        //    };
        //    var token = context.SerializeTicket();
        //    var refreshTicket = new AuthenticationTicket(context.Ticket.Identity, tokenProperties);
        //    _refreshTokens.TryAdd(token, refreshTicket);
        //    // consider storing only the hash of the handle
        //    context.SetToken(token);
        //    */
        //}

        /// <summary>
        /// 刷新refreshToken[刷新access token时，refresh token也会重新生成]
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        //public override Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        //{
        //    string token = context.Token;
        //    string value;
        //    if (_authenticationCodes.TryRemove(context.Token, out value))
        //    {
        //        context.DeserializeTicket(value);
        //    }
        //    return base.ReceiveAsync(context);
        //}

        
    }
}
