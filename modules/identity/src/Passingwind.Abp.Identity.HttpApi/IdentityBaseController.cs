using Passingwind.Abp.Identity.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.Identity;

public abstract class IdentityBaseController : AbpControllerBase
{
    protected IdentityBaseController()
    {
        LocalizationResource = typeof(IdentityResourceV2);
    }
}
