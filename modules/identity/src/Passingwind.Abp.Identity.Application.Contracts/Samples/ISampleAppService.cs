using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Identity.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
