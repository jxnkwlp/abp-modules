using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.IdentityClient;

public interface IIdentityClientLoginAppService : IApplicationService
{
    Task LoginAsync(string name, string? redirectUrl = null);
}
