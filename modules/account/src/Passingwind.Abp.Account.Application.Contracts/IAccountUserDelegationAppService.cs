using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

public interface IAccountUserDelegationAppService : IApplicationService
{
    Task<ListResultDto<AccountUserDelegationDto>> GetMyDelegationListAsync();

    Task<ListResultDto<AccountUserDelegationDto>> GetDelegatedListAsync();

    Task CreateAsync(AccountUserDelegationCreateDto input);

    Task DeleteAsync(Guid id);

    Task<ListResultDto<UserBasicDto>> UserLookupAsync(string? filter = null);
}
