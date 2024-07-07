using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Passingwind.Abp.PermissionManagement.MongoDB;

[DependsOn(
    typeof(PermissionManagementApplicationTestModule),
    typeof(PermissionManagementMongoDbModule)
)]
public class PermissionManagementMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
