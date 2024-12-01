using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement;

public interface IFileShareAppService : IApplicationService
{
    Task<PagedResultDto<FileShareResultDto>> GetListAsync(string containerName, FileShareListRequestDto input);

    Task<FileShareResultDto> CreateAsync(string containerName, Guid fileId, FileShareCreateRequestDto input);

    Task<FileShareResultDto> GetAsync(string containerName, Guid id);

    Task DeleteAsync(string containerName, Guid id);

    Task<FileShareResultDto> VerifyTokenAsync(string containerName, string token);

    Task<IRemoteStreamContent?> GetBlobAsync(string containerName, string token);

    Task<byte[]?> GetBytesAsync(string containerName, string token);

    Task<Stream?> GetStreamAsync(string containerName, string token);
}
