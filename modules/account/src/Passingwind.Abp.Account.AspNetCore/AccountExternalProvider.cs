using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Passingwind.Abp.Account;

public class AccountExternalProvider : IAccountExternalProvider, ITransientDependency
{
    protected ILogger<AccountExternalProvider> Logger { get; }
    protected IOptions<AccountExternalLoginOptions> AccountExternalOptions { get; }

    public AccountExternalProvider(IOptions<AccountExternalLoginOptions> accountExternalOptions, ILogger<AccountExternalProvider> logger)
    {
        AccountExternalOptions = accountExternalOptions;
        Logger = logger;
    }

    public virtual Task LoginInfoReceivedAsync(AccountExternalCallbackLoginInfoContext context)
    {
        return Task.CompletedTask;
    }

    public virtual Task SignInAsync(AccountExternalCallbackSignInContext context)
    {
        var options = AccountExternalOptions.Value;

        if (options.RedirectToErrorPage
            && !string.IsNullOrWhiteSpace(options.ErrorPageUrl)
            && !context.SignInResult.Succeeded
            && context.SignInResult != SignInResult.Failed)
        {
            context.Handled = true;
            var errorDescription = $"login_{context.SignInResult}".ToLowerInvariant();
            context.Result = new RedirectResult($"{options.ErrorPageUrl}?error_description={errorDescription}");
        }

        return Task.CompletedTask;
    }

    public virtual Task UserSignInAsync(AccountExternalCallbackUserSignInContext context)
    {
        return Task.CompletedTask;
    }
}
