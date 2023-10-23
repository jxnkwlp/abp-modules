using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.IdentityClient.MongoDB;

[DependsOn(
    typeof(IdentityClientDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class IdentityClientMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<IdentityClientMongoDbContext>(options => options.AddDefaultRepositories());
    }
}
