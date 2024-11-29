using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.AuditLogging;

public interface IAuditLogSettingAppService : IApplicationService
{
    Task<AuditLogCleanupSettingsDto> GetCleanupAsync();
    Task UpdateCleanupAsync(AuditLogCleanupSettingsDto input);
}
