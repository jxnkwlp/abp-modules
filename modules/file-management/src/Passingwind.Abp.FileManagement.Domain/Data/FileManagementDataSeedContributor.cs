using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace Passingwind.Abp.FileManagement.Data;

public class FileManagementDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly ILogger<FileManagementDataSeedContributor> _logger;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IFileContainerRepository _fileContainerRepository;
    private readonly IFileItemRepository _fileRepository;
    private readonly IFileItemManager _fileManager;

    public FileManagementDataSeedContributor(
        ILogger<FileManagementDataSeedContributor> logger,
        IGuidGenerator guidGenerator,
        IFileContainerRepository fileContainerRepository,
        IFileItemRepository fileRepository,
        IFileItemManager fileManager)
    {
        _logger = logger;
        _guidGenerator = guidGenerator;
        _fileContainerRepository = fileContainerRepository;
        _fileRepository = fileRepository;
        _fileManager = fileManager;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await CreateDefaultContainer();
        await UpdateFullPathAsync();
    }

    protected virtual async Task CreateDefaultContainer()
    {
        var count = await _fileContainerRepository.CountAsync();

        // create 'default' container
        if (count == 0)
        {
            var entity = new FileContainer(_guidGenerator.Create(), "default", FileAccessMode.Authorized)
            {
                Description = "The default container",
                MaximumEachFileSize = int.MaxValue,
                MaximumFileQuantity = int.MaxValue,
                AllowAnyFileExtension = false,
                AllowedFileExtensions = ".txt,.png,.jpg,.jpeg,.bmp,.gif,.docx,.doc,.xlsx,.xls,.pdf,.ppt,.pptx,.zip,.rar,.7z",
                OverrideBehavior = FileOverrideBehavior.Rename,
            };

            entity.SetIsDefault(true);

            await _fileContainerRepository.InsertAsync(entity);

            _logger.LogInformation("Initial default file contianer.");
        }
    }

    protected virtual async Task UpdateFullPathAsync()
    {
        var list = await _fileRepository.GetListAsync(x => x.Path == null || string.IsNullOrEmpty(x.Path.FullPath));

        foreach (var item in list)
        {
            try
            {
                await _fileManager.RefreshFullPathAsync(item.Id);
            }
            catch
            {
                //  Ignore dirty data
                _logger.LogWarning($"Refresh file full path failed. File name:'{item.FileName}', id:'{item.Id}' ");
            }
        }
    }
}
