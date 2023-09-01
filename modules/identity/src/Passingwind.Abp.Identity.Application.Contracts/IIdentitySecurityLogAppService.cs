using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Identity;

public interface IIdentitySecurityLogAppService : IApplicationService
{
    Task<IdentitySecurityLogDto> GetAsync(Guid id);

    Task<PagedResultDto<IdentitySecurityLogDto>> GetListAsync(IdentitySecurityLogPagedListRequestDto input);

    Task<PagedResultDto<IdentitySecurityLogDto>> GetListByCurrentUserAsync(IdentitySecurityLogPagedListRequestDto input);

    Task DeleteAsync(Guid id);
}
