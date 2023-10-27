using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

public interface IAccountExternalAppService : IApplicationService
{
    Task LoginAsync([NotNull] string provider, string? redirectUrl = null);

    Task<AccountExternalLoginResultDto> CallbackAsync([NotNull] AccountExternalLoginCallbackDto input);
}
