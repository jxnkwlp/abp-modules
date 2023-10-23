using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.IdentityClient;

public class IdentityClientManager : DomainService
{
    private readonly IIdentityClientRepository _identityClientRepository;

    public IdentityClientManager(IIdentityClientRepository identityClientRepository)
    {
        _identityClientRepository = identityClientRepository;
    }
}
