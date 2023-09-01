using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Identity;

public interface IIdentitySettingAppService : IApplicationService
{
    Task<IdentitySettingsDto> GetAsync();
    Task UpdateAsync(IdentitySettingsDto input);
}
