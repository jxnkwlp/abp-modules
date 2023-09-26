using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Account;

public class NullAccountTwoFactoryCodeSender : IAccountTwoFactoryCodeSender, ISingletonDependency
{
    protected ILogger<NullAccountTwoFactoryCodeSender> Logger { get; }

    public NullAccountTwoFactoryCodeSender(ILogger<NullAccountTwoFactoryCodeSender> logger)
    {
        Logger = logger;
    }

    public virtual Task SendAsync(IdentityUser user, string provider, CancellationToken cancellationToken = default)
    {
        Logger.LogWarning("Two-factor code not sent. Please implement '{0}' first.", typeof(IAccountTwoFactoryCodeSender).FullName);
        return Task.CompletedTask;
    }
}
