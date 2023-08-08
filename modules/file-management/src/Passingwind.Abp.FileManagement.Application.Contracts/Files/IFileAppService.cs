using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileAppService : IApplicationService
{
    Task<PagedResultDto<FileDto>> GetListAsync(string containerName, FilePagedListRequestDto input);

    Task<FileDto> GetAsync(string containerName, Guid id);

    Task<IRemoteStreamContent?> GetBlobAsync(string containerName, Guid id);
    Task<Stream?> GeBlobStreamAsync(string containerName, Guid id);
    Task<byte[]> GetBlobBytesAsync(string containerName, Guid id);

    Task<FileDto> CreateDirectoryAsync(string containerName, FileDirectoryCreateDto input);

    Task<FileDto> CreateAsync(string containerName, FileCreateDto input);
    Task<FileDto> CreateByStreamAsync(string containerName, FileCreateByStreamDto input);
    Task<FileDto> CreateByBytesAsync(string containerName, FileCreateByBytesDto input);

    Task<FileDto> UpdateAsync(string containerName, Guid id, FileUpdateDto input);

    Task<FileDto> MoveAsync(string containerName, Guid id, FileMoveRequestDto input);

    Task DeleteAsync(string containerName, Guid id);
}
