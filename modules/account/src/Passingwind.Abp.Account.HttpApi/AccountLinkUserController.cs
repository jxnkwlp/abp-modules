using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Passingwind.Abp.Account;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Identity;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("api/account/link-user")]
public class AccountLinkUserController : AccountBaseController, IAccountLinkUserAppService
{
    private readonly IAccountLinkUserAppService _service;

    public AccountLinkUserController(IAccountLinkUserAppService service)
    {
        _service = service;
    }

    /// <inheritdoc/>
    [HttpGet]
    public virtual Task<ListResultDto<AccountLinkUserDto>> GetListAsync(AccountLinkUserListRequestDto input)
    {
        return _service.GetListAsync(input);
    }

    /// <inheritdoc/>
    [HttpPost("token")]
    public virtual Task<AccountLinkDto> CreateLinkTokenAsync()
    {
        return _service.CreateLinkTokenAsync();
    }

    /// <inheritdoc/>
    [HttpPost("token/verify")]
    public virtual Task<AccountLinkTokenValidationResultDto> VerifyLinkTokenAsync(AccountLinkTokenValidationRequestDto input)
    {
        return _service.VerifyLinkTokenAsync(input);
    }

    /// <inheritdoc/>
    [HttpPost("unlink")]
    public virtual Task UnlinkAsync(AccountUnlinkDto input)
    {
        return _service.UnlinkAsync(input);
    }
}
