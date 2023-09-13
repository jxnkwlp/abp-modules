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
    protected IFileManager FileManager;
    protected IFileRepository FileRepository;
    protected IFileContainerRepository FileContainerRepository;
    protected IFileAccessTokenProvider FileAccessTokenProvider;
    protected FileManagementOptions FileManagementOptions;

    public FileShareAppService(
        IFileManager fileManager,
        IFileRepository fileRepository,
        IFileContainerRepository fileContainerRepository,
        IFileAccessTokenProvider fileAccessTokenProvider,
        IOptions<FileManagementOptions> options)
    {
        FileManager = fileManager;
        FileRepository = fileRepository;
        FileContainerRepository = fileContainerRepository;
        FileAccessTokenProvider = fileAccessTokenProvider;
        FileManagementOptions = options.Value;
    }

    public virtual async Task<FileShareResultDto> CreateAsync(string containerName, Guid id, FileShareCreateRequestDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await FileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, entity);

        TimeSpan? expiration = input.ExpirationSecond.HasValue ? TimeSpan.FromSeconds(input.ExpirationSecond.Value) : null;
        var token = await FileAccessTokenProvider.CreateAsync(container, entity, expiration);

        return new FileShareResultDto
        {
            FileName = entity.FileName,
            Length = entity.Length,
            MimeType = entity.MimeType,
            ExpirationTime = expiration.HasValue ? Clock.Now.Add(expiration.Value) : null,
            DownloadUrl = string.Format(FileManagementOptions.FileShareDownloadUrlFormat, token, container.Name),
            Token = token,
        };
    }

    [AllowAnonymous]
    public virtual async Task<FileShareResultDto> GetAsync(string token)
    {
        var validationResult = await FileAccessTokenProvider.ValidAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        var file = validationResult.File;

        var container = await FileContainerRepository.FindAsync(file.ContainerId);

        if (container == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        return new FileShareResultDto
        {
            FileName = file.FileName,
            Length = file.Length,
            MimeType = file.MimeType,
            ExpirationTime = validationResult.ExpirationTime,
            DownloadUrl = string.Format(FileManagementOptions.FileShareDownloadUrlFormat, token),
            Token = token,
        };
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent?> GetBlobAsync(string token)
    {
        var validationResult = await FileAccessTokenProvider.ValidAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        var file = validationResult.File;

        var container = await FileContainerRepository.FindAsync(file.ContainerId);

        if (container == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        var fileStream = await FileManager.GetFileSteamAsync(container, file);

        if (fileStream == null)
            throw new BlobNotFoundException();

        return new RemoteStreamContent(fileStream, file.FileName, file.MimeType);
    }

    [AllowAnonymous]
    public virtual async Task<byte[]?> GetBytesAsync(string token)
    {
        var validationResult = await FileAccessTokenProvider.ValidAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        var file = validationResult.File;

        var container = await FileContainerRepository.FindAsync(file.ContainerId);

        if (container == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        return await FileManager.GetFileBytesAsync(container, file);
    }

    [AllowAnonymous]
    public virtual async Task<Stream?> GetStreamAsync(string token)
    {
        var validationResult = await FileAccessTokenProvider.ValidAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        var file = validationResult.File;

        var container = await FileContainerRepository.FindAsync(file.ContainerId);

        if (container == null)
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);

        return await FileManager.GetFileSteamAsync(container, file);
    }
}
