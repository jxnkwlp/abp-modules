using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Passingwind.Abp.FileManagement.MongoDB;

[DependsOn(
    typeof(FileManagementTestBaseModule),
    typeof(FileManagementMongoDbModule)
    )]
public class FileManagementMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
