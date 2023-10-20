using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.Account.Settings;

public interface IAccountSettingsManager
{
    Task<AccountGeneralSettings> GetGeneralSettingsAsync(CancellationToken cancellationToken = default);
    Task<AccountCaptchaSettings> GetCaptchaSettingsAsync(CancellationToken cancellationToken = default);
    Task<AccountRecaptchaSettings> GetRecaptchaSettingsAsync(CancellationToken cancellationToken = default);
    Task<AccountSecurityLogsSettings> GetSecurityLogsSettingsAsync(CancellationToken cancellationToken = default);

    Task SetGeneralSettingsAsync(AccountGeneralSettings settings, CancellationToken cancellationToken = default);
    Task SetCaptchaSettingsAsync(AccountCaptchaSettings settings, CancellationToken cancellationToken = default);
    Task SetRecaptchaSettingsAsync(AccountRecaptchaSettings settings, CancellationToken cancellationToken = default);
    Task SetSecurityLogsSettingsAsync(AccountSecurityLogsSettings settings, CancellationToken cancellationToken = default);
}
