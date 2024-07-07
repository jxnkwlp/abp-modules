using Volo.Abp.Modularity;

namespace Passingwind.Abp.PermissionManagement;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class PermissionManagementApplicationTestBase<TStartupModule> : PermissionManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
