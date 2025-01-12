using System;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement;

[Obsolete]
public interface IFileNameDuplicateDetectionProvider
{
    Task<bool> IsExistsAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
}
