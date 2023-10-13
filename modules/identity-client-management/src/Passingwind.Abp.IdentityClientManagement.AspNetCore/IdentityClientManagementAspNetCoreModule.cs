using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.IdentityClientManagement.Identity;
using Passingwind.AspNetCore.Authentication.Saml2;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Authentication.OpenIdConnect;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Passingwind.Abp.IdentityClientManagement;

[DependsOn(
    typeof(IdentityClientManagementDomainModule),
    typeof(AbpAspNetCoreAuthenticationOpenIdConnectModule),
    typeof(AbpAspNetCoreModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpMultiTenancyModule)
)]
public class IdentityClientManagementAspNetCoreModule : AbpModule
{
    private static readonly AsyncOneTimeRunner OneTimeRunner = new AsyncOneTimeRunner();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<OpenIdConnectEventType>();
        context.Services.AddTransient<Saml2EventType>();

        context.Services.AddOptions<IdentityClientProviderOption>();

        context.Services.AddSingleton<OpenIdConnectPostConfigureOptions>();
        context.Services.AddSingleton<Saml2PostConfigureOptions>();

        context.Services.AddHttpClient();
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var service = context.ServiceProvider.GetRequiredService<IIdentityClientRegisterProvider>();

        await OneTimeRunner.RunAsync(async () => await service.RegisterAllAsync());
    }
}
