using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
