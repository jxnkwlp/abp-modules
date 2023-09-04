using System.Linq;
using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

public class IdentityApplicationAutoMapperProfile : Profile
{
    public IdentityApplicationAutoMapperProfile()
    {
        CreateMap<IdentityUser, IdentityUserV2Dto>();

        CreateMap<IdentityClaimType, IdentityClaimTypeDto>()
            .Ignore(x => x.ValueTypeAsString);

        CreateMap<IdentityClaim, IdentityClaimDto>();

        CreateMap<IdentityUserClaim, IdentityUserClaimDto>();
        CreateMap<IdentityRoleClaim, IdentityRoleClaimDto>();

        CreateMap<OrganizationUnit, OrganizationUnitDto>()
            .ForMember(x => x.RoleIds, x => x.MapFrom(r => r.Roles.Select(s => s.RoleId)));

        CreateMap<IdentitySecurityLog, IdentitySecurityLogDto>();

        CreateMap<IdentityUserSettings, IdentityUserSettingsDto>().ReverseMap();
        CreateMap<IdentityPasswordSettings, IdentityPasswordSettingsDto>().ReverseMap();
        CreateMap<IdentityLockoutSettings, IdentityLockoutSettingsDto>().ReverseMap();
        CreateMap<IdentitySignInSettings, IdentitySignInSettingsDto>().ReverseMap();
        CreateMap<IdentityTwofactorSettings, IdentityTwofactorSettingsDto>().ReverseMap();
        CreateMap<OrganizationUnitSettings, OrganizationUnitSettingsDto>().ReverseMap();
    }
}
