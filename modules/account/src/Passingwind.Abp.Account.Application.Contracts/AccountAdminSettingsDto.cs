using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class AccountAdminSettingsDto
{
    [Required]
    public AccountGeneralSettingsDto General { get; set; } = null!;
    [Required]
    public AccountCaptchaSettingsDto Captcha { get; set; } = null!;
    [Required]
    public AccountRecaptchaSettingsDto Recaptcha { get; set; } = null!;
    [Required]
    public AccountExternalLoginSettingsDto ExternalLogin { get; set; } = null!;
}

public class AccountGeneralSettingsDto
{
    public bool IsSelfRegistrationEnabled { get; set; }
    public bool EnableLocalLogin { get; set; }

    public bool EnableChangePasswordOnProfile { get; set; }
    public bool EnableChangePasswordOnLogin { get; set; }
    public bool EnableAuthenticatorSetupOnLogin { get; set; }
}

public class AccountExternalLoginSettingsDto
{
    public bool AutoCreateUser { get; set; }
    public bool BypassTwofactory { get; set; }
}

public class AccountCaptchaSettingsDto
{
    public bool EnableOnLogin { get; set; }
    public bool EnableOnRegistration { get; set; }
}

public class AccountRecaptchaSettingsDto
{
    public double Score { get; set; }
    public string? SiteKey { get; set; }
    public string? SiteSecret { get; set; }
    public string? VerifyBaseUrl { get; set; }
    public int Version { get; set; }
}
