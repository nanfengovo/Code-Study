using Volo.Abp.Settings;

namespace RbacV1.Settings;

public class RbacV1SettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(RbacV1Settings.MySetting1));
    }
}
