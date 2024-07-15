using ApiMultiAuthe.Auth.AuthOffline;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ApiMultiAuthe.Auth.AuthOnline
{
    public class AuthOnlineHandler : AuthenticationHandler<AuthOnlineOptions>
    {
        private readonly IConfiguration _config;
        public AuthOnlineHandler(IOptionsMonitor<AuthOnlineOptions> options, ILoggerFactory logger, UrlEncoder encoder, IConfiguration config) : base(options, logger, encoder)
        {
            _config = config;
        }

        public AuthOnlineHandler(IOptionsMonitor<AuthOnlineOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration config) : base(options, logger, encoder, clock)
        {
            _config = config;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!_config.GetValue<bool>("AuthOnline") == true) 
            {
                var response = await Context.AuthenticateAsync(AuthOfflineDefaults.AuthenticationScheme);
                return response;
            }

            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity("admin")), AuthOnlineDefaults.AuthenticationScheme));
        }
    }
}
