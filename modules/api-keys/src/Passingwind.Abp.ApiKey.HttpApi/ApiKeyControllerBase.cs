using Passingwind.Abp.ApiKey.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.ApiKey;

public abstract class ApiKeyControllerBase : AbpControllerBase
{
    protected ApiKeyControllerBase()
    {
        LocalizationResource = typeof(ApiKeyResource);
    }
}
