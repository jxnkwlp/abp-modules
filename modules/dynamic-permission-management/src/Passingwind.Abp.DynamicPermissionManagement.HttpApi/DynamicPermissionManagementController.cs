using Passingwind.Abp.DynamicPermissionManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.DynamicPermissionManagement;

public abstract class DynamicPermissionManagementController : AbpControllerBase
{
    protected DynamicPermissionManagementController()
    {
        LocalizationResource = typeof(DynamicPermissionManagementResource);
    }
}
