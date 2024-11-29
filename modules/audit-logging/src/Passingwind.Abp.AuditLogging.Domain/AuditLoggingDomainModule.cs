using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.AuditLogging.Cleanup;
using Passingwind.Abp.AuditLogging.Options;
using Volo.Abp;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Passingwind.Abp.AuditLogging;

[DependsOn(
    typeof(AbpAuditLoggingDomainModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpBackgroundWorkersModule),
    typeof(AuditLoggingDomainSharedModule)
)]
public class AuditLoggingDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        context.Services.AddOptions<AuditLoggingCleanupOptions>("AuditLogging");
    }

    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.AddBackgroundWorkerAsync<AuditLogCleanupBackgroundWorker>();
    }
}
