using AutoMapper;
using Passingwind.Abp.Account.Settings;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Account;

public class AccountApplicationAutoMapperProfile : Profile
{
    public AccountApplicationAutoMapperProfile()
    {
        CreateMap<IdentityUser, AccountProfileDto>()
            .ForMember(dest => dest.HasPassword,
                op => op.MapFrom(src => src.PasswordHash != null))
            .MapExtraProperties();
        CreateMap<AccountGeneralSettings, AccountGeneralSettingsDto>().ReverseMap();
        CreateMap<AccountCaptchaSettings, AccountCaptchaSettingsDto>().ReverseMap();
        CreateMap<AccountRecaptchaSettings, AccountRecaptchaSettingsDto>().ReverseMap();
        CreateMap<AccountExternalLoginSettings, AccountExternalLoginSettingsDto>().ReverseMap();
        CreateMap<IdentityUser, UserBasicDto>();
    }
}
