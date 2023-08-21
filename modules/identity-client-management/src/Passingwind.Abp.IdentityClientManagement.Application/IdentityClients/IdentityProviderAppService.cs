using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

public class IdentityProviderAppService : IdentityClientManagementAppService, IIdentityProviderAppService
{
    private readonly IIdentityClientRepository _identityClientRepository;

    public IdentityProviderAppService(IIdentityClientRepository identityClientRepository)
    {
        _identityClientRepository = identityClientRepository;
    }

    [AllowAnonymous]
    public async Task<ListResultDto<IdentityProviderDto>> GetListAsync()
    {
        var list = await _identityClientRepository.GetListAsync(false);

        var providers = list
            .Where(x => x.IsEnabled)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .Select(x => new IdentityProviderDto()
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                ProviderType = x.ProviderType,
                AuthenticationUrl = $"/auth/external/{x.Name}/login",
            })
            .ToList();

        return new ListResultDto<IdentityProviderDto>(providers);
    }
}
