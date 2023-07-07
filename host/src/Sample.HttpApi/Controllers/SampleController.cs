using Sample.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Sample.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SampleController : AbpControllerBase
{
    protected SampleController()
    {
        LocalizationResource = typeof(SampleResource);
    }
}
