using Passingwind.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.Account;

public abstract class AccountBaseController : AbpControllerBase
{
    protected AccountBaseController()
    {
        LocalizationResource = typeof(AccountResource);
    }
}
