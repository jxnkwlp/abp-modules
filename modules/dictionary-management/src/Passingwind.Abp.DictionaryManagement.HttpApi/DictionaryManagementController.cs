using Passingwind.Abp.DictionaryManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.DictionaryManagement;

public abstract class DictionaryManagementController : AbpControllerBase
{
    protected DictionaryManagementController()
    {
        LocalizationResource = typeof(DictionaryManagementResource);
    }
}
