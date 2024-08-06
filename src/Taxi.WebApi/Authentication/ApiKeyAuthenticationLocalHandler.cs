using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Taxi.WebApi.Authentication;

public class ApiKeyAuthenticationLocalHandler(
    IOptionsMonitor<ApiKeyAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<ApiKeyAuthenticationOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authenticationTicket = new AuthenticationTicket(CreateClaimsPrincipal(), Scheme.Name);

        return await Task.FromResult(AuthenticateResult.Success(authenticationTicket));
    }

    private ClaimsPrincipal CreateClaimsPrincipal()
    {
        var claims = new[] { new Claim(ClaimTypes.Name, "LocalUser") };
        var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
        return new ClaimsPrincipal(claimsIdentity);
    }
}