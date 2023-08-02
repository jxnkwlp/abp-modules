using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.FileManagement;

[DependsOn(
    typeof(PassingwindAbpFileManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class PassingwindAbpFileManagementApplicationContractsModule : AbpModule
{
}
