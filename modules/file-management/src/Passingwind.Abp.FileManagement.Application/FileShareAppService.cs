using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Files;
using Passingwind.Abp.FileManagement.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace Passingwind.Abp.FileManagement;

[Authorize]
public class FileShareAppService : FileManagementAppService, IFileShareAppService
{
    protected IFileManager FileManager { get; }
    protected IFileRepository FileRepository { get; }
    protected IFileContainerRepository FileContainerRepository { get; }
    protected IFileAccessTokenProvider FileAccessTokenProvider { get; }
    protected IFileAccessTokenRepository FileAccessTokenRepository { get; }
    protected FileManagementOptions FileManagementOptions { get; }

    public FileShareAppService(
        IFileManager fileManager,
        IFileRepository fileRepository,
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
            FileName = x.FileName,
            Length = x.Length,
            MimeType = x.MimeType,
            ExpirationTime = x.ExpirationTime,
            Token = x.Token,
            DirectDownloadUrl = GetDownloadUrl(x.Token, containerName),
        }));
    }

    public virtual async Task<FileShareResultDto> GetAsync(Guid id)
    {
        var entity = await FileAccessTokenRepository.GetAsync(id);

        if (entity.CreatorId != CurrentUser.Id)
        {
            throw new EntityNotFoundException();
        }

        var file = await FileManager.GetAsync(entity.FileId);

        var container = await FileContainerRepository.GetAsync(file.ContainerId);

        return new FileShareResultDto
        {
            Id = entity.Id,
            FileName = entity.FileName,
            Length = entity.Length,
            MimeType = entity.MimeType,
            ExpirationTime = entity.ExpirationTime,
            Token = entity.Token,
            DirectDownloadUrl = GetDownloadUrl(entity.Token, container.Name),
        };
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await FileAccessTokenRepository.GetAsync(id);

        if (entity.CreatorId != CurrentUser.Id)
        {
            throw new EntityNotFoundException();
        }

        await FileAccessTokenRepository.DeleteAsync(entity);
    }

    public virtual async Task<FileShareResultDto> CreateAsync(string containerName, Guid id, FileShareCreateRequestDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var fileItem = await FileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, fileItem);

        TimeSpan? expiration = input.ExpirationSecond.HasValue ? TimeSpan.FromSeconds(input.ExpirationSecond.Value) : null;

        var entity = await FileManager.CreateAccessTokenAsync(container.Id, id, expiration);

        return new FileShareResultDto
        {
            Id = entity.Id,
            FileName = entity.FileName,
            Length = entity.Length,
            MimeType = entity.MimeType,
            ExpirationTime = entity.ExpirationTime,

            DirectDownloadUrl = GetDownloadUrl(entity.Token, container.Name),
            Token = entity.Token,
        };
    }

    [AllowAnonymous]
    public virtual async Task<FileShareResultDto> VerifyTokenAsync(string token)
    {
        var validationResult = await FileAccessTokenProvider.VerifyAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
        {
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);
        }

        var file = validationResult.File;

        return new FileShareResultDto
        {
            Id = validationResult.TokenId,
            FileName = file.FileName,
            Length = file.Length,
            MimeType = file.MimeType,
            ExpirationTime = validationResult.ExpirationTime,
        };
    }

    [AllowAnonymous]
    public virtual async Task<FileShareResultDto> GetAsync(string token)
    {
        var validationResult = await FileAccessTokenProvider.VerifyAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
        {
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);
        }

        var file = validationResult.File;

        return new FileShareResultDto
        {
            FileName = file.FileName,
            Length = file.Length,
            MimeType = file.MimeType,
            ExpirationTime = validationResult.ExpirationTime,
        };
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent?> GetBlobAsync(string token)
    {
        var validationResult = await FileAccessTokenProvider.VerifyAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
        {
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);
        }

        var file = validationResult.File;

        var fileStream = await FileManager.GetFileSteamAsync(file.ContainerId, file.Id);

        return fileStream == null ? throw new BlobNotFoundException() : new RemoteStreamContent(fileStream, file.FileName, file.MimeType);
    }

    [AllowAnonymous]
    public virtual async Task<byte[]?> GetBytesAsync(string token)
    {
        var validationResult = await FileAccessTokenProvider.VerifyAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
        {
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);
        }

        var file = validationResult.File;

        return await FileManager.GetFileBytesAsync(file.ContainerId, file.Id);
    }

    [AllowAnonymous]
    public virtual async Task<Stream?> GetStreamAsync(string token)
    {
        var validationResult = await FileAccessTokenProvider.VerifyAsync(token);

        if (!validationResult.IsValid || validationResult.File == null)
        {
            throw new BusinessException(FileManagementErrorCodes.ShareFileNotExistsOrExpired);
        }

        var file = validationResult.File;

        return await FileManager.GetFileSteamAsync(file.ContainerId, file.Id);
    }

    protected virtual string GetDownloadUrl(string token, string containerName)
    {
        return string.Format(FileManagementOptions.FileShareDownloadUrlFormat, token, containerName);
    }
}
