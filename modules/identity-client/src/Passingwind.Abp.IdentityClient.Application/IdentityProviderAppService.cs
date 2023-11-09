using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.IdentityClient;

public class IdentityProviderAppService : IdentityClientAppBaseService, IIdentityProviderAppService
{
    protected IIdentityClientRepository IdentityClientRepository { get; }
    protected IIdentityProviderAuthenticationUrl IdentityProviderAuthenticationUrl { get; }

    public IdentityProviderAppService(
        IIdentityClientRepository identityClientRepository,
        IIdentityProviderAuthenticationUrl authenticationUrl)
    {
        IdentityClientRepository = identityClientRepository;
        IdentityProviderAuthenticationUrl = authenticationUrl;
    }

    [AllowAnonymous]
    public virtual async Task<ListResultDto<IdentityProviderDto>> GetListAsync()
    {
        var list = await IdentityClientRepository.GetListAsync(false);

        var providers = list
            .Where(x => x.IsEnabled)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .Select(x =>
            {
                return new IdentityProviderDto()
                {
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    ProviderType = x.ProviderType,
                    AuthenticationUrl = IdentityProviderAuthenticationUrl.Get(x),
                };
            })
            .ToList();

        return new ListResultDto<IdentityProviderDto>(providers);
    }
}
