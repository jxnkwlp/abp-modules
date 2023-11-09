using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.IdentityClient.Identity;

public class AuthenticationSchemeProviderManager : IAuthenticationSchemeProviderManager, ITransientDependency
{
    protected ILogger<AuthenticationSchemeProviderManager> Logger { get; }
    protected IAbpLazyServiceProvider LazyServiceProvider { get; }
    protected IAuthenticationSchemeProvider AuthenticationSchemeProvider { get; }

    public AuthenticationSchemeProviderManager(
        ILogger<AuthenticationSchemeProviderManager> logger,
        IAbpLazyServiceProvider lazyServiceProvider,
        IAuthenticationSchemeProvider authenticationSchemeProvider)
    {
        Logger = logger;
        LazyServiceProvider = lazyServiceProvider;
        AuthenticationSchemeProvider = authenticationSchemeProvider;
    }

    public virtual Task RegisterAsync<TOptions, THandler>(string name, string displayName, TOptions options, CancellationToken cancellationToken = default) where TOptions : class
    {
        Logger.LogDebug("Try register authentication scheme '{0}', option type '{1}'", name, options.GetType().Name);

        IOptionsMonitorCache<TOptions> optionCache = LazyServiceProvider.LazyGetRequiredService<IOptionsMonitorCache<TOptions>>();

        // remove
        AuthenticationSchemeProvider.RemoveScheme(name);
        optionCache.TryRemove(name);

        // add 
        optionCache.TryAdd(name, options);
        AuthenticationScheme scheme = new AuthenticationScheme(name, displayName, typeof(THandler));
        bool result = AuthenticationSchemeProvider.TryAddScheme(scheme);

        if (!result)
        {
            throw new System.Exception($"Add authentication scheme of '{scheme}' failed.");
        }

        Logger.LogInformation("Registered authentication scheme successfully. provider '{0}', option type '{1}'", name, options.GetType().Name);

        return Task.CompletedTask;
    }

    public virtual Task UnRegisterAsync<TOptions>(string name, CancellationToken cancellationToken = default) where TOptions : class
    {
        Logger.LogInformation("Try unregister authentication scheme '{0}' ", name);

        IOptionsMonitorCache<TOptions> optionCache = LazyServiceProvider.LazyGetRequiredService<IOptionsMonitorCache<TOptions>>();

        // remove
        AuthenticationSchemeProvider.RemoveScheme(name);
        optionCache.TryRemove(name);

        return Task.CompletedTask;
    }
}
