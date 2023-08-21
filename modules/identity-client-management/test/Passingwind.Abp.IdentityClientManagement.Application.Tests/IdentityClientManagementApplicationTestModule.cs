using Volo.Abp.Modularity;

namespace Passingwind.Abp.IdentityClientManagement;

[DependsOn(
    typeof(IdentityClientManagementApplicationModule),
    typeof(IdentityClientManagementDomainTestModule)
    )]
public class IdentityClientManagementApplicationTestModule : AbpModule
{

}
