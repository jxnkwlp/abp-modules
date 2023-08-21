using AutoMapper;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;

namespace Passingwind.Abp.IdentityClientManagement;

public class IdentityClientManagementDomainMappingProfile : Profile
{
    public IdentityClientManagementDomainMappingProfile()
    {
        CreateMap<IdentityClient, IdentityClientEto>();
    }
}
