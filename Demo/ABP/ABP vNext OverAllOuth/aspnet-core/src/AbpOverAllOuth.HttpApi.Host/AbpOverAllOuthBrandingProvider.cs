using Microsoft.Extensions.Localization;
using AbpOverAllOuth.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace AbpOverAllOuth;

[Dependency(ReplaceServices = true)]
public class AbpOverAllOuthBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<AbpOverAllOuthResource> _localizer;

    public AbpOverAllOuthBrandingProvider(IStringLocalizer<AbpOverAllOuthResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
