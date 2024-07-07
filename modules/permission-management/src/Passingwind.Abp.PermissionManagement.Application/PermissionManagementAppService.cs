using Passingwind.Abp.PermissionManagement.Localization;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.PermissionManagement;

public abstract class PermissionManagementAppService : ApplicationService
{
    protected PermissionManagementAppService()
    {
        LocalizationResource = typeof(PermissionManagementResource);
        ObjectMapperContext = typeof(PermissionManagementApplicationModule);
    }
}
