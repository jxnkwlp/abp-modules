using AutoMapper;
using Passingwind.Abp.Account.Settings;

namespace Passingwind.Abp.Account;

public class AccountApplicationAutoMapperProfile : Profile
{
    public AccountApplicationAutoMapperProfile()
    {
        CreateMap<AccountGeneralSettings, AccountGeneralSettingsDto>().ReverseMap();
        CreateMap<AccountCaptchaSettings, AccountCaptchaSettingsDto>().ReverseMap();
        CreateMap<AccountRecaptchaSettings, AccountRecaptchaSettingsDto>().ReverseMap();
    }
}
