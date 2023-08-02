using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Options;
using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.FileManagement.Files;

public class FileContainerManager : DomainService
{
    private readonly IFileContainerRepository _fileContainerRepository;
    private readonly FileManagementOptions _options;

    public FileContainerManager(IFileContainerRepository fileContainerRepository, IOptions<FileManagementOptions> options)
    {
        _fileContainerRepository = fileContainerRepository;
        _options = options.Value;
    }

    public virtual async Task<bool> IsExistsAsync(FileContainer fileContainer, CancellationToken cancellationToken = default)
    {
        return await _fileContainerRepository.CheckExistsAsync(fileContainer, cancellationToken);
    }

    public virtual async Task CheckExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        _ = await _fileContainerRepository.GetByNameAsync(name, cancellationToken);
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
        var entity = new FileContainer(GuidGenerator.Create(), name, accessMode ?? _options.DefaultContainerAccessMode)
        {
            Description = description,
            MaximumEachFileSize = maximumEachFileSize ?? _options.DefaultMaximumFileSize,
            MaximumFileQuantity = maximumFileQuantity ?? _options.DefaultContainerMaximumFileQuantity,
            AllowAnyFileExtension = allowAnyFileExtension ?? false,
            AllowedFileExtensions = allowedFileExtensions ?? string.Join(",", _options.DefaultAllowedFileExtensions ?? new string[0]),
            ProhibitedFileExtensions = prohibitedFileExtensions ?? string.Join(",", _options.DefaultProhibitedFileExtensions ?? new string[0]),
            OverrideBehavior = overrideBehavior ?? _options.DefaultOverrideBehavior,
            AutoDeleteBlob = autoDeleteBlob ?? false,
        };

        return Task.FromResult(entity);
    }
}
