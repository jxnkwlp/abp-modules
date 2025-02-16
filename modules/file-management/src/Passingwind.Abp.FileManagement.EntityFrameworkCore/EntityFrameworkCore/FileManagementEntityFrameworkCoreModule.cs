using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.FileManagement.EntityFrameworkCore;

[DependsOn(
    typeof(FileManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class FileManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<FileManagementDbContext>(options =>
        {
            options.AddDefaultRepositories();
            options.Entity<FileItem>(c => c.DefaultWithDetailsFunc = d => d.IncludeAll());
            options.Entity<FileContainer>(c => c.DefaultWithDetailsFunc = d => d.Include(x => x.Accesses));
        });
    }
}
