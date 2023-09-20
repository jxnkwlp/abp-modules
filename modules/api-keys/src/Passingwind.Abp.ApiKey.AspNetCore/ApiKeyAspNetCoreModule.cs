using Microsoft.Extensions.DependencyInjection;
using Passingwind.AspNetCore.Authentication.ApiKey;
using Volo.Abp.AspNetCore;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.ApiKey;

[DependsOn(
    typeof(ApiKeyDomainModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpAspNetCoreModule)
    )]
public class ApiKeyAspNetCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var abpOptions = context.Services.ExecutePreConfiguredActions<AbpApiKeyOptions>();
        var apiKeyOptions = context.Services.ExecutePreConfiguredActions<ApiKeyOptions>();

        if (abpOptions.ConfigreAuthentication)
        {
            context
                .Services
                .AddAuthentication()
                .AddApiKey<ApiKeyProvider>(options => options.Realm = options.Realm);

            context.Services.ConfigureApplicationCookie(options =>
            {
                options.ForwardDefaultSelector = (context) =>
                {
                    if (context.IsApiKeyAuthorizationRequest(apiKeyOptions.QueryStringName ?? ApiKeyDefaults.QueryStringName, apiKeyOptions.HeaderName ?? ApiKeyDefaults.HeaderName, apiKeyOptions.HeaderAuthenticationSchemeName ?? ApiKeyDefaults.HeaderAuthenticationSchemeName))
                    {
                        return ApiKeyDefaults.AuthenticationScheme;
                    }

                    return null;
                };
            });
        }
    }
}
