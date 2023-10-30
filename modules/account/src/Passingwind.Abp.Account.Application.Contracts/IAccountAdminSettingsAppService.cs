using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

public interface IAccountAdminSettingsAppService : IApplicationService
{
    Task<AccountAdminSettingsDto> GetAsync();
    Task UpdateAsync(AccountAdminSettingsDto input);

    Task<AccountGeneralSettingsDto> GetGeneralAsync();
    Task UpdateGeneralAsync(AccountGeneralSettingsDto input);

    Task<AccountCaptchaSettingsDto> GetCaptchaAsync();
    Task UpdateCaptchaAsync(AccountCaptchaSettingsDto input);

    Task<AccountRecaptchaSettingsDto> GetRecaptchaAsync();
    Task UpdateRecaptchaAsync(AccountRecaptchaSettingsDto input);

    Task<AccountExternalLoginSettingsDto> GetExternalLoginAsync();
    Task UpdateExternalLoginAsync(AccountExternalLoginSettingsDto input);
}
