using Passingwind.Abp.ApiKey.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Passingwind.Abp.ApiKey.Permissions;

public class ApiKeyPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ApiKeyPermissions.GroupName, L("Permission:ApiKey"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ApiKeyResource>(name);
    }
}
