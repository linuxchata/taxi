using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Taxi.WebApi.Authentication;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(Header.ApiKey, out var apiKeyValues))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        var providedApiKey = apiKeyValues.FirstOrDefault();
        var expectedApiKey = Options.ApiKey;

        if (!string.Equals(providedApiKey, expectedApiKey.FromSecureString(), StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        var authenticationTicket = new AuthenticationTicket(CreateClaimsPrincipal(), Scheme.Name);

        return await Task.FromResult(AuthenticateResult.Success(authenticationTicket));
    }

    private ClaimsPrincipal CreateClaimsPrincipal()
    {
        var claims = new[] { new Claim(ClaimTypes.Name, "ApiKeyUser") };
        var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
        return new ClaimsPrincipal(claimsIdentity);
    }
}