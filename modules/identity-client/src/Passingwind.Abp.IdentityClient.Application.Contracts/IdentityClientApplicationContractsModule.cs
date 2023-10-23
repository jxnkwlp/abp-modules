using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.IdentityClient;

[DependsOn(
    typeof(IdentityClientDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class IdentityClientApplicationContractsModule : AbpModule
{
}
