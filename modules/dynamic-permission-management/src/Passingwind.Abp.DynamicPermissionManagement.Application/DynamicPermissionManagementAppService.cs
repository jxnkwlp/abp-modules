using Passingwind.Abp.DynamicPermissionManagement.Localization;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.DynamicPermissionManagement;

public abstract class DynamicPermissionManagementAppService : ApplicationService
{
    protected DynamicPermissionManagementAppService()
    {
        LocalizationResource = typeof(DynamicPermissionManagementResource);
        ObjectMapperContext = typeof(DynamicPermissionManagementApplicationModule);
    }
}
