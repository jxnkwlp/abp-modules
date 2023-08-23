using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public interface IDictionaryItemRepository : IRepository<DictionaryItem, Guid>
{
    Task<long> GetCountAsync(string? filter = null, string? groupName = null, bool? isEnabled = null, CancellationToken cancellationToken = default);

    Task<List<DictionaryItem>> GetListAsync(string? filter = null, string? groupName = null, bool? isEnabled = null, bool includeDetails = false, string? sorting = null, CancellationToken cancellationToken = default);

    Task<List<DictionaryItem>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, string? groupName = null, bool? isEnabled = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<DictionaryItem?> FindByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<DictionaryItem> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<bool> IsNameExistsAsync(string name, IEnumerable<Guid>? excludeIds = null, CancellationToken cancellationToken = default);
}
