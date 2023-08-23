using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.DictionaryManagement.MongoDB;

[DependsOn(
    typeof(DictionaryManagementTestBaseModule),
    typeof(DictionaryManagementMongoDbModule)
    )]
public class DictionaryManagementMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var stringArray = MongoDbFixture.ConnectionString.Split('?');
        var connectionString = stringArray[0].EnsureEndsWith('/') +
                                   "Db_" +
                               Guid.NewGuid().ToString("N") + "/?" + stringArray[1];

        Configure<AbpDbConnectionOptions>(options => options.ConnectionStrings.Default = connectionString);
    }
}
