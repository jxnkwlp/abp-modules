using System;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Passingwind.Authentication.Saml2.Configuration;

namespace Passingwind.Authentication.Saml2;

public class Saml2PostConfigureOptions : IPostConfigureOptions<Saml2Options>
{
    private readonly IDataProtectionProvider _dp;

    public Saml2PostConfigureOptions(IDataProtectionProvider dp)
    {
        _dp = dp;
    }

    public void PostConfigure(string name, Saml2Options options)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (string.IsNullOrEmpty(options.SignOutScheme))
        {
            options.SignOutScheme = options.SignInScheme!;
        }

        options.DataProtectionProvider ??= _dp;

        if (options.Backchannel == null)
        {
            options.Backchannel = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
            options.Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("Saml2 handler");
            options.Backchannel.Timeout = options.BackchannelTimeout;
            options.Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB 
        }

        if (options.StateDataFormat == null)
        {
            var dataProtector = options.DataProtectionProvider.CreateProtector(typeof(Saml2Handler).FullName!, name, "v1");
            options.StateDataFormat = new PropertiesDataFormat(dataProtector);
        }

        if (options.ConfigurationManager == null)
        {
            if (options.Configuration != null)
            {
                options.ConfigurationManager = new StaticConfigurationManager(options.Configuration);
            }
            else if (options.IdpMetadataUri != null)
            {
                options.ConfigurationManager = new ConfigurationManager(options, options.IdpMetadataUri, options.Backchannel);
            }
        }
    }
}
