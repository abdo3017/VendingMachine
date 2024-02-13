using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace VendingMachineTest.Utilts
{
    internal class IntegrationTestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public IntegrationTestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
          ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
          : base(options, logger, encoder, clock)
        {
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name, Helper.UserName),
                new Claim(ClaimTypes.Role, Helper.Role),
                new Claim("uid", Helper.UserId)
            };
            var identity = new ClaimsIdentity(claims, "bearer");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "bearer");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
