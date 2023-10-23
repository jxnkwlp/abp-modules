using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.IdentityClient.Options;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.IdentityClient;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpIdentityDomainModule),
    typeof(IdentityClientDomainSharedModule)
)]
public class IdentityClientDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<IdentityClientDomainModule>();
        Configure<AbpAutoMapperOptions>(options => options.AddProfile<IdentityClientDomainMappingProfile>(validate: true));

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<IdentityClient, IdentityClientEto>(typeof(IdentityClientDomainModule));

            options.AutoEventSelectors.Add<IdentityClient>();
        });

        context.Services.AddOptions<IdentityClientOption>("IdentityClient");
    }
}
