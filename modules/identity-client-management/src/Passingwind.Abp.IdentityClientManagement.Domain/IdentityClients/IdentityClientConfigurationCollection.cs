using System;
using System.Collections.Generic;
using System.Linq;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;

namespace Passingwind.Abp.IdentityClientManagement.Identity;

public class IdentityClientConfigurationCollection
{
    private readonly List<IdentityClientConfiguration> _configurations;

    public IdentityClientConfigurationCollection(IEnumerable<IdentityClientConfiguration> configurations)
    {
        if (configurations == null)
            throw new ArgumentNullException(nameof(configurations));

        _configurations = configurations.ToList();
    }

    public T GetValue<T>(string name, T defaultValue = default) where T : struct
    {
        var item = _configurations.Find(x => x.Name == name);
        if (item == null || item.Value == default)
            return defaultValue;

        return item.Value.To<T>();
    }

    public string? GetValue(string name, string? defaultValue = default)
    {
        var item = _configurations.Find(x => x.Name == name);
        if (item == null || string.IsNullOrWhiteSpace(item.Value))
            return defaultValue;

        return item.Value;
    }
}
