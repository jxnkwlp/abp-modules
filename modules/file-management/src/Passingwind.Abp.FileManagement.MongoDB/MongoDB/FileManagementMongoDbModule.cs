using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.FileManagement.MongoDB;

[DependsOn(
    typeof(FileManagementDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class FileManagementMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<FileManagementMongoDbContext>(options => options.AddDefaultRepositories());
    }
}
