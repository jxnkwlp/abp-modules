using Passingwind.Abp.IdentityClientManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.IdentityClientManagement;

public abstract class IdentityClientManagementController : AbpControllerBase
{
    protected IdentityClientManagementController()
    {
        LocalizationResource = typeof(IdentityClientManagementResource);
    }
}
