using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.AuditLogging;

public interface IAuditLogAppService : IApplicationService
{
    Task<PagedResultDto<AuditLogDto>> GetListAsync(AuditLogListRequestDto input);

    Task<AuditLogDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);
}
