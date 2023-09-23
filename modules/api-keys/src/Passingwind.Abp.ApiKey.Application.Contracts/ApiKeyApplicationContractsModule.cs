using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.ApiKey;

[DependsOn(
    typeof(ApiKeyDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class ApiKeyApplicationContractsModule : AbpModule
{
}
