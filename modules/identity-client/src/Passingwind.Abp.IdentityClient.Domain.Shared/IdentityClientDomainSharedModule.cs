using Passingwind.Abp.IdentityClient.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.IdentityClient;

public class IdentityClientDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<IdentityClientDomainSharedModule>());

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<IdentityClientResource>("en")
                .AddVirtualJson("/Localization/IdentityClient");
        });

        Configure<AbpExceptionLocalizationOptions>(options => options.MapCodeNamespace("IdentityClient", typeof(IdentityClientResource)));
    }
}
