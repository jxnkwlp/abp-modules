using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

public class IdentityClientManager : DomainService
{
    private readonly IIdentityClientRepository _identityClientRepository;

    public IdentityClientManager(IIdentityClientRepository identityClientRepository)
    {
        _identityClientRepository = identityClientRepository;
    }
}
