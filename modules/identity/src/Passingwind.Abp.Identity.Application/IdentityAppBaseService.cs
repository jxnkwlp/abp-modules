using Passingwind.Abp.Identity.Localization;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Identity;

public abstract class IdentityAppBaseService : ApplicationService
{
    protected IdentityAppBaseService()
    {
        LocalizationResource = typeof(IdentityResourceV2);
        ObjectMapperContext = typeof(IdentityApplicationModule);
    }
}
