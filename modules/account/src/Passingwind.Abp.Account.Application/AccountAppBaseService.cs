using Passingwind.Abp.Account.Localization;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

public abstract class AccountAppBaseService : ApplicationService
{
    protected AccountAppBaseService()
    {
        LocalizationResource = typeof(AccountResource);
        ObjectMapperContext = typeof(AccountApplicationModule);
    }
}
