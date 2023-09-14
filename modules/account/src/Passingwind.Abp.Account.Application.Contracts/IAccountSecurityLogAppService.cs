using System.Threading.Tasks;
using Passingwind.Abp.Identity;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Account;

public interface IAccountSecurityLogAppService
{
    Task<PagedResultDto<IdentitySecurityLogDto>> GetListAsync(IdentitySecurityLogPagedListRequestDto input);
}
