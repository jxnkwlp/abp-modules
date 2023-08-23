using Passingwind.Abp.DictionaryManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.DictionaryManagement;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(DictionaryManagementEntityFrameworkCoreTestModule)
    )]
public class DictionaryManagementDomainTestModule : AbpModule
{
}
