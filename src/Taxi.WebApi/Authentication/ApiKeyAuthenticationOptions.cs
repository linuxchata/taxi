using System.Security;
using Microsoft.AspNetCore.Authentication;

namespace Taxi.WebApi.Authentication;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public SecureString ApiKey { get; set; }
}