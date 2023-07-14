using Passingwind.Abp.FileManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Passingwind.Abp.FileManagement.Permissions;

public class FileManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var fileManagement = context.AddGroup(FileManagementPermissions.GroupName, L($"Permission:{FileManagementPermissions.GroupName}"));

        var fileContainer = fileManagement.AddPermission(FileManagementPermissions.FileContainer.Default, L($"Permission:{FileManagementPermissions.FileContainer.Default}"));
        fileContainer.AddChild(FileManagementPermissions.FileContainer.Update, L($"Permission:{FileManagementPermissions.FileContainer.Update}"));
        fileContainer.AddChild(FileManagementPermissions.FileContainer.Delete, L($"Permission:{FileManagementPermissions.FileContainer.Delete}"));

        var file = fileManagement.AddPermission(FileManagementPermissions.File.Default, L($"Permission:{FileManagementPermissions.File.Default}"));
        file.AddChild(FileManagementPermissions.File.Delete, L($"Permission:{FileManagementPermissions.File.Delete}"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FileManagementResource>(name);
    }
}
