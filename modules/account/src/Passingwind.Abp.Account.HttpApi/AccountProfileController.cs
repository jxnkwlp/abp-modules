using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("/api/account/my-profile")]
public class AccountProfileController : AbpControllerBase, IAccountProfileAppService
{
    protected IAccountProfileAppService ProfileAppService { get; }

    public AccountProfileController(IAccountProfileAppService profileAppService)
    {
        ProfileAppService = profileAppService;
    }

    [HttpGet]
    public virtual Task<ProfileDto> GetAsync()
    {
        return ProfileAppService.GetAsync();
    }

    [HttpPut]
    public virtual Task<ProfileDto> UpdateAsync(UpdateProfileDto input)
    {
        return ProfileAppService.UpdateAsync(input);
    }

    [HttpPost]
    [Route("change-password")]
    public virtual Task ChangePasswordAsync(ChangePasswordInput input)
    {
        return ProfileAppService.ChangePasswordAsync(input);
    }

    [HttpPost("email-confirm/token")]
    public virtual Task SendEmailConfirmAsync()
    {
        return ProfileAppService.SendEmailConfirmAsync();
    }

    [HttpPost("email-confirm/verify")]
    public virtual Task<AccountVerifyTokenResultDto> VerifyEmailConfirmTokenAsync(AccountVerifyTokenRequestDto input)
    {
        return ProfileAppService.VerifyEmailConfirmTokenAsync(input);
    }

    [HttpPut("email-confirm")]
    public virtual Task UpdateEmailConfirmAsync(AccountVerifyTokenRequestDto input)
    {
        return ProfileAppService.UpdateEmailConfirmAsync(input);
    }

    [HttpPost("change-phone-number/token")]
    public virtual Task SendChangePhoneNumberTokenAsync(AccountProfileChangePhoneNumberTokenDto input)
    {
        return ProfileAppService.SendChangePhoneNumberTokenAsync(input);
    }

    [HttpPost("change-phone-number/verify")]
    public virtual Task<AccountVerifyTokenResultDto> VerifyChangePhoneNumberTokenAsync(AccountProfileChangePhoneNumberDto input)
    {
        return ProfileAppService.VerifyChangePhoneNumberTokenAsync(input);
    }

    [HttpPut("change-phone-number")]
    public virtual Task ChangePhoneNumberAsync(AccountProfileChangePhoneNumberDto input)
    {
        return ProfileAppService.ChangePhoneNumberAsync(input);
    }

    [HttpPost("change-email/token")]
    public virtual Task SendChangeEmailTokenAsync(AccountProfileChangeEmailTokenDto input)
    {
        return ProfileAppService.SendChangeEmailTokenAsync(input);
    }

    [HttpPost("change-email/verify")]
    public virtual Task<AccountVerifyTokenResultDto> VerifyChangeEmailTokenAsync(AccountProfileChangeEmailDto input)
    {
        return ProfileAppService.VerifyChangeEmailTokenAsync(input);
    }

    [HttpPut("change-email")]
    public virtual Task ChangeEmailAsync(AccountProfileChangeEmailDto input)
    {
        return ProfileAppService.ChangeEmailAsync(input);
    }
}
