using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Identity;

public interface IIdentitySecurityLogAppService : IApplicationService
{
    Task<PagedResultDto<IdentitySecurityLogsDto>> GetListAsync(IdentitySecurityLogPagedListRequestDto input);

    Task<IdentitySecurityLogsDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);
}
