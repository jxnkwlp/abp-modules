using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Account;

public interface IAccountTwoFactoryCodeSender
{
    Task SendAsync(IdentityUser user, string provider, CancellationToken cancellationToken = default);
}
