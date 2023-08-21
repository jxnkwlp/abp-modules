using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.IdentityClientManagement.MongoDB;

[DependsOn(
    typeof(IdentityClientManagementDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class IdentityClientManagementMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<IdentityClientManagementMongoDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, MongoQuestionRepository>();
             */
        });
    }
}
