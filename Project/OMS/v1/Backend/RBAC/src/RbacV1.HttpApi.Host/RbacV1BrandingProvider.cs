using Microsoft.Extensions.Localization;
using RbacV1.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace RbacV1;

[Dependency(ReplaceServices = true)]
public class RbacV1BrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<RbacV1Resource> _localizer;

    public RbacV1BrandingProvider(IStringLocalizer<RbacV1Resource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
