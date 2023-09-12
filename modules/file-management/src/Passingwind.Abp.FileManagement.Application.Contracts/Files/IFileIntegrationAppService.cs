using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.FileManagement.Files;

[IntegrationService]
public interface IFileIntegrationAppService : IApplicationService
{
    Task<FileContainerDto> GetContainerAsync(Guid id);

    Task<FileDto> GetAsync(string containerName, Guid id);

    Task<Stream?> GetStreamAsync(string containerName, Guid id);
    Task<byte[]> GetBytesAsync(string containerName, Guid id);

    Task<FileDto> CreateByStreamAsync(string containerName, FileCreateByStreamDto input);
    Task<FileDto> CreateByBytesAsync(string containerName, FileCreateByBytesDto input);
}
