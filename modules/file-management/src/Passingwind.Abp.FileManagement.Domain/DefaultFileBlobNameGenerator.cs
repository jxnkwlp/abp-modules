using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Passingwind.Abp.FileManagement;

public class DefaultFileBlobNameGenerator : IFileBlobNameGenerator, ITransientDependency
{
    private readonly IClock _clock;
    private readonly FileManagementOptions _options;

    public DefaultFileBlobNameGenerator(IClock clock, IOptions<FileManagementOptions> options)
    {
        _clock = clock;
        _options = options.Value;
    }

    public virtual Task<string> CreateAsync(Guid containerId, Guid fileId, string uniqueId, string fileName, string? mimeType = null, long? length = null, string? hash = null, CancellationToken cancellationToken = default)
    {
        var now = _clock.Now;
        var directorySeparator = _options.BlobDirectorySeparator;

        var blobName = $"{now.Year}{directorySeparator}{now.Month}{directorySeparator}{now.Day}{directorySeparator}{fileId:N}";

        return Task.FromResult(blobName);
    }
}