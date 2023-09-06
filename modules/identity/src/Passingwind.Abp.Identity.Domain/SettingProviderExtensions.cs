using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Settings;

namespace Passingwind.Abp;

public static class SettingProviderExtensions
{
    public static async Task<T> GetEnumValueAsync<T>([NotNull] this ISettingProvider settingProvider, [NotNull] string name)
            where T : struct
    {
        Check.NotNull(settingProvider, nameof(settingProvider));
        Check.NotNull(name, nameof(name));

        var value = await settingProvider.GetOrNullAsync(name);

        if (string.IsNullOrWhiteSpace(value))
            return default;

        return (T)Enum.Parse(typeof(T), value);
    }
}
