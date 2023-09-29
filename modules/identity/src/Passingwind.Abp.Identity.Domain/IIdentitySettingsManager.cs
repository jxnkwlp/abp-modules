using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.Identity;

public interface IIdentitySettingsManager
{
    Task<IdentityUserSettings> GetUserSettingsAsync(CancellationToken cancellationToken = default);
    Task<IdentityPasswordSettings> GetPasswordSettingsAsync(CancellationToken cancellationToken = default);
    Task<IdentityLockoutSettings> GetLockoutSettingsAsync(CancellationToken cancellationToken = default);
    Task<IdentitySignInSettings> GetSignInSettingsAsync(CancellationToken cancellationToken = default);
    Task<IdentityTwofactorSettings> GetTwoFactorSettingsAsync(CancellationToken cancellationToken = default);
    Task<OrganizationUnitSettings> GetOrganizationUnitSettingsAsync(CancellationToken cancellationToken = default);

    Task SetUserSettingsAsync(IdentityUserSettings settings, CancellationToken cancellationToken = default);
    Task SetPasswordSettingsAsync(IdentityPasswordSettings settings, CancellationToken cancellationToken = default);
    Task SetLockoutSettingsAsync(IdentityLockoutSettings settings, CancellationToken cancellationToken = default);
    Task SetSignInSettingsAsync(IdentitySignInSettings settings, CancellationToken cancellationToken = default);
    Task SetTwofactorSettingsAsync(IdentityTwofactorSettings settings, CancellationToken cancellationToken = default);
    Task SetOrganizationUnitSettingsAsync(OrganizationUnitSettings settings, CancellationToken cancellationToken = default);
}
