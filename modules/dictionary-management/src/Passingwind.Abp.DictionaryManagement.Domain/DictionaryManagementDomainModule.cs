using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.DictionaryManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(DictionaryManagementDomainSharedModule)
)]
public class DictionaryManagementDomainModule : AbpModule
{
}
