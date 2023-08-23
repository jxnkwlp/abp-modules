using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.DictionaryManagement;

[DependsOn(
    typeof(DictionaryManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class DictionaryManagementApplicationContractsModule : AbpModule
{
}
