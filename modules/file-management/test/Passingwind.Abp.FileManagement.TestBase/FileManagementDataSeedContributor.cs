using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.FileManagement;

public class FileManagementDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly ICurrentTenant _currentTenant;
    private readonly IFileContainerRepository _fileContainerRepository;
    private readonly IFileRepository _fileRepository;

    public FileManagementDataSeedContributor(
        IGuidGenerator guidGenerator, ICurrentTenant currentTenant, IFileContainerRepository fileContainerRepository, IFileRepository fileRepository)
    {
        _guidGenerator = guidGenerator;
        _currentTenant = currentTenant;
        _fileContainerRepository = fileContainerRepository;
        _fileRepository = fileRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        /* Instead of returning the Task.CompletedTask, you can insert your test data
         * at this point!
         */

        using (_currentTenant.Change(context?.TenantId))
        {
            // container
            await _fileContainerRepository.InsertAsync(new FileContainer(Guid.Parse("eaf86134-7148-490d-a08f-1a6abddf9f30"), "test-01", Files.FileAccessMode.Anonymous));
            await _fileContainerRepository.InsertAsync(new FileContainer(Guid.Parse("eaf86134-7148-490d-a08f-1a6abddf9f31"), "test-02", Files.FileAccessMode.Authorized));
            await _fileContainerRepository.InsertAsync(new FileContainer(Guid.Parse("eaf86134-7148-490d-a08f-1a6abddf9f32"), "test-03", Files.FileAccessMode.Private));

            // files
            await _fileRepository.InsertAsync(new FileItem(_guidGenerator.Create(), Guid.Parse("eaf86134-7148-490d-a08f-1a6abddf9f30"), true, "dir01", "dir01"));
            await _fileRepository.InsertAsync(new FileItem(_guidGenerator.Create(), Guid.Parse("eaf86134-7148-490d-a08f-1a6abddf9f30"), true, "dir02", "dir02"));

            // await _fileRepository.InsertAsync(new FileItem(_guidGenerator.Create(), Guid.Parse("eaf86134-7148-490d-a08f-1a6abddf9f30"), false, "file01.txt", "file01.txt"));
        }
    }
}
