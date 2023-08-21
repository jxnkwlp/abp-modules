using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Passingwind.Authentication.Saml2;

public static class Saml2Extensions
{
    public static AuthenticationBuilder AddSaml2(this AuthenticationBuilder builder, Action<Saml2Options>? configureOptions = null)
    {
        return AddSaml2(builder, Saml2Defaults.AuthenticationScheme, Saml2Defaults.AuthenticationScheme, configureOptions);
    }

    public static AuthenticationBuilder AddSaml2(this AuthenticationBuilder builder, string scheme, string displayName, Action<Saml2Options>? configureOptions = null)
    {
        //builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<Saml2Options>, PostConfigureSaml2Options>());

        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<Saml2Options>, Saml2PostConfigureOptions>());

        builder.AddScheme<Saml2Options, Saml2Handler>(scheme, displayName, configureOptions);

        //builder.AddRemoteScheme<AuthenticationOptions, Saml2Handler>(scheme, displayName, configureOptions);

        //builder.Services.Configure<AuthenticationOptions>(o =>
        //{
        //    o.AddScheme(scheme, s =>
        //    {
        //        s.HandlerType = typeof(Saml2Handler);
        //        s.DisplayName = displayName;
        //    });
        //});

        //if (configureOptions != null)
        //{
        //    builder.Services.Configure(scheme, configureOptions);
        //}

        builder.Services.AddTransient<Saml2Handler>();

        return builder;
    }
}
