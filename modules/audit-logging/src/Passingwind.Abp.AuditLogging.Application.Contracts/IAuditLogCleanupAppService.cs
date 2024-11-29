using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.AuditLogging;

public interface IAuditLogCleanupAppService : IApplicationService
{
    Task RunAsync(AuditLogCleanupRequestDto input);
}
