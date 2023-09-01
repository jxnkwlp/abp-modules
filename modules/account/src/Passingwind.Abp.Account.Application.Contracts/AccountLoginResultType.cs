namespace Passingwind.Abp.Account;

public enum AccountLoginResultType : byte
{
    Success = 1,
    InvalidUserNameOrPassword = 2,
    NotAllowed = 3,
    LockedOut = 4,
    RequiresTwoFactor = 5,
    RequiresChangePassword = 6,
}
