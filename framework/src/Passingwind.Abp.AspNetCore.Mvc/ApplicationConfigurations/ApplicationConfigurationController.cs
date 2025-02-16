using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

namespace Passingwind.Abp.AspNetCore.Mvc.ApplicationConfigurations;

[Area("Application")]
[Route("api/application/configuration")]
public class ApplicationConfigurationController : IApplicationConfigurationAppService
{
    protected IApplicationConfigurationAppService Service { get; }
    protected readonly IAbpAntiForgeryManager AntiForgeryManager;

    public ApplicationConfigurationController(IApplicationConfigurationAppService service, IAbpAntiForgeryManager antiForgeryManager)
    {
        Service = service;
        AntiForgeryManager = antiForgeryManager;
    }

    [HttpGet]
    public virtual Task<ApplicationConfigurationDto> GetAsync(ApplicationConfigurationRequestOptions options)
    {
        AntiForgeryManager.SetCookie();

        return Service.GetAsync(options);
    }
}
