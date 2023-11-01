namespace Passingwind.Abp.Account;

public static class AccountErrorCodes
{
    public const string LocalLoginDisabled = "Account:Error:0101";

    public const string LoginChangePasswordDisabled = "Account:Error:0102";
    public const string LoginAuthenticatorSetupDisabled = "Account:Error:0102";

    public const string TwoFactorDisabled = "Account:Error:0200";
    public const string TwoFactorCannotDisabled = "Account:Error:0201";
    public const string TwoFactorCodeValidFailed = "Account:Error:0202";
    public const string TwoFactorCanNotChange = "Account:Error:0203";

    public const string AuthenticatorDisabled = "Account:Error:0210";
    public const string ChangePasswordDisabled = "Account:Error:0211";

    public const string TokenInvalid = "Account:Error:0220";

    public const string UserNotFound = "Account:Error:0301";
    public const string UserEmailRequired = "Account:Error:0302";
    public const string UserPhoneNumberRequired = "Account:Error:0303";
    public const string UserRequireConfirmedEmail = "Account:Error:0304";
    public const string UserRequireConfirmedPhoneNumber = "Account:Error:0305";

    public const string UserNotLink = "Account:Error:0401";

    public const string ExternalLoginRemoteError = "Account:Error:0501";
    public const string ExternalLoginInfoAvailable = "Account:Error:0502";
    public const string ExternalLoginUserNotFound = "Account:Error:0503";
}
