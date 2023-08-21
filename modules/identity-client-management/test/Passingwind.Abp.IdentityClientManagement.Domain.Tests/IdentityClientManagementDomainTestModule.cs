using Passingwind.Abp.IdentityClientManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.IdentityClientManagement;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(IdentityClientManagementEntityFrameworkCoreTestModule)
    )]
public class IdentityClientManagementDomainTestModule : AbpModule
{

}
