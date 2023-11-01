using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.Identity.MongoDB.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.Identity.MongoDB;

[DependsOn(
    typeof(IdentityDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class IdentityMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<IdentityMongoDbContextV2>(options => options.AddRepository<IdentityUserTwoFactor, IdentityUserTwoFactorRepository>());
    }
}
