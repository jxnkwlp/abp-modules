using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.FileManagement.Permissions;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement.Admin;

[Authorize(FileManagementPermissions.FileShares.Default)]
public class FileShareAdminAppService : FileManagementAppService, IFileShareAdminAppService
{
    protected IFileItemManager FileManager { get; }
    protected IFileItemRepository FileRepository { get; }
    protected IFileContainerRepository FileContainerRepository { get; }
    protected IFileAccessTokenRepository FileAccessTokenRepository { get; }

    public FileShareAdminAppService(
        IFileItemManager fileManager,
        IFileItemRepository fileRepository,
        IFileContainerRepository fileContainerRepository,
        IFileAccessTokenRepository fileAccessTokenRepository)
    {
        FileManager = fileManager;
        FileRepository = fileRepository;
        FileContainerRepository = fileContainerRepository;
        FileAccessTokenRepository = fileAccessTokenRepository;
    }

    public virtual async Task<PagedResultDto<FileShareResultDto>> GetListAsync(FileShareAdminListRequestDto input)
    {
        var count = await FileAccessTokenRepository.GetCountAsync(containerId: input.ContainerId, fileId: input.FileId, userId: input.UserId);
        var list = await FileAccessTokenRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, containerId: input.ContainerId, fileId: input.FileId, userId: input.UserId);

        var containerIds = list.Select(x => x.ContainerId).Distinct().ToArray();
        var containers = await FileContainerRepository.GetListByIdsAsync(containerIds);

        return new PagedResultDto<FileShareResultDto>(count, list.ConvertAll(x => new FileShareResultDto
        {
            Id = x.Id,
            ContainerId = x.ContainerId,
            ContainerName = containers.Find(c => c.Id == x.ContainerId)?.Name,
            FileName = x.FileName,
            Length = x.Length,
            MimeType = x.MimeType,
            ExpirationTime = x.ExpirationTime,
        }));
    }

    public virtual async Task<FileShareResultDto> GetAsync(Guid id)
    {
        var entity = await FileAccessTokenRepository.GetAsync(id);

        _ = await FileManager.GetAsync(entity.FileId);
        var container = await FileContainerRepository.GetAsync(entity.ContainerId);

        return new FileShareResultDto
        {
            Id = entity.Id,
            ContainerId = container.Id,
            ContainerName = container.Name,
            FileName = entity.FileName,
            Length = entity.Length,
            MimeType = entity.MimeType,
            ExpirationTime = entity.ExpirationTime,
        };
    }

    [Authorize(FileManagementPermissions.FileShares.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await FileAccessTokenRepository.GetAsync(id);

        await FileAccessTokenRepository.DeleteAsync(entity);
    }
}
