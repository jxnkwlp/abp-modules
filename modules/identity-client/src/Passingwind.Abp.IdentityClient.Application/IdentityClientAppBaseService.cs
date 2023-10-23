using Passingwind.Abp.IdentityClient.Localization;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.IdentityClient;

public abstract class IdentityClientAppBaseService : ApplicationService
{
    protected IdentityClientAppBaseService()
    {
        LocalizationResource = typeof(IdentityClientResource);
        ObjectMapperContext = typeof(IdentityClientApplicationModule);
    }
}
