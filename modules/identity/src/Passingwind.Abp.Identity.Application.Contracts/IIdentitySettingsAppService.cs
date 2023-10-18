using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Identity;

public interface IIdentitySettingsAppService : IApplicationService
{
    Task<IdentitySettingsDto> GetAsync();
    Task UpdateAsync(IdentitySettingsDto input);

    Task<IdentityUserSettingsDto> GetUserAsync();
    Task UpdateUserAsync(IdentityUserSettingsDto input);

    Task<IdentityPasswordSettingsDto> GetPasswordAsync();
    Task UpdatePasswordAsync(IdentityPasswordSettingsDto input);

    Task<IdentityLockoutSettingsDto> GetLockoutAsync();
    Task UpdateLockoutAsync(IdentityLockoutSettingsDto input);

    Task<IdentitySignInSettingsDto> GetSignInAsync();
    Task UpdateSignInAsync(IdentitySignInSettingsDto input);

    Task<IdentityTwofactorSettingsDto> GetTwofactorAsync();
    Task UpdateTwofactorAsync(IdentityTwofactorSettingsDto input);

    Task<OrganizationUnitSettingsDto> GetOrganizationUnitAsync();
    Task UpdateOrganizationUnitAsync(OrganizationUnitSettingsDto input);
}
