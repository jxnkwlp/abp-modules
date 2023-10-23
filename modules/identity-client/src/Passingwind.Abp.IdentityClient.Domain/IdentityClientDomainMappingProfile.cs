using AutoMapper;

namespace Passingwind.Abp.IdentityClient;

public class IdentityClientDomainMappingProfile : Profile
{
    public IdentityClientDomainMappingProfile()
    {
        CreateMap<IdentityClient, IdentityClientEto>();
    }
}
