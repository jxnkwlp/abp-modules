using AutoMapper;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;
using Volo.Abp.AutoMapper;

namespace Passingwind.Abp.IdentityClientManagement;

public class IdentityClientManagementApplicationAutoMapperProfile : Profile
{
    public IdentityClientManagementApplicationAutoMapperProfile()
    {
        CreateMap<IdentityClient, IdentityClientDto>()
            .Ignore(x => x.OpenIdConnectConfiguration)
            .Ignore(x => x.Saml2Configuration)
            ;
        CreateMap<IdentityClientClaimMap, IdentityClientClaimMapDto>();
        CreateMap<IdentityClientConfiguration, IdentityClientConfigurationDto>();

        CreateMap<IdentityClientOpenIdConnectConfiguration, IdentityClientOpenIdConnectConfigurationDto>().ReverseMap();
        CreateMap<IdentityClientSaml2Configuration, IdentityClientSaml2ConfigurationDto>().ReverseMap();
    }
}
