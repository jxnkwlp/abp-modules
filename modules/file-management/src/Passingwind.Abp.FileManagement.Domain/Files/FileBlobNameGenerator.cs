using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Passingwind.Abp.FileManagement.Files;

public class FileBlobNameGenerator : IFileBlobNameGenerator, ITransientDependency
{
    private readonly IClock _clock;
    private readonly FileManagementOptions _options;

    public FileBlobNameGenerator(IClock clock, IOptions<FileManagementOptions> options)
    {
        _clock = clock;
        _options = options.Value;
    }

    public Task<string> CreateAsync(Guid containerId, Guid fileId, string uniqueId, string fileName, string mimeType, long length, string hash)
    {
        var now = _clock.Now;
        var directorySeparator = _options.BlobDirectorySeparator;

        var blobName = $"{now.Year}{directorySeparator}{now.Month}{directorySeparator}{now.Day}{directorySeparator}{uniqueId}";

        return Task.FromResult(blobName);
    }
}