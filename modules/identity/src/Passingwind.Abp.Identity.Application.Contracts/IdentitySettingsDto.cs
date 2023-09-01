namespace Passingwind.Abp.Identity;

public class IdentitySettingsDto
{
    public IdentityUserSettingsDto User { get; set; } = null!;
    public IdentityPasswordSettingsDto Password { get; set; } = null!;
    public IdentityLockoutSettingsDto Lockout { get; set; } = null!;
    public IdentitySignInSettingsDto SignIn { get; set; } = null!;
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

public class OrganizationUnitSettingsDto
{
    public int MaxUserMembershipCount { get; set; }
}
