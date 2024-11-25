using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

public interface IAccountUserDelegationAppService : IApplicationService
{
    /// <summary>
    ///  Get delegated users
    /// </summary>
    Task<ListResultDto<AccountUserDelegationDto>> GetMyDelegatedListAsync();

    /// <summary>
    ///  Get my delegated users
    /// </summary>
    Task<ListResultDto<AccountUserDelegationDto>> GetDelegatedListAsync();

    /// <summary>
    ///  Create new delegation
    /// </summary>
    Task CreateAsync(AccountUserDelegationCreateDto input);

    /// <summary>
    ///  Delete delegation by id
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    ///  Search user
    /// </summary>
    Task<ListResultDto<UserBasicDto>> UserLookupAsync(string? filter = null);
}
