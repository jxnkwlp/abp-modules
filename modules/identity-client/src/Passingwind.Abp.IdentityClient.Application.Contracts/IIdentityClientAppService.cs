using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.IdentityClient;

public interface IIdentityClientAppService : IApplicationService
{
    Task<PagedResultDto<IdentityClientDto>> GetListAsync(IdentityClientListRequestDto input);

    Task<IdentityClientDto> GetAsync(Guid id);

    Task<IdentityClientDto> CreateAsync(IdentityClientCreateDto input);

    Task<IdentityClientDto> UpdateAsync(Guid id, IdentityClientUpdateDto input);

    Task DeleteAsync(Guid id);

    Task VerifyAsync(Guid id);
}
