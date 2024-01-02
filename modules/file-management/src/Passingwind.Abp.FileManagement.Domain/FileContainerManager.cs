using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Files;
using Passingwind.Abp.FileManagement.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.FileManagement;

public class FileContainerManager : DomainService, IFileContainerManager
{
    protected IFileContainerRepository FileContainerRepository { get; }
    protected FileManagementOptions FileManagementOptions { get; }
    protected IFileContainerAccessProvider FileContainerAccessProvider { get; }

    public FileContainerManager(IFileContainerRepository fileContainerRepository, IOptions<FileManagementOptions> options, IFileContainerAccessProvider fileContainerAccessProvider)
    {
        FileContainerRepository = fileContainerRepository;
        FileManagementOptions = options.Value;
        FileContainerAccessProvider = fileContainerAccessProvider;
    }

    public virtual async Task<FileContainer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await FileContainerRepository.GetAsync(id, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileContainer> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await FileContainerRepository.GetByNameAsync(name, cancellationToken);
    }

    public virtual async Task<bool> IsExistsAsync(FileContainer fileContainer, CancellationToken cancellationToken = default)
    {
        return await FileContainerRepository.IsNameExistsAsync(fileContainer.Name, new[] { fileContainer.Id }, cancellationToken);
    }

    public virtual async Task<bool> IsExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await FileContainerRepository.IsNameExistsAsync(name, cancellationToken: cancellationToken);
    }

    public virtual async Task CheckExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        _ = await FileContainerRepository.GetByNameAsync(name, cancellationToken);
    }

    public virtual async Task<FileContainer> CreateAsync(string name, FileAccessMode accessMode = FileAccessMode.Authorized, FileOverrideBehavior overrideBehavior = FileOverrideBehavior.None, string? description = null, long? maximumEachFileSize = null, int? maximumFileQuantity = null, bool? allowAnyFileExtension = null, string? allowedFileExtensions = null, string? prohibitedFileExtensions = null, bool? autoDeleteBlob = false)
    {
        var entity = new FileContainer(GuidGenerator.Create(), name, accessMode)
        {
            Description = description,
            MaximumEachFileSize = maximumEachFileSize ?? FileManagementOptions.DefaultMaximumFileSize,
            MaximumFileQuantity = maximumFileQuantity ?? FileManagementOptions.DefaultContainerMaximumFileQuantity,
            AllowAnyFileExtension = allowAnyFileExtension ?? false,
            AllowedFileExtensions = allowedFileExtensions ?? string.Join(",", FileManagementOptions.DefaultAllowedFileExtensions ?? new string[0]),
            ProhibitedFileExtensions = prohibitedFileExtensions ?? string.Join(",", FileManagementOptions.DefaultProhibitedFileExtensions ?? new string[0]),
            OverrideBehavior = overrideBehavior,
            AutoDeleteBlob = autoDeleteBlob ?? false,
        };

        return await FileContainerRepository.InsertAsync(entity);
    }

    public virtual async Task<FileContainer> UpdateAsync(FileContainer fileContainer, CancellationToken cancellationToken = default)
    {
        return await FileContainerRepository.UpdateAsync(fileContainer, cancellationToken: cancellationToken);
    }

    public virtual async Task DeleteAsync(FileContainer fileContainer, CancellationToken cancellationToken = default)
    {
        await FileContainerRepository.DeleteAsync(fileContainer, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> CanAccessAsync(string name, string providerName, Guid providerId, CancellationToken cancellationToken = default)
    {
        return await FileContainerAccessProvider.IsGrantedAsync(name, providerName, providerId, cancellationToken);
    }
}
