using System;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileRenameProvider
{
    Task<string> GetAsync(FileContainer container, string fileName, Guid? parentId, CancellationToken cancellationToken = default);
}
