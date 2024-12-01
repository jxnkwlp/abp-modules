using Volo.Abp.Domain.Entities;

namespace Passingwind.Abp.FileManagement;

public class FileContainerAdminUpdateDto : FileContainerCreateOrUpdateBasicDto, IHasConcurrencyStamp
{
    public virtual string ConcurrencyStamp { get; set; } = null!;
}
