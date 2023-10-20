using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Account;

public interface IAccountTwoFactorTokenSender
{
    Task SendAsync(IdentityUser user, string provider, string token, CancellationToken cancellationToken = default);

    Task SendEmailConfirmationTokenAsync(IdentityUser user, string token, CancellationToken cancellationToken = default);

    Task SendChangePhoneNumberTokenAsync(IdentityUser user, string phoneNumber, string token, CancellationToken cancellationToken = default);

    Task SendChangeEmailTokenAsync(IdentityUser user, string email, string token, CancellationToken cancellationToken = default);
}
