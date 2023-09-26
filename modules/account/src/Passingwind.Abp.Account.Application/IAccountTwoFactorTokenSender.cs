using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Account;

public interface IAccountTwoFactorTokenSender
{
    Task SendAsync(IdentityUser user, string provider, string token, CancellationToken cancellationToken = default);
}
