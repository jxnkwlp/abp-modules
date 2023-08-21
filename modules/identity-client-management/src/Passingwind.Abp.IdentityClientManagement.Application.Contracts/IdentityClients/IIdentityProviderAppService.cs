using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

public interface IIdentityProviderAppService : IApplicationService
{
    Task<ListResultDto<IdentityProviderDto>> GetListAsync();
}
