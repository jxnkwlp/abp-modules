using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.IdentityClientManagement.EntityFrameworkCore;

[DependsOn(
    typeof(IdentityClientManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class IdentityClientManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<IdentityClientManagementDbContext>(options => options.Entity<IdentityClient>(config => config.DefaultWithDetailsFunc = x => x.Include(i => i.ClaimMaps).Include(i => i.Configurations)));
    }
}
