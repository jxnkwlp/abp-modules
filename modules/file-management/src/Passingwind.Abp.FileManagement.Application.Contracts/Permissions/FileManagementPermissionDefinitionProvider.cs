using Passingwind.Abp.FileManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Passingwind.Abp.FileManagement.Permissions;

public class FileManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1201:Use method chaining.", Justification = "<Pending>")]
    public override void Define(IPermissionDefinitionContext context)
    {
        var fileManagement = context.AddGroup(FileManagementPermissions.GroupName, L($"Permission:{FileManagementPermissions.GroupName}"));

        var fileContainer = fileManagement.AddPermission(FileManagementPermissions.FileContainers.Default, L($"Permission:{FileManagementPermissions.FileContainers.Default}"));
        fileContainer.AddChild(FileManagementPermissions.FileContainers.Create, L("Permission:Create"));
        fileContainer.AddChild(FileManagementPermissions.FileContainers.Update, L("Permission:Update"));
        fileContainer.AddChild(FileManagementPermissions.FileContainers.Delete, L("Permission:Delete"));

        var file = fileManagement.AddPermission(FileManagementPermissions.Files.Default, L($"Permission:{FileManagementPermissions.Files.Default}"));
        file.AddChild(FileManagementPermissions.Files.Delete, L("Permission:Delete"));

        var fileShare = fileManagement.AddPermission(FileManagementPermissions.FileShares.Default, L($"Permission:{FileManagementPermissions.FileShares.Default}"));
        fileShare.AddChild(FileManagementPermissions.FileShares.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FileManagementResource>(name);
    }
}
