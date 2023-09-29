using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

public interface IAccountAdminSettingsAppService : IApplicationService
{
    Task<AccountAdminSettingsDto> GetAsync();
    Task UpdateAsync(AccountAdminSettingsDto input);
}
