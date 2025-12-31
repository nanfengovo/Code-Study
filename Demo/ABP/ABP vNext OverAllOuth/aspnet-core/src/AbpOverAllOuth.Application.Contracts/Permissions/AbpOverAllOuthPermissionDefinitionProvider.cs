using AbpOverAllOuth.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace AbpOverAllOuth.Permissions;

public class AbpOverAllOuthPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AbpOverAllOuthPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(AbpOverAllOuthPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpOverAllOuthResource>(name);
    }
}
