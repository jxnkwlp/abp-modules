namespace Passingwind.Abp.Account.Settings;

public static class AccountSettingNames
{
    public const string GroupName = "Account";

    public static class General
    {
        public const string IsSelfRegistrationEnabled = "Abp.Account.IsSelfRegistrationEnabled";
        public const string EnableLocalLogin = "Abp.Account.EnableLocalLogin";
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
