using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

public interface IAccountAdminSettingAppService : IApplicationService
{
    Task<AccountAdminSettingsDto> GetAsync();
    Task UpdateAsync(AccountAdminSettingsDto input);
}
