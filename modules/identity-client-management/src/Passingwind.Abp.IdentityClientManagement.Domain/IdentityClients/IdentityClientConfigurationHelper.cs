using System;
using System.Collections.Generic;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;

namespace Passingwind.Abp.IdentityClientManagement.Identity;

public static class IdentityClientConfigurationHelper
{
    public static IdentityClientSaml2Configuration ToSaml2Configuration(IEnumerable<IdentityClientConfiguration> configurations)
    {
        var result = new IdentityClientSaml2Configuration();

        return ToObject(result, configurations);
    }

    public static IdentityClientOpenIdConnectConfiguration ToOpenIdConnectConfiguration(IEnumerable<IdentityClientConfiguration> configurations)
    {
        var result = new IdentityClientOpenIdConnectConfiguration();

        return ToObject(result, configurations);

        // var c = new IdentityClientConfigurationCollection(configurations);

        //ObjectHelper.TrySetProperty(result, x => x.Authority, _ => c.GetValue(nameof(IdentityClientOpenIdConnectConfiguration.Authority)));
        //ObjectHelper.TrySetProperty(result, x => x.ClientId, _ => c.GetValue(nameof(IdentityClientOpenIdConnectConfiguration.ClientId)));
        //ObjectHelper.TrySetProperty(result, x => x.ClientSecret, _ => c.GetValue(nameof(IdentityClientOpenIdConnectConfiguration.ClientSecret)));
        //ObjectHelper.TrySetProperty(result, x => x.MetadataAddress, _ => c.GetValue(nameof(IdentityClientOpenIdConnectConfiguration.MetadataAddress)));
        //ObjectHelper.TrySetProperty(result, x => x.RequireHttpsMetadata, _ => c.GetValue<bool>(nameof(IdentityClientOpenIdConnectConfiguration.RequireHttpsMetadata)));
        //ObjectHelper.TrySetProperty(result, x => x.ResponseMode, _ => c.GetValue(nameof(IdentityClientOpenIdConnectConfiguration.ResponseMode)));
        //ObjectHelper.TrySetProperty(result, x => x.ResponseType, _ => c.GetValue(nameof(IdentityClientOpenIdConnectConfiguration.ResponseType)));
        //ObjectHelper.TrySetProperty(result, x => x.UsePkce, _ => c.GetValue<bool>(nameof(IdentityClientOpenIdConnectConfiguration.UsePkce)));
        //ObjectHelper.TrySetProperty(result, x => x.Scope, _ => c.GetValue(nameof(IdentityClientOpenIdConnectConfiguration.Scope)));
        //ObjectHelper.TrySetProperty(result, x => x.GetClaimsFromUserInfoEndpoint, _ => c.GetValue<bool>(nameof(IdentityClientOpenIdConnectConfiguration.GetClaimsFromUserInfoEndpoint)));

        // return result;
    }

    public static IReadOnlyList<IdentityClientConfiguration> ToConfigurations<T>(T configuration)
    {
        var result = new List<IdentityClientConfiguration>();

        foreach (var item in typeof(T).GetProperties())
        {
            result.Add(new IdentityClientConfiguration()
            {
                Name = item.Name,
                Value = item.GetValue(configuration, null)?.ToString()
            });
        }

        return result;
    }

    private static T ToObject<T>(T instance, IEnumerable<IdentityClientConfiguration> configurations) where T : class
    {
        var properties = typeof(T).GetProperties();

        foreach (var item in configurations)
        {
            var property = System.Array.Find(properties, x => x.Name == item.Name);
            if (property == null)
                continue;

            if (string.IsNullOrWhiteSpace(item.Value))
                continue;

            var originType = property.PropertyType;
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                originType = Nullable.GetUnderlyingType(property.PropertyType);
            }

            var value = Convert.ChangeType(item.Value, originType);

            property.SetValue(instance, value, null);
        }

        return instance;
    }
}
