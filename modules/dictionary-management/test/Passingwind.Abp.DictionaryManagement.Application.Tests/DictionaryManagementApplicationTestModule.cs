using Volo.Abp.Modularity;

namespace Passingwind.Abp.DictionaryManagement;

[DependsOn(
    typeof(DictionaryManagementApplicationModule),
    typeof(DictionaryManagementDomainTestModule)
    )]
public class DictionaryManagementApplicationTestModule : AbpModule
{
}
