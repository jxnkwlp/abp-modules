using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Passingwind.Abp.Identity.AspNetCore;

[DependsOn(
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpIdentityApplicationModule),
    typeof(IdentityDomainModule))]
public class IdentityAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // TODO
        PreConfigure<IdentityBuilder>(builder => builder.AddSignInManager<SignInManager>());
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddScoped<SignInManager>();

        var options = context.Services.ExecutePreConfiguredActions(new AbpIdentityAspNetCoreOptions());

        if (options.ConfigureAuthentication)
        {
            context.Services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddApplicationPartialCookie()
                ;
        }
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(ServiceDescriptor.Scoped<SignInManager<IdentityUser>, SignInManager>());
    }
}
