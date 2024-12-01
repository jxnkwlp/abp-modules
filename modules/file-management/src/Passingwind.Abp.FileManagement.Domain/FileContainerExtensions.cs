using Volo.Abp.Data;

namespace Passingwind.Abp.FileManagement;

public static class FileContainerExtensions
{
    public static FileContainer SetIsDefault(this FileContainer entity, bool value)
    {
        return entity.SetProperty("IsDefault", value ? 1 : 0);
    }

    public static bool GetIsDefault(this FileContainer entity)
    {
        return entity.GetProperty<int>("IsDefault") == 1;
    }
}
