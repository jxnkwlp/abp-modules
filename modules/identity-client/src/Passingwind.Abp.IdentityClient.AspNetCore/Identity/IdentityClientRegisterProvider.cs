using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passingwind.Abp.IdentityClient.OpenIdConnect;
using Passingwind.Abp.IdentityClient.Options;
using Passingwind.Abp.IdentityClient.Saml2;
using Passingwind.AspNetCore.Authentication.Saml2;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.IdentityClient.Identity;

public class IdentityClientRegisterProvider : IIdentityClientRegisterProvider, ITransientDependency
{
    protected ILogger<IdentityClientRegisterProvider> Logger { get; }

    protected IIdentityClientRepository IdentityClientRepository { get; }
    protected IdentityClientManager IdentityClientManager { get; }
    protected IdentityClientProviderOption IdentityClientProviderOption { get; }
    protected IdentityClientOption IdentityClientOption { get; }
    protected IAuthenticationSchemeProviderManager ExternalLoginProviderManager { get; }

    protected OpenIdConnectPostConfigureOptions OpenIdConnectPostConfigureOptions { get; }
    protected IOpenIdConnectOptionBuilder OpenIdConnectOptionBuilder { get; }
    protected Saml2PostConfigureOptions Saml2PostConfigureOptions { get; }
    protected ISaml2OptionBuilder Saml2OptionBuilder { get; }

    public IdentityClientRegisterProvider(
        ILogger<IdentityClientRegisterProvider> logger,
        IIdentityClientRepository identityClientRepository,
        IdentityClientManager identityClientManager,
        IOptions<IdentityClientProviderOption> identityClientProviderOption,
        IOptions<IdentityClientOption> identityClientOption,
        IAuthenticationSchemeProviderManager externalLoginProviderManager,
        OpenIdConnectPostConfigureOptions openIdConnectPostConfigureOptions,
        IOpenIdConnectOptionBuilder openIdConnectOptionBuilder,
        Saml2PostConfigureOptions saml2PostConfigureOptions,
        ISaml2OptionBuilder saml2OptionBuilder)
    {
        Logger = logger;
        IdentityClientRepository = identityClientRepository;
        IdentityClientManager = identityClientManager;
        IdentityClientProviderOption = identityClientProviderOption.Value;
        IdentityClientOption = identityClientOption.Value;
        ExternalLoginProviderManager = externalLoginProviderManager;
        OpenIdConnectPostConfigureOptions = openIdConnectPostConfigureOptions;
        OpenIdConnectOptionBuilder = openIdConnectOptionBuilder;
        Saml2PostConfigureOptions = saml2PostConfigureOptions;
        Saml2OptionBuilder = saml2OptionBuilder;
    }

    public virtual async Task RegisterAllAsync(CancellationToken cancellationToken = default)
    {
        if (!IdentityClientOption.ConfigureAuthenticationSchame)
            return;

        var list = await IdentityClientRepository.GetListAsync(includeDetails: true, cancellationToken: cancellationToken);

        foreach (var item in list)
        {
            await RegisterAsync(item, cancellationToken);
        }
    }

    public virtual async Task RegisterAsync(IdentityClient identityClient, CancellationToken cancellationToken = default)
    {
        if (!IdentityClientOption.ConfigureAuthenticationSchame)
            return;

        if (!identityClient.IsEnabled)
            return;

        try
        {
            if (identityClient.ProviderType == IdentityProviderType.OpenIdConnect)
            {
                await RegisterOpenIdConnectAsync(identityClient, cancellationToken);
            }
            else if (identityClient.ProviderType == IdentityProviderType.Saml2)
            {
                await RegisterSaml2Async(identityClient, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Register identity client {0} with provider {1} failed.", identityClient.Name, identityClient.ProviderName);
        }
    }

    public virtual async Task UnregisterAsync(IdentityClient identityClient, CancellationToken cancellationToken = default)
    {
        await ExternalLoginProviderManager.UnRegisterAsync<OpenIdConnectOptions>(identityClient.ProviderName, cancellationToken);
    }

    public virtual async Task ValidateAsync(IdentityClient identityClient, CancellationToken cancellationToken = default)
    {
        if (identityClient.ProviderType == IdentityProviderType.OpenIdConnect)
        {
            var options = await OpenIdConnectOptionBuilder.GetAsync(identityClient.ProviderName, IdentityClientConfigurationHelper.ToOpenIdConnectConfiguration(identityClient.Configurations), cancellationToken);

            OpenIdConnectPostConfigureOptions.PostConfigure(identityClient.ProviderName, options);

            options.Validate(identityClient.ProviderName);
        }
        else if (identityClient.ProviderType == IdentityProviderType.Saml2)
        {
            var options = await Saml2OptionBuilder.GetAsync(identityClient.ProviderName, IdentityClientConfigurationHelper.ToSaml2Configuration(identityClient.Configurations), cancellationToken);

            Saml2PostConfigureOptions.PostConfigure(identityClient.ProviderName, options);

            options.Validate(identityClient.ProviderName);
        }
        else
        {
            // 
        }
    }

    protected virtual async Task RegisterOpenIdConnectAsync(IdentityClient identityClient, CancellationToken cancellationToken = default)
    {
        var options = await OpenIdConnectOptionBuilder.GetAsync(identityClient.ProviderName, IdentityClientConfigurationHelper.ToOpenIdConnectConfiguration(identityClient.Configurations), cancellationToken);

        OpenIdConnectPostConfigureOptions.PostConfigure(identityClient.ProviderName, options);

        options.Validate(identityClient.ProviderName);

        await ExternalLoginProviderManager.RegisterAsync<OpenIdConnectOptions, OpenIdConnectHandler>(identityClient.ProviderName, identityClient.DisplayName, options, cancellationToken);
    }

    protected virtual async Task RegisterSaml2Async(IdentityClient identityClient, CancellationToken cancellationToken = default)
    {
        var options = await Saml2OptionBuilder.GetAsync(identityClient.ProviderName, IdentityClientConfigurationHelper.ToSaml2Configuration(identityClient.Configurations), cancellationToken);

        Saml2PostConfigureOptions.PostConfigure(identityClient.ProviderName, options);

        options.Validate(identityClient.ProviderName);

        await ExternalLoginProviderManager.RegisterAsync<Saml2Options, Saml2Handler>(identityClient.ProviderName, identityClient.DisplayName, options, cancellationToken);
    }
}
