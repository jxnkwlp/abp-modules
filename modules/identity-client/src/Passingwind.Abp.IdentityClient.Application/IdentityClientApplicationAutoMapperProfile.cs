using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Passingwind.Abp.IdentityClient;

public class IdentityClientApplicationAutoMapperProfile : Profile
{
    public IdentityClientApplicationAutoMapperProfile()
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
