using System.Threading.Tasks;
using Volo.Abp.Account;

namespace Passingwind.Abp.Account;

public interface IAccountProfileAppService : IProfileAppService
{
    Task SendEmailConfirmAsync();
    Task<AccountVerifyTokenResultDto> VerifyEmailConfirmTokenAsync(AccountVerifyTokenRequestDto input);
    Task UpdateEmailConfirmAsync(AccountVerifyTokenRequestDto input);

    Task SendChangeEmailTokenAsync(AccountProfileChangeEmailTokenDto input);
    Task<AccountVerifyTokenResultDto> VerifyChangeEmailTokenAsync(AccountProfileChangeEmailDto input);
    Task ChangeEmailAsync(AccountProfileChangeEmailDto input);

    Task SendChangePhoneNumberTokenAsync(AccountProfileChangePhoneNumberTokenDto input);
    Task<AccountVerifyTokenResultDto> VerifyChangePhoneNumberTokenAsync(AccountProfileChangePhoneNumberDto input);
    Task ChangePhoneNumberAsync(AccountProfileChangePhoneNumberDto input);
}
