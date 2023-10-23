using Passingwind.Abp.IdentityClient.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.IdentityClient;

public abstract class IdentityClientControllerBase : AbpControllerBase
{
    protected IdentityClientControllerBase()
    {
        LocalizationResource = typeof(IdentityClientResource);
    }
}
