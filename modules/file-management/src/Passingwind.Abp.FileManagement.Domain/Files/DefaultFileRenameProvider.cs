using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.FileManagement.Files;

public class DefaultFileRenameProvider : IFileRenameProvider, ITransientDependency
{
    private readonly IFileRepository _fileRepository;

    public DefaultFileRenameProvider(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public virtual async Task<string> GetAsync(FileContainer container, string fileName, Guid? parentId, CancellationToken cancellationToken = default)
    {
        // TODO
        bool exist = false;

        var newFileName = fileName;

        var name = Path.GetFileNameWithoutExtension(fileName);
        var ext = Path.GetExtension(newFileName);

        var count = await _fileRepository.CountAsync(x => x.ContainerId == container.Id && x.ParentId == parentId, cancellationToken);

        newFileName = $"{name} - renamed{count + 1}{ext}";

        int loopIndex = 0;

        do
        {
            loopIndex++;

            exist = await _fileRepository.IsFileNameExistsAsync(container.Id, newFileName, parentId);

            if (exist)
            {
                newFileName = $"{name} - renamed{count + 1}_{loopIndex}{ext}";
            }

        } while (exist);

        return newFileName;
    }
}