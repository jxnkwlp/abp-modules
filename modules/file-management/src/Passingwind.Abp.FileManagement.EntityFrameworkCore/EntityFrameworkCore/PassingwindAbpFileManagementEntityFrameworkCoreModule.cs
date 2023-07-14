using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.FileManagement.EntityFrameworkCore;

[DependsOn(
    typeof(PassingwindAbpFileManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class PassingwindAbpFileManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<FileManagementDbContext>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
        });
    }
}
