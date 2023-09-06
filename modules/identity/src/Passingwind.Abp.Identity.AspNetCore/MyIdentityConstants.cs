namespace Passingwind.Abp.Identity;

public static class MyIdentityConstants
{
    private const string CookiePrefix = "Identity";

    public static readonly string RequiresChangePasswordScheme = CookiePrefix + ".RequiresChangePassword";
    public static readonly string TwoFactorInitialScheme = CookiePrefix + ".TwoFactorInitial";
}
