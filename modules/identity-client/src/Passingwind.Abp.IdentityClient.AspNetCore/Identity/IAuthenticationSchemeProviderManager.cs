using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.IdentityClient.Identity;

public interface IAuthenticationSchemeProviderManager
{
    Task RegisterAsync<TOptions, THandler>(string name, string displayName, TOptions options, CancellationToken cancellationToken = default) where TOptions : class;

    Task UnRegisterAsync<TOptions>(string name, CancellationToken cancellationToken = default) where TOptions : class;
}
