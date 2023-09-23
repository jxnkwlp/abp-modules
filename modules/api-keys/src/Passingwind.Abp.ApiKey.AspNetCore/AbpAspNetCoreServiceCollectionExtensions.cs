using Microsoft.Extensions.DependencyInjection;
using Passingwind.AspNetCore.Authentication.ApiKey;

namespace Passingwind.Abp.ApiKey;

public static class AbpAspNetCoreServiceCollectionExtensions
{
    public static IServiceCollection ForwardIdentityAuthenticationForApiKey(this IServiceCollection services, string scheme = ApiKeyDefaults.AuthenticationScheme)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.ForwardDefaultSelector = ctx =>
            {
                if (ctx.IsApiKeyAuthorizationRequest())
                {
                    return scheme;
                }

                return null;
            };
        });

        return services;
    }
}
