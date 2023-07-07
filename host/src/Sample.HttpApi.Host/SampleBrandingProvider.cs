using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Sample;

[Dependency(ReplaceServices = true)]
public class SampleBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Sample";
}
