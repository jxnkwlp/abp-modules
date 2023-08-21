using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;
using Passingwind.Abp.IdentityClientManagement.Options;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.IdentityClientManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpIdentityDomainModule),
    typeof(IdentityClientManagementDomainSharedModule)
)]
public class IdentityClientManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<IdentityClientManagementDomainModule>();
        Configure<AbpAutoMapperOptions>(options => options.AddProfile<IdentityClientManagementDomainMappingProfile>(validate: true));

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<IdentityClient, IdentityClientEto>(typeof(IdentityClientManagementDomainModule));

            options.AutoEventSelectors.Add<IdentityClient>();
        });

        context.Services.AddOptions<IdentityClientOption>("IdentityClient");
    }
}
