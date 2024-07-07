using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public interface IDictionaryGroupRepository : IRepository<DictionaryGroup, Guid>
{
    Task<long> GetCountAsync(string? filter = null, string? parentName = null, bool? isPublic = null, CancellationToken cancellationToken = default);

    Task<List<DictionaryGroup>> GetListAsync(string? filter = null, string? parentName = null, bool? isPublic = null, bool includeDetails = false, string? sorting = null, CancellationToken cancellationToken = default);

    Task<List<DictionaryGroup>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, string? parentName = null, bool? isPublic = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<DictionaryGroup?> FindByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<DictionaryGroup> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<bool> IsNameExistsAsync(string name, IEnumerable<Guid>? excludeIds = null, CancellationToken cancellationToken = default);
}
