using System;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement;

public interface IFileRenameProvider
{
    Task<string> RenameAsync(string container, string fileName, Guid? parentId = null, bool isDirectory = false, CancellationToken cancellationToken = default);
    Task<string> RenameAsync(FileContainer container, string fileName, Guid? parentId = null, bool isDirectory = false, CancellationToken cancellationToken = default);
}
