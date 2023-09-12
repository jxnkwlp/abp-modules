using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Options;
using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.FileManagement.Files;

public class FileContainerManager : DomainService
{
    protected IFileContainerRepository FileContainerRepository { get; }
    protected FileManagementOptions FileManagementOptions { get; }

    public FileContainerManager(IFileContainerRepository fileContainerRepository, IOptions<FileManagementOptions> options)
    {
        FileContainerRepository = fileContainerRepository;
        FileManagementOptions = options.Value;
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
        return await FileContainerRepository.CheckExistsAsync(fileContainer, cancellationToken);
    }

    public virtual async Task CheckExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        _ = await FileContainerRepository.GetByNameAsync(name, cancellationToken);
    }

    public virtual Task<FileContainer> CreateAsync(
        string name,
        FileAccessMode? accessMode,
        string? description = null,
        long? maximumEachFileSize = null,
        int? maximumFileQuantity = null,
        FileOverrideBehavior? overrideBehavior = null,
        bool? allowAnyFileExtension = null,
        string? allowedFileExtensions = null,
        string? prohibitedFileExtensions = null,
        bool? autoDeleteBlob = false)
    {
        var entity = new FileContainer(GuidGenerator.Create(), name, accessMode ?? FileManagementOptions.DefaultContainerAccessMode)
        {
            Description = description,
            MaximumEachFileSize = maximumEachFileSize ?? FileManagementOptions.DefaultMaximumFileSize,
            MaximumFileQuantity = maximumFileQuantity ?? FileManagementOptions.DefaultContainerMaximumFileQuantity,
            AllowAnyFileExtension = allowAnyFileExtension ?? false,
            AllowedFileExtensions = allowedFileExtensions ?? string.Join(",", FileManagementOptions.DefaultAllowedFileExtensions ?? new string[0]),
            ProhibitedFileExtensions = prohibitedFileExtensions ?? string.Join(",", FileManagementOptions.DefaultProhibitedFileExtensions ?? new string[0]),
            OverrideBehavior = overrideBehavior ?? FileManagementOptions.DefaultOverrideBehavior,
            AutoDeleteBlob = autoDeleteBlob ?? false,
        };

        return Task.FromResult(entity);
    }
}
