using System.Threading.Tasks;
using Passingwind.Abp.DynamicPermissionManagement.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.DynamicPermissionManagement.Data;

public class PermissionDefinitionDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    protected DynamicPermissionManager DynamicPermissionManager { get; }

    public PermissionDefinitionDataSeedContributor(DynamicPermissionManager dynamicPermissionManager)
    {
        DynamicPermissionManager = dynamicPermissionManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await DynamicPermissionManager.InitialGroupDefinitionsToPermissionAsync();
        await DynamicPermissionManager.InitialDefinitionsToPermissionAsync();
        await DynamicPermissionManager.ClearPermissionDefinitionCacheAsync();
    }
}
