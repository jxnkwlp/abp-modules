using System;
using System.Threading;
using System.Threading.Tasks;
using Passingwind.Abp.FileManagement.Files;
using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.FileManagement;

public interface IFileContainerManager : IDomainService
{
    Task CheckExistsAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> IsExistsAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> IsExistsAsync(FileContainer fileContainer, CancellationToken cancellationToken = default);

    Task<FileContainer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<FileContainer> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<FileContainer> CreateAsync(
        string name,
        FileAccessMode accessMode = FileAccessMode.Authorized,
        FileOverrideBehavior overrideBehavior = FileOverrideBehavior.None,
        string? description = null,
        long? maximumEachFileSize = null,
        int? maximumFileQuantity = null,
        bool? allowAnyFileExtension = null,
        string? allowedFileExtensions = null,
        string? prohibitedFileExtensions = null,
        bool? autoDeleteBlob = false);

    Task<FileContainer> UpdateAsync(FileContainer fileContainer, CancellationToken cancellationToken = default);

    Task DeleteAsync(FileContainer fileContainer, CancellationToken cancellationToken = default);
    Task<bool> CanAccessAsync(string name, string providerName, Guid providerId, CancellationToken cancellationToken = default);
}
