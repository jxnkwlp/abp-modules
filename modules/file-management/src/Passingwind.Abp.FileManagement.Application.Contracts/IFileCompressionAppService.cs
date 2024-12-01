using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement;

/// <summary>
///  File compression services
/// </summary>
public interface IFileCompressionAppService : IApplicationService
{
    /// <summary>
    ///  Compress some files into specified file, and return it as <see cref="IRemoteStreamContent"/>
    /// </summary>
    Task<IRemoteStreamContent> CompressAsync(string containerName, FileCompressToBlobRequestDto input);

    /// <summary>
    ///  Compress some files into specified file, and return file info
    /// </summary>
    Task<FileDto> CompressToFileAsync(string containerName, FileCompressRequestDto input);

    /// <summary>
    ///  Compress some files into specified file, and return file info
    /// </summary>
    Task<Stream?> CompressToStreamAsync(string containerName, FileCompressRequestDto input);

    /// <summary>
    ///  Decompress an file to current directory
    /// </summary>
    Task<FileDecompressResultDto> DecompressAsync(string containerName, Guid id, FileDecompressRequestDto input);
}
