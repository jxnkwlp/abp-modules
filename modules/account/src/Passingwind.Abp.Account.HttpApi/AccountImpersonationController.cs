using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("api/account/impersonations")]
public class AccountImpersonationController : AccountBaseController, IAccountImpersonationAppService
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
    [HttpPost("{userId}/link-login")]
    public Task LoginLoginAsync(Guid userId)
    {
        return _service.LoginLoginAsync(userId);
    }
}
