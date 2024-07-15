using ApiMultiAuthe.Auth.AuthOnline;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ApiMultiAuthe.Auth.AuthOffline
{
    public class AuthOfflineHandler : AuthenticationHandler<AuthOfflineOptions>
    {
        private readonly IConfiguration _config;

        public AuthOfflineHandler(IOptionsMonitor<AuthOfflineOptions> options, ILoggerFactory logger, UrlEncoder encoder, IConfiguration config) : base(options, logger, encoder)
        {
            _config = config;
        }

        public AuthOfflineHandler(IOptionsMonitor<AuthOfflineOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration config) : base(options, logger, encoder, clock)
        {
            _config = config;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (_config.GetValue<bool>("AuthOnline") == true)
            {
                return AuthenticateResult.NoResult();
            }

            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity("admin")), AuthOnlineDefaults.AuthenticationScheme));
        }
    }
}
