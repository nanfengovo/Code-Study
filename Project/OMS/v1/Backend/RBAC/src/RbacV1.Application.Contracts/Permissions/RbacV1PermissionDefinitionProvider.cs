using RbacV1.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace RbacV1.Permissions;

public class RbacV1PermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(RbacV1Permissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(RbacV1Permissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<RbacV1Resource>(name);
    }
}
