using System;

namespace Passingwind.Abp.Identity;

public static class IdentityV2Constants
{
    private const string CookiePrefix = "Identity";

    public static readonly string RequiresChangePasswordScheme = CookiePrefix + ".RequiresChangePassword";
    public static readonly string TwoFactorInitialScheme = CookiePrefix + ".TwoFactorInitial";
}

[Obsolete]
public static class MyIdentityConstants
{
    public static readonly string RequiresChangePasswordScheme = IdentityV2Constants.RequiresChangePasswordScheme;
    public static readonly string TwoFactorInitialScheme = IdentityV2Constants.TwoFactorInitialScheme;
}
