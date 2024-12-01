using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement;

/// <summary>
///  Compatible to default container
/// </summary>
public interface IFileCompatibleAppService : IApplicationService
{
    Task<FileDto> CreateAsync(DefaultFileCreateDto input);

    Task<FileDto> CreatebyStreamAsync(DefaultFileCreateByStreamDto input);

    Task<FileDto> CreatebyBytesAsync(DefaultFileCreateByBytesDto input);

    Task<IRemoteStreamContent> GetBlobAsync(Guid id);

    Task<Stream?> GeBlobStreamAsync(Guid id);

    Task<byte[]?> GetBlobBytesAsync(Guid id);
}
