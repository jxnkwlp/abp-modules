using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Account;

public class NullAccountTwoFactorTokenSender : IAccountTwoFactorTokenSender, ISingletonDependency
{
    protected ILogger<NullAccountTwoFactorTokenSender> Logger { get; }

    public NullAccountTwoFactorTokenSender(ILogger<NullAccountTwoFactorTokenSender> logger)
    {
        Logger = logger;
    }

    public virtual Task SendAsync(IdentityUser user, string provider, string token, CancellationToken cancellationToken = default)
    {
        Logger.NotImplement(token, typeof(IAccountTwoFactorTokenSender).FullName);

        return Task.CompletedTask;
    }

    public virtual Task SendEmailConfirmationTokenAsync(IdentityUser user, string token, CancellationToken cancellationToken = default)
    {
        Logger.NotImplement(token, typeof(IAccountTwoFactorTokenSender).FullName);

        return Task.CompletedTask;
    }

    public virtual Task SendChangePhoneNumberTokenAsync(IdentityUser user, string phoneNumber, string token, CancellationToken cancellationToken = default)
    {
        Logger.NotImplement(token, typeof(IAccountTwoFactorTokenSender).FullName);

        return Task.CompletedTask;
    }

    public virtual Task SendChangeEmailTokenAsync(IdentityUser user, string email, string token, CancellationToken cancellationToken = default)
    {
        Logger.NotImplement(token, typeof(IAccountTwoFactorTokenSender).FullName);

        return Task.CompletedTask;
    }
}

internal static partial class NullAccountTwoFactorTokenSenderLoggerMessage
{
    [LoggerMessage("Token '{Token}' not sent. Please implement '{TypeName}' first.", Level = LogLevel.Warning)]
    internal static partial void NotImplement(this ILogger logger, string token, string? typeName);
}
