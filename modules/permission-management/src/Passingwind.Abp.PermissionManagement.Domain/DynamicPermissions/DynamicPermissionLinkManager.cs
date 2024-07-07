using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public class DynamicPermissionLinkManager : DomainService, IDynamicPermissionLinkManager
{
    protected IDynamicPermissionLinkRepository DynamicPermissionLinkRepository { get; }

    public DynamicPermissionLinkManager(IDynamicPermissionLinkRepository dynamicPermissionLinkRepository)
    {
        DynamicPermissionLinkRepository = dynamicPermissionLinkRepository;
    }

    [UnitOfWork]
    public virtual async Task AddOrUpdateAsync(string dynamicPermissionName, IEnumerable<string> targetNames, CancellationToken cancellationToken = default)
    {
        targetNames = targetNames.Distinct();

        var list = await DynamicPermissionLinkRepository.GetListAsync(x => x.SourceName == dynamicPermissionName, cancellationToken: cancellationToken);

        var oldNames = list.Select(x => x.TargetName).ToArray();

        var removeNames = oldNames.Except(targetNames);

        await DynamicPermissionLinkRepository.DeleteDirectAsync(x => x.SourceName == dynamicPermissionName && removeNames.Contains(x.TargetName), cancellationToken);

        var addNames = targetNames.Except(oldNames);

        await DynamicPermissionLinkRepository.InsertManyAsync(addNames.Select(x => new DynamicPermissionLink(GuidGenerator.Create(), dynamicPermissionName, x)), cancellationToken: cancellationToken);
    }

    [UnitOfWork]
    public virtual async Task DeleteAsync(string dynamicPermissionName, CancellationToken cancellationToken = default)
    {
        await DynamicPermissionLinkRepository.DeleteDirectAsync(x => x.SourceName == dynamicPermissionName, cancellationToken);
    }

    [UnitOfWork]
    public virtual async Task<List<string>> GetLinksAsync(string dynamicPermissionName, CancellationToken cancellationToken = default)
    {
        var list = await DynamicPermissionLinkRepository.GetListAsync(x => x.SourceName == dynamicPermissionName, cancellationToken: cancellationToken);

        return list.ConvertAll(x => x.TargetName);
    }
}
