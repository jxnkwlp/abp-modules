namespace Passingwind.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class ApplicationConfigurationEndpointOptions
{
    public bool EnableAuth { get; set; } = true;
    public bool EnableClock { get; set; } = true;
    public bool EnableCurrentTenant { get; set; } = true;
    public bool EnableCurrentUser { get; set; } = true;
    public bool EnableExtraProperties { get; set; } = true;
    public bool EnableFeatures { get; set; } = true;
    public bool EnableGlobalFeatures { get; set; } = true;
    public bool EnableLocalization { get; set; } = true;
    public bool EnableMultiTenancy { get; set; } = true;
    public bool EnableObjectExtensions { get; set; } = true;
    public bool EnableSetting { get; set; } = true;
    public bool EnableTiming { get; set; } = true;
}
