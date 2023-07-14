using Passingwind.Abp.FileManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.FileManagement;

public abstract class FileManagementController : AbpControllerBase
{
    protected FileManagementController()
    {
        LocalizationResource = typeof(FileManagementResource);
    }
}
