using Passingwind.Abp.FileManagement.Localization;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.FileManagement;

public abstract class FileManagementAppService : ApplicationService
{
    protected FileManagementAppService()
    {
        LocalizationResource = typeof(FileManagementResource);
        ObjectMapperContext = typeof(PassingwindAbpFileManagementApplicationModule);
    }
}
