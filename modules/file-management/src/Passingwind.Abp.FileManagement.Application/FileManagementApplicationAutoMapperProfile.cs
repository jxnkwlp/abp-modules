using AutoMapper;
using Passingwind.Abp.FileManagement.Files;

namespace Passingwind.Abp.FileManagement;

public class FileManagementApplicationAutoMapperProfile : Profile
{
    public FileManagementApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<FileContainer, FileContainerDto>();
        CreateMap<File, FileDto>();
    }
}
