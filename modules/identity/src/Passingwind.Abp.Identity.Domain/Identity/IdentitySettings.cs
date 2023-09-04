namespace Passingwind.Abp.Identity;

public class IdentityUserSettings
{
    public bool IsEmailUpdateEnabled { get; set; }
    public bool IsUserNameUpdateEnabled { get; set; }
    public bool RequireUniqueEmail { get; set; }
}

public class IdentityPasswordSettings
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

public class IdentityLockoutSettings
{
    public bool AllowedForNewUsers { get; set; }
    public int LockoutDuration { get; set; }
    public int MaxFailedAccessAttempts { get; set; }
}

public class IdentitySignInSettings
{
    public bool EnablePhoneNumberConfirmation { get; set; }
    public bool RequireConfirmedEmail { get; set; }
    public bool RequireConfirmedPhoneNumber { get; set; }
}

public class OrganizationUnitSettings
{
    public int MaxUserMembershipCount { get; set; }
}

public class IdentityTwofactorSettings
{
    public bool IsRememberBrowserEnabled { get; set; }
    public IdentityTwofactoryBehaviour TwoFactorBehaviour { get; set; }
    public bool UsersCanChange { get; set; }
}
