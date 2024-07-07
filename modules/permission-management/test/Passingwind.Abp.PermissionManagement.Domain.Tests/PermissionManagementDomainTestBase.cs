using Volo.Abp.Modularity;

namespace Passingwind.Abp.PermissionManagement;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class PermissionManagementDomainTestBase<TStartupModule> : PermissionManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
