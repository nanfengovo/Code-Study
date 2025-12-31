using Volo.Abp.Settings;

namespace AbpOverAllOuth.Settings;

public class AbpOverAllOuthSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AbpOverAllOuthSettings.MySetting1));
    }
}
