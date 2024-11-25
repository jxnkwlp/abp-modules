using Volo.Abp.Authorization.Permissions;

namespace Sample.Permissions;

public class SamplePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SamplePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(SamplePermissions.MyPermission1, L("Permission:MyPermission1"));
    }
}
