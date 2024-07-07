using Passingwind.Abp.PermissionManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.PermissionManagement;

public abstract class PermissionManagementController : AbpControllerBase
{
    protected PermissionManagementController()
    {
        LocalizationResource = typeof(PermissionManagementResource);
    }
}
