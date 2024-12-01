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
    Task<PagedResultDto<FileDto>> GetListAsync(Guid containerId, FilePagedListRequestDto input);

    Task<FileDto> GetAsync(Guid containerId, Guid id);

    Task<FileDto> CreateDirectoryAsync(Guid containerId, FileDirectoryCreateDto input);

    Task<FileDto> CreateAsync(Guid containerId, FileCreateDto input);
    Task<FileDto> CreateByStreamAsync(Guid containerId, FileCreateByStreamDto input);
    Task<FileDto> CreateByBytesAsync(Guid containerId, FileCreateByBytesDto input);

    Task<FileDto> UpdateAsync(Guid containerId, Guid id, FileUpdateDto input);

    Task<FileDto> MoveAsync(Guid containerId, Guid id, FileMoveAdminRequestDto input);

    Task<IRemoteStreamContent?> GetBlobAsync(Guid containerId, Guid id);
    Task<Stream?> GeBlobStreamAsync(Guid containerId, Guid id);
    Task<byte[]?> GetBlobBytesAsync(Guid containerId, Guid id);

    Task DeleteAsync(Guid containerId, Guid id);

    Task SetTagsAsync(Guid containerId, Guid id, FileTagsUpdateDto input);
}
