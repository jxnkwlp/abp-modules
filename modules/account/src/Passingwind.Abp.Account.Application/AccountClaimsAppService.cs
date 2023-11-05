using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.Identity;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Passingwind.Abp.Account;

[Authorize]
public class AccountClaimsAppService : AccountAppBaseService, IAccountClaimsAppService
{
    protected IdentityUserManager UserManager { get; }

    public AccountClaimsAppService(IdentityUserManager userManager)
    {
        UserManager = userManager;
    }

    public virtual async Task<ListResultDto<AccountClaimResultDto>> GetListAsync()
    {
        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

        var list = await UserManager.GetClaimsAsync(user);

        return new ListResultDto<AccountClaimResultDto>(list.Select(x => new AccountClaimResultDto()
        {
            ClaimType = x.Type,
            ClaimValue = x.Value,
        }).ToList());
    }
}
