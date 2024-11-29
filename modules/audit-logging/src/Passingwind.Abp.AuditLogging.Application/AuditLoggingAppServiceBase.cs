using Passingwind.Abp.AuditLogging.Localization;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.AuditLogging;

public abstract class AuditLoggingAppServiceBase : ApplicationService
{
    protected AuditLoggingAppServiceBase()
    {
        LocalizationResource = typeof(AuditLoggingResource);
        ObjectMapperContext = typeof(AuditLoggingApplicationModule);
    }
}
