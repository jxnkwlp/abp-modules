using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.IdentityClient;

public interface IIdentityClientRepository : IRepository<IdentityClient, Guid>
{
    Task<long> GetCountAsync(string? filter, CancellationToken cancellationToken = default);

    Task<List<IdentityClient>> GetListAsync(string? filter, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<List<IdentityClient>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<IdentityClient> GetByNameAsync(string name, bool includeDetails = true, CancellationToken cancellationToken = default);
    Task<IdentityClient?> FindByNameAsync(string name, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<IdentityClient?> FindByProviderNameAsync(string providerName, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<IdentityClient> GetByProviderNameAsync(string providerName, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<bool> IsNameExistsAsync(string name, Guid[]? excludeIds = null, CancellationToken cancellationToken = default);
}
