using Passingwind.Abp.DictionaryManagement.Localization;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.DictionaryManagement;

public abstract class DictionaryManagementAppService : ApplicationService
{
    protected DictionaryManagementAppService()
    {
        LocalizationResource = typeof(DictionaryManagementResource);
        ObjectMapperContext = typeof(DictionaryManagementApplicationModule);
    }
}
