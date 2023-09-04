namespace Passingwind.Abp.Identity.Settings;

public static class IdentitySettingNamesV2
{
    public const string GroupName = "Identity";

    public static class User
    {
        private const string Prefix = "Abp.Identity.User";

        public const string RequireUniqueEmail = Prefix + ".RequireUniqueEmail";
    }

    public static class Twofactor
    {
        public const string Prefix = GroupName + ".Twofactor";

        public const string IsRememberBrowserEnabled = Prefix + ".IsRememberBrowserEnabled";
        public const string TwoFactorBehaviour = Prefix + ".TwoFactorBehaviour";
        public const string UsersCanChange = Prefix + ".UsersCanChange";
    }
}
