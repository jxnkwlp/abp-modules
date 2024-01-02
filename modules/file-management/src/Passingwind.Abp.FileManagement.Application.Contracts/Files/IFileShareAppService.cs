using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileShareAppService : IApplicationService
{
    Task<PagedResultDto<FileShareResultDto>> GetListAsync(string containerName, FileShareListRequestDto input);

    Task<FileShareResultDto> CreateAsync(string containerName, Guid id, FileShareCreateRequestDto input);

    Task<FileShareResultDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<FileShareResultDto> VerifyTokenAsync(string token);

    Task<IRemoteStreamContent?> GetBlobAsync(string token);

    Task<byte[]?> GetBytesAsync(string token);

    Task<Stream?> GetStreamAsync(string token);
}
