namespace Passingwind.Abp.Account;

public static class AccountErrorCodes
{
    public const string LocalLoginDisabled = "Account:Error:0101";

    public const string TwoFactorDisabled = "Account:Error:0200";
    public const string TwoFactorCannotDisabled = "Account:Error:0201";
    public const string TwoFactorCodeValidFailed = "Account:Error:0202";

    public const string UserNotFound = "Account:Error:0301";

    public const string UserNotLink = "Account:Error:0401";
}
