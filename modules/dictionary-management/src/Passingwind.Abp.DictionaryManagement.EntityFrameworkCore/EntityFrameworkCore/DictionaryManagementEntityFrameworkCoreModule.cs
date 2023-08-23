using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.DictionaryManagement.EntityFrameworkCore;

[DependsOn(
    typeof(DictionaryManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class DictionaryManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<DictionaryManagementDbContext>();
    }
}
