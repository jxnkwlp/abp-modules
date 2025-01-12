using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement;

/// <summary>
///  The file admin management app service
/// </summary>
public interface IFileAdminAppService : IApplicationService
{
    Task<PagedResultDto<FileItemDto>> GetListAsync(Guid containerId, FilePagedListRequestDto input);

    Task<FileItemDto> GetAsync(Guid containerId, Guid id);

    Task<FileItemDto> CreateDirectoryAsync(Guid containerId, FileDirectoryCreateDto input);

    Task<FileItemDto> CreateAsync(Guid containerId, FileCreateDto input);
    Task<FileItemDto> CreateByStreamAsync(Guid containerId, FileCreateByStreamDto input);
    Task<FileItemDto> CreateByBytesAsync(Guid containerId, FileCreateByBytesDto input);

    Task<FileItemDto> UpdateAsync(Guid containerId, Guid id, FileUpdateDto input);

    Task<FileItemDto> MoveAsync(Guid containerId, Guid id, FileMoveAdminRequestDto input);

    Task<IRemoteStreamContent?> GetBlobAsync(Guid containerId, Guid id);
    Task<Stream?> GeBlobStreamAsync(Guid containerId, Guid id);
    Task<byte[]?> GetBlobBytesAsync(Guid containerId, Guid id);

    Task DeleteAsync(Guid containerId, Guid id);

    Task SetTagsAsync(Guid containerId, Guid id, FileTagsUpdateDto input);
}
