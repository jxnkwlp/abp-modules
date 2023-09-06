using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Identity;

public class IdentitySettingsDto
{
    [Required]
    public IdentityUserSettingsDto User { get; set; } = null!;
    [Required]
    public IdentityPasswordSettingsDto Password { get; set; } = null!;
    [Required]
    public IdentityLockoutSettingsDto Lockout { get; set; } = null!;
    [Required]
    public IdentitySignInSettingsDto SignIn { get; set; } = null!;
    [Required]
    public IdentityTwofactorSettingsDto Twofactor { get; set; } = null!;
    [Required]
    public OrganizationUnitSettingsDto OrganizationUnit { get; set; } = null!;
}

public class IdentityUserSettingsDto
{
    public bool IsEmailUpdateEnabled { get; set; }
    public bool IsUserNameUpdateEnabled { get; set; }
    public bool RequireUniqueEmail { get; set; }
}

public class IdentityPasswordSettingsDto
{
    public bool RequireDigit { get; set; }
    public bool RequireLowercase { get; set; }
    public bool RequireNonAlphanumeric { get; set; }
    public bool RequireUppercase { get; set; }
    public int RequiredLength { get; set; }
    public int RequiredUniqueChars { get; set; }
    public bool ForceUsersToPeriodicallyChangePassword { get; set; }
    public int PasswordChangePeriodDays { get; set; }
}

public class IdentityLockoutSettingsDto
{
    public bool AllowedForNewUsers { get; set; }
    public int LockoutDuration { get; set; }
    public int MaxFailedAccessAttempts { get; set; }
}

public class IdentitySignInSettingsDto
{
    public bool EnablePhoneNumberConfirmation { get; set; }
    public bool RequireConfirmedEmail { get; set; }
    public bool RequireConfirmedPhoneNumber { get; set; }
}

public class IdentityTwofactorSettingsDto
{
    public bool IsRememberBrowserEnabled { get; set; }
    public IdentityTwofactoryBehaviour TwoFactorBehaviour { get; set; }
    public bool UsersCanChange { get; set; }
    public string? AuthenticatorIssuer { get; set; }
    public string TwoFactorBehaviourDescription => TwoFactorBehaviour.ToString();
}

public class OrganizationUnitSettingsDto
{
    public int MaxUserMembershipCount { get; set; }
}
