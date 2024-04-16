using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("api/account/impersonation")]
public class AccountImpersonationController : AbpControllerBase, IAccountImpersonationAppService
{
    private readonly IAccountImpersonationAppService _service;

    public AccountImpersonationController(IAccountImpersonationAppService service)
    {
        _service = service;
    }

    /// <inheritdoc/>
    [HttpPost("{userId}/login")]
    public virtual Task LoginAsync(Guid userId)
    {
        return _service.LoginAsync(userId);
    }

    /// <inheritdoc/>
    [HttpPost("link/users/{userId}/login")]
    public virtual Task LinkLoginAsync(Guid userId)
    {
        return _service.LinkLoginAsync(userId);
    }

    /// <inheritdoc/>
    [HttpPost("logout")]
    public virtual Task LogoutAsync()
    {
        return _service.LogoutAsync();
    }

    /// <inheritdoc/>
    [HttpPost("delegations/{id}/login")]
    public virtual Task DelegationLoginAsync(Guid id)
    {
        return _service.DelegationLoginAsync(id);
    }
}
