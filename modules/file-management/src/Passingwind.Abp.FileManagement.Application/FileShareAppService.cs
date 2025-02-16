using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Options;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace Passingwind.Abp.FileManagement;

[Authorize]
public class FileShareAppService : FileManagementAppService, IFileShareAppService
{
    protected IFileItemManager FileManager { get; }
    protected IFileItemRepository FileRepository { get; }
    protected IFileContainerRepository FileContainerRepository { get; }
    protected IFileAccessTokenProvider FileAccessTokenProvider { get; }
    protected IFileAccessTokenRepository FileAccessTokenRepository { get; }
    protected FileManagementOptions FileManagementOptions { get; }

    public FileShareAppService(
        IFileItemManager fileManager,
        IFileItemRepository fileRepository,
        IFileContainerRepository fileContainerRepository,
        IFileAccessTokenProvider fileAccessTokenProvider,
        IOptions<FileManagementOptions> options,
        IFileAccessTokenRepository fileAccessTokenRepository)
    {
        FileManager = fileManager;
        FileRepository = fileRepository;
        FileContainerRepository = fileContainerRepository;
        FileAccessTokenProvider = fileAccessTokenProvider;
        FileManagementOptions = options.Value;
        FileAccessTokenRepository = fileAccessTokenRepository;
    }

    public virtual async Task<PagedResultDto<FileShareResultDto>> GetListAsync(string containerName, FileShareListRequestDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        var userId = CurrentUser.GetId();

        var count = await FileAccessTokenRepository.GetCountAsync(container.Id, fileId: input.FileId, userId: userId);
        var list = await FileAccessTokenRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, container.Id, fileId: input.FileId, userId: userId);

        return new PagedResultDto<FileShareResultDto>(count, list.ConvertAll(x => new FileShareResultDto
        {
            Id = x.Id,
            ContainerId = x.ContainerId,
            ContainerName = containerName,
            FileName = x.FileName,
            Length = x.Length,
            MimeType = x.MimeType,
            ExpirationTime = x.ExpirationTime,
            DirectDownloadUrl = GetDownloadUrl(container, x),
        }));
    }

    public virtual async Task<FileShareResultDto> GetAsync(string containerName, Guid id)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        var entity = await FileAccessTokenRepository.GetAsync(id);

        if (entity.CreatorId != CurrentUser.Id)
        {
            throw new EntityNotFoundException();
        }

        var file = await FileManager.GetAsync(entity.FileId);

        await CheckFileIsInContainerAsync(container, file);

        return new FileShareResultDto
        {
            Id = entity.Id,
            ContainerId = entity.ContainerId,
            ContainerName = containerName,
            FileName = entity.FileName,
            Length = entity.Length,
            MimeType = entity.MimeType,
            ExpirationTime = entity.ExpirationTime,
            DirectDownloadUrl = GetDownloadUrl(container, entity),
        };
    }

    public virtual async Task DeleteAsync(string containerName, Guid id)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        var entity = await FileAccessTokenRepository.GetAsync(id);

        if (entity.CreatorId != CurrentUser.Id)
        {
            throw new EntityNotFoundException();
        }

        var file = await FileManager.GetAsync(entity.FileId);

        await CheckFileIsInContainerAsync(container, file);

        await FileAccessTokenRepository.DeleteAsync(entity);
    }

    public virtual async Task<FileShareResultDto> CreateAsync(string containerName, Guid fileId, FileShareCreateRequestDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var fileItem = await FileRepository.GetAsync(fileId);

        await CheckFileIsInContainerAsync(container, fileItem);

        DateTime? expiration = input.ExpirationTime > Clock.Now ? input.ExpirationTime : null;

        var entity = await FileManager.CreateAccessTokenAsync(fileId, expiration);

        return new FileShareResultDto
        {
            Id = entity.Id,
            ContainerId = entity.ContainerId,
            ContainerName = containerName,
            FileName = entity.FileName,
            Length = entity.Length,
            MimeType = entity.MimeType,
            ExpirationTime = entity.ExpirationTime,
            Token = entity.Token,
            DirectDownloadUrl = GetDownloadUrl(container, entity),
        };
    }

    [AllowAnonymous]
    public virtual async Task<FileShareResultDto> VerifyTokenAsync(string containerName, string token)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        var validationResult = await FileAccessTokenProvider.VerifyAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
        {
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);
        }

        var file = validationResult.File;

        await CheckFileIsInContainerAsync(container, file);

        return new FileShareResultDto
        {
            Id = validationResult.TokenId,
            ContainerId = file.ContainerId,
            ContainerName = containerName,
            FileName = file.FileName,
            Length = file.Length,
            MimeType = file.MimeType,
            ExpirationTime = validationResult.ExpirationTime,
        };
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent?> GetBlobAsync(string containerName, string token)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        var validationResult = await FileAccessTokenProvider.VerifyAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
        {
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);
        }

        var file = validationResult.File;

        await CheckFileIsInContainerAsync(container, file);

        var fileStream = await FileManager.GetFileSteamAsync(file.Id);

        return fileStream == null ? throw new BlobNotFoundException() : new RemoteStreamContent(fileStream, file.FileName, file.MimeType);
    }

    [AllowAnonymous]
    public virtual async Task<byte[]?> GetBytesAsync(string containerName, string token)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        var validationResult = await FileAccessTokenProvider.VerifyAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
        {
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);
        }

        var file = validationResult.File;

        await CheckFileIsInContainerAsync(container, file);

        return await FileManager.GetFileBytesAsync(file.Id);
    }

    [AllowAnonymous]
    public virtual async Task<Stream?> GetStreamAsync(string containerName, string token)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        var validationResult = await FileAccessTokenProvider.VerifyAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
        {
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);
        }

        var file = validationResult.File;

        await CheckFileIsInContainerAsync(container, file);

        return await FileManager.GetFileSteamAsync(file.Id);
    }

    protected virtual string? GetDownloadUrl(FileContainer fileContainer, FileAccessToken fileAccessToken)
    {
        return null;
        // return string.Format(FileManagementOptions.FileShareDownloadUrlFormat, token, containerName);
    }
}
