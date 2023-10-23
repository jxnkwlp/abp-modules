using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.IdentityClient;

public interface IIdentityProviderAppService : IApplicationService
{
    Task<ListResultDto<IdentityProviderDto>> GetListAsync();
}
