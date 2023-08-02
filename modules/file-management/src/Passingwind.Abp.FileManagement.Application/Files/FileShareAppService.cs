using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Options;
using Volo.Abp;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement.Files;

[Authorize]
public class FileShareAppService : FileManagementAppService, IFileShareAppService
{
    private readonly IFileManager _fileManager;
    private readonly IFileRepository _fileRepository;
    private readonly IFileContainerRepository _fileContainerRepository;
    private readonly IFileAccessTokenProvider _fileAccessTokenProvider;
    private readonly FileManagementOptions _options;

    public FileShareAppService(
        IFileManager fileManager,
        IFileRepository fileRepository,
        IFileContainerRepository fileContainerRepository,
        IFileAccessTokenProvider fileAccessTokenProvider,
        IOptions<FileManagementOptions> options)
    {
        _fileManager = fileManager;
        _fileRepository = fileRepository;
        _fileContainerRepository = fileContainerRepository;
        _fileAccessTokenProvider = fileAccessTokenProvider;
        _options = options.Value;
    }

    public virtual async Task<FileShareResultDto> CreateAsync(string containerName, Guid id, FileShareCreateRequestDto input)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await _fileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, entity);

        TimeSpan? expiration = input.ExpirationSecond.HasValue ? TimeSpan.FromSeconds(input.ExpirationSecond.Value) : null;
        var token = await _fileAccessTokenProvider.CreateAsync(container, entity, expiration);

        return new FileShareResultDto
        {
            FileName = entity.FileName,
            Length = entity.Length,
            MimeType = entity.MimeType,
            ExpirationTime = expiration.HasValue ? Clock.Now.Add(expiration.Value) : null,
            DownloadUrl = string.Format(_options.FileShareDownloadUrlFormat, token, container.Name),
            Token = token,
        };
    }

    [AllowAnonymous]
    public virtual async Task<FileShareResultDto> GetAsync(string token)
    {
        var validationResult = await _fileAccessTokenProvider.ValidAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        var file = validationResult.File;

        var container = await _fileContainerRepository.FindAsync(file.ContainerId);

        if (container == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        return new FileShareResultDto
        {
            FileName = file.FileName,
            Length = file.Length,
            MimeType = file.MimeType,
            ExpirationTime = validationResult.ExpirationTime,
            DownloadUrl = string.Format(_options.FileShareDownloadUrlFormat, token),
            Token = token,
        };
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent?> GetBlobAsync(string token)
    {
        var validationResult = await _fileAccessTokenProvider.ValidAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        var file = validationResult.File;

        var container = await _fileContainerRepository.FindAsync(file.ContainerId);

        if (container == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        var fileStream = await _fileManager.GetFileSteamAsync(container, file);

        if (fileStream == null)
            throw new BlobNotFoundException();

        return new RemoteStreamContent(fileStream, file.FileName, file.MimeType);
    }

    [AllowAnonymous]
    public virtual async Task<byte[]?> GetBytesAsync(string token)
    {
        var validationResult = await _fileAccessTokenProvider.ValidAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        var file = validationResult.File;

        var container = await _fileContainerRepository.FindAsync(file.ContainerId);

        if (container == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        return await _fileManager.GetFileBytesAsync(container, file);
    }

    [AllowAnonymous]
    public virtual async Task<Stream?> GetStreamAsync(string token)
    {
        var validationResult = await _fileAccessTokenProvider.ValidAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        var file = validationResult.File;

        var container = await _fileContainerRepository.FindAsync(file.ContainerId);

        if (container == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        return await _fileManager.GetFileSteamAsync(container, file);
    }
}
