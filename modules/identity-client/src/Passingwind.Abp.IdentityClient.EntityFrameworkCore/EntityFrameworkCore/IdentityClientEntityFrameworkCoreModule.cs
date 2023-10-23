using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.IdentityClient.EntityFrameworkCore;

[DependsOn(
    typeof(IdentityClientDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class IdentityClientEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<IdentityClientDbContext>(options =>
        {
            options.AddDefaultRepositories();
            options.Entity<IdentityClient>(config => config.DefaultWithDetailsFunc = x => x.Include(i => i.ClaimMaps).Include(i => i.Configurations));
        });
    }
}
