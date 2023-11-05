using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

public interface IAccountClaimsAppService : IApplicationService
{
    Task<ListResultDto<AccountClaimResultDto>> GetListAsync();
}
