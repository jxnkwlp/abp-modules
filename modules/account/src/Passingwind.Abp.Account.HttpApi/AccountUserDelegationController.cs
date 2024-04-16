using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("api/account/user-delegation")]
public class AccountUserDelegationController : AbpControllerBase, IAccountUserDelegationAppService
{
    private readonly IAccountUserDelegationAppService _service;

    public AccountUserDelegationController(IAccountUserDelegationAppService service)
    {
        _service = service;
    }

    /// <inheritdoc/>
    [HttpGet("my-delegateds")]
    public virtual Task<ListResultDto<AccountUserDelegationDto>> GetMyDelegatedListAsync()
    {
        return _service.GetMyDelegatedListAsync();
    }

    /// <inheritdoc/>
    [HttpGet("delegateds")]
    public virtual Task<ListResultDto<AccountUserDelegationDto>> GetDelegatedListAsync()
    {
        return _service.GetDelegatedListAsync();
    }

    /// <inheritdoc/>
    [HttpPost]
    public virtual Task CreateAsync(AccountUserDelegationCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    /// <inheritdoc/>
    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    /// <inheritdoc/>
    [HttpGet("user-lookup")]
    public virtual Task<ListResultDto<UserBasicDto>> UserLookupAsync(string? filter = null)
    {
        return _service.UserLookupAsync(filter);
    }
}
