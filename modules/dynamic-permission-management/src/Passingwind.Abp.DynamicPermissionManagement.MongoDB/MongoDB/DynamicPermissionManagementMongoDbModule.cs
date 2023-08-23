using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Abp.PermissionManagement.MongoDB;

namespace Passingwind.Abp.DynamicPermissionManagement.MongoDB;

[DependsOn(
    typeof(DynamicPermissionManagementDomainModule),
    typeof(AbpPermissionManagementMongoDbModule),
    typeof(AbpMongoDbModule)
    )]
public class DynamicPermissionManagementMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<DynamicPermissionManagementMongoDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, MongoQuestionRepository>();
             */
        });
    }
}
