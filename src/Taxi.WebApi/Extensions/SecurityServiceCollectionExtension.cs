﻿using System.Net;
using Taxi.WebApi.Authentication;

namespace Taxi.WebApi.ServiceCollectionExtensions;

/// <summary>
/// Contains extension methods to <see cref="IServiceCollection"/>
/// </summary>
public static class SecurityServiceCollectionExtension
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var apiKey = configuration.GetValue<string>("ApiKey")!.ToSecureString();

        services
            .AddAuthentication(Scheme.ApiKey)
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                Scheme.ApiKey,
                x => x.ApiKey = new NetworkCredential(string.Empty, apiKey).SecurePassword);

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policy.ApiKey, policy =>
            {
                policy.AddAuthenticationSchemes(Scheme.ApiKey);
                policy.RequireAuthenticatedUser();
            });
        });

        return services;
    }
}