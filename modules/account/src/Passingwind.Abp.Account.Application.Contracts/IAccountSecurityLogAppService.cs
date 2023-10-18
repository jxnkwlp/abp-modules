using System.Threading.Tasks;
using Passingwind.Abp.Identity;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

public interface IAccountSecurityLogAppService : IApplicationService
{
    Task<PagedResultDto<IdentitySecurityLogDto>> GetListAsync(AccountSecurityLogPagedListRequestDto input);
}
