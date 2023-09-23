using Passingwind.Abp.ApiKey.Localization;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.ApiKey;

public abstract class ApiKeyAppService : ApplicationService
{
    protected ApiKeyAppService()
    {
        LocalizationResource = typeof(ApiKeyResource);
        ObjectMapperContext = typeof(ApiKeyApplicationModule);
    }
}
