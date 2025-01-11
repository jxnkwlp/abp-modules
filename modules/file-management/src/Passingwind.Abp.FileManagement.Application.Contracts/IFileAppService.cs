using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement;

/// <summary>
///  The file app service for users
/// </summary>
public interface IFileAppService : IApplicationService
{
    Task<PagedResultDto<FileItemDto>> GetListAsync(string containerName, FilePagedListRequestDto input);

    Task<FileItemDto> GetAsync(string containerName, Guid id);

    Task<FileItemDto> CreateDirectoryAsync(string containerName, FileDirectoryCreateDto input);

    Task<FileItemDto> CreateAsync(string containerName, FileCreateDto input);
    Task<FileItemDto> CreateByStreamAsync(string containerName, FileCreateByStreamDto input);
    Task<FileItemDto> CreateByBytesAsync(string containerName, FileCreateByBytesDto input);

    Task<FileItemDto> RenameAsync(string containerName, Guid id, FileUpdateDto input);

    Task<FileItemDto> MoveAsync(string containerName, Guid id, FileMoveRequestDto input);

    Task<IRemoteStreamContent?> GetBlobAsync(string containerName, Guid id);
    Task<Stream?> GeBlobStreamAsync(string containerName, Guid id);
    Task<byte[]?> GetBlobBytesAsync(string containerName, Guid id);
}
