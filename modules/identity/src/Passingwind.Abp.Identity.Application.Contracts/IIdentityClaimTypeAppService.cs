using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Identity;

public interface IIdentityClaimTypeAppService : IApplicationService
{
    Task<ListResultDto<IdentityClaimTypeDto>> GetAllListAsync();
    Task<PagedResultDto<IdentityClaimTypeDto>> GetListAsync(IdentityClaimTypePagedListRequestDto input);
    Task<IdentityClaimTypeDto> CreateAsync(IdentityClaimTypeCreateDto input);
    Task<IdentityClaimTypeDto> UpdateAsync(Guid id, IdentityClaimTypeUpdateDto input);
    Task DeleteAsync(Guid id);
}
