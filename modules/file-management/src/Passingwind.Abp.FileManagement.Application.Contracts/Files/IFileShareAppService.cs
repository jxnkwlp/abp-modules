using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileShareAppService : IApplicationService
{
    Task<FileShareResultDto> CreateAsync(string containerName, Guid id, FileShareCreateRequestDto input);

    Task<FileShareResultDto> GetAsync(string token);

    Task<IRemoteStreamContent?> GetBlobAsync(string token);

    Task<byte[]?> GetBytesAsync(string token);

    Task<Stream?> GetStreamAsync(string token);
}
