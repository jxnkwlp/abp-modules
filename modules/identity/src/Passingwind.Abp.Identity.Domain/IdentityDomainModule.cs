using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Passingwind.Abp.Identity.Options;
using Passingwind.Abp.Identity.TokenProviders;
using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Passingwind.Abp.Identity;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpIdentityDomainModule),
    typeof(IdentityDomainSharedModule)
)]
public class IdentityDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddScoped<IdentityUserManager>();
        context.Services.TryAddScoped(typeof(UserManager<IdentityUser>), provider => provider.GetRequiredService(typeof(IdentityUserManager)));

        context.Services.AddAbpDynamicOptions<IdentityOptions, IdentityOptionsManager>();

        context.Services.AddOptions<IdentityUserTokenOptions>();

        // configure identity
        var identityBuilder = context.Services.GetObject<IdentityBuilder>();
        var userType = identityBuilder.UserType;

        var authenticatorProviderType = typeof(AuthenticatorTokenProviderV2<>).MakeGenericType(userType);
        var passlessLoginTokenProviderType = typeof(PasswordlessLoginTokenProvider<>).MakeGenericType(userType);
        var emailTokenProviderType = typeof(EmailTokenProviderV2<>).MakeGenericType(userType);
        // var phoneNumberProviderType = typeof(PhoneNumberTokenProviderV2<>).MakeGenericType(userType);

        identityBuilder
            .AddTokenProvider(TokenOptions.DefaultAuthenticatorProvider, authenticatorProviderType)
            .AddTokenProvider(TokenOptions.DefaultEmailProvider, emailTokenProviderType)
            .AddTokenProvider(TokenOptions.DefaultPhoneProvider, typeof(PhoneNumberTokenProviderV2))
            .AddTokenProvider("PasswordlessLoginTokenProvider", passlessLoginTokenProviderType);
    }
}
