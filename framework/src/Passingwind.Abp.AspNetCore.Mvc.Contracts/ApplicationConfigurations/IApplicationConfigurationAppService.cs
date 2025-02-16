using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

namespace Passingwind.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public interface IApplicationConfigurationAppService : IApplicationService
{
    Task<ApplicationConfigurationDto> GetAsync(ApplicationConfigurationRequestOptions options);
}
