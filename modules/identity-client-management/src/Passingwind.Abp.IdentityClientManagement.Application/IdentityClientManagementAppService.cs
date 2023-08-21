using Passingwind.Abp.IdentityClientManagement.Localization;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.IdentityClientManagement;

public abstract class IdentityClientManagementAppService : ApplicationService
{
    protected IdentityClientManagementAppService()
    {
        LocalizationResource = typeof(IdentityClientManagementResource);
        ObjectMapperContext = typeof(IdentityClientManagementApplicationModule);
    }
}
