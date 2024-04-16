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
    /// <param name="input"></param>
    Task CreateAsync(AccountUserDelegationCreateDto input);

    /// <summary>
    ///  Delete delegation by id
    /// </summary>
    /// <param name="id"></param>
    Task DeleteAsync(Guid id);

    /// <summary>
    ///  Search user
    /// </summary>
    /// <param name="filter"></param>
    Task<ListResultDto<UserBasicDto>> UserLookupAsync(string? filter = null);
}
