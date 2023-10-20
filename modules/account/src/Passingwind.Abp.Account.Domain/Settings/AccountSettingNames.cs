namespace Passingwind.Abp.Account.Settings;

public static class AccountSettingNames
{
    public const string GroupName = "Account";

    /// <summary>
    ///  General settings
    /// </summary>
    public static class General
    {
        public const string Prefix = "Abp.Account";

        public const string IsSelfRegistrationEnabled = Prefix + ".IsSelfRegistrationEnabled";
        public const string EnableLocalLogin = Prefix + ".EnableLocalLogin";

        /// <summary>
        ///  Allow user change password on profile
        /// </summary>
        public const string EnableChangePasswordOnProfile = GroupName + ".EnableChangePasswordOnProfile";

        /// <summary>
        ///  Allow user change password on login
        /// </summary>
        public const string EnableChangePasswordOnLogin = GroupName + ".EnableChangePasswordOnLogin";

        /// <summary>
        ///  Allow user setup authenticator on login
        /// </summary>
        public const string EnableAuthenticatorSetupOnLogin = GroupName + ".EnableAuthenticatorSetupOnLogin";

        //public const string AllowTwofactorProviders = GroupName + ".AllowTwofactorProviders";
    }

    public static class SecurityLogs
    {
        public const string Prefix = GroupName + ".SecurityLogs";

        public const string AllowUserDelete = Prefix + ".AllowUserDelete";
    }

    public static class Captcha
    {
        public const string Prefix = GroupName + ".Captcha";

        public const string EnableOnLogin = Prefix + ".EnableOnLogin";
        public const string EnableOnRegistration = Prefix + ".EnableOnRegistration";
    }

    public static class Recaptcha
    {
        public const string Prefix = GroupName + ".Recaptcha";

        public const string Score = Prefix + ".Score";
        public const string SiteKey = Prefix + ".SiteKey";
        public const string SiteSecret = Prefix + ".SiteSecret";
        public const string VerifyBaseUrl = Prefix + ".VerifyBaseUrl";
        public const string Version = Prefix + ".Version";
    }
}
