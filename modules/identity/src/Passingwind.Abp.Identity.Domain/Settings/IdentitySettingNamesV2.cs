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

        public const string AuthenticatorEnabled = Prefix + ".Authenticator.Enabled";
        public const string AuthenticatorIssuer = Prefix + ".Authenticator.Issuer";
    }

    //public static class Token
    //{
    //    public const string Prefix = GroupName + ".Token";

    //    public const string RequireConfirmedEmail = Prefix + ".RequireConfirmedEmail";
    //    public const string RequireConfirmedPhoneNumber = Prefix + ".RequireConfirmedPhoneNumber";
    //}
}
