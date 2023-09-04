namespace Passingwind.Abp.Account.Settings;

public class AccountGeneralSettings
{
    public bool IsSelfRegistrationEnabled { get; set; }
    public bool EnableLocalLogin { get; set; }
}

public class AccountCaptchaSettings
{
    public bool EnableOnLogin { get; set; }
    public bool EnableOnRegistration { get; set; }
}

public class AccountRecaptchaSettings
{
    public double Score { get; set; }
    public string? SiteKey { get; set; }
    public string? SiteSecret { get; set; }
    public string? VerifyBaseUrl { get; set; }
    public int Version { get; set; }
}
