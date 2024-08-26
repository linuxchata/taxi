using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Taxi.WebApi.Authentication;

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<ApiKeyAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<ApiKeyAuthenticationOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(Header.ApiKey, out var apiKeyValues))
        {
            return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        }

        var providedApiKey = apiKeyValues.FirstOrDefault();
        var expectedApiKey = Options.ApiKey;

        if (!string.Equals(providedApiKey, expectedApiKey.FromSecureString(), StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        }

        var authenticationTicket = new AuthenticationTicket(CreateClaimsPrincipal(), Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
    }

    private ClaimsPrincipal CreateClaimsPrincipal()
    {
        var claims = new[] { new Claim(ClaimTypes.Name, "ApiKeyUser") };
        var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
        return new ClaimsPrincipal(claimsIdentity);
    }
}