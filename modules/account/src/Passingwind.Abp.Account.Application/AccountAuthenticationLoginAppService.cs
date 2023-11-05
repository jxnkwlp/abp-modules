using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.Identity;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Passingwind.Abp.Account;

[Authorize]
public class AccountAuthenticationLoginAppService : AccountAppBaseService, IAccountAuthenticationLoginAppService
{
    protected IdentityUserManager UserManager { get; }

    public AccountAuthenticationLoginAppService(IdentityUserManager userManager)
    {
        UserManager = userManager;
    }

    public virtual async Task<ListResultDto<AccountAuthenticationLoginResultDto>> GetListAsync()
    {
        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

        var list = await UserManager.GetLoginsAsync(user);

        return new ListResultDto<AccountAuthenticationLoginResultDto>(list.Select(x => new AccountAuthenticationLoginResultDto()
        {
            LoginProvider = x.LoginProvider,
            ProviderDisplayName = x.ProviderDisplayName,
        }).ToList());
    }
}
