using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace Passingwind.Abp.Account;

[ControllerName("AccountExternal")]
[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("api/account/external")]
public class AccountExternalController : AccountBaseController, IAccountExternalAppService
{
    protected IAccountExternalAppService ExternalAppService { get; }

    public AccountExternalController(IAccountExternalAppService externalAppService)
    {
        ExternalAppService = externalAppService;
    }

    [HttpPost("login")]
    public virtual Task LoginAsync([NotNull] string provider, string? redirectUrl = null)
    {
        return ExternalAppService.LoginAsync(provider, redirectUrl);
    }

    [HttpPost("callback")]
    public virtual Task<AccountExternalLoginResultDto> CallbackAsync([NotNull] AccountExternalLoginCallbackDto input)
    {
        return ExternalAppService.CallbackAsync(input);
    }
}
