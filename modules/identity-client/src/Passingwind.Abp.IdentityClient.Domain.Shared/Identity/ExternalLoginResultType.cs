namespace Passingwind.Abp.IdentityClient.Identity;

public enum ExternalLoginResultType
{
    Success = 1,
    NotAllowed = 2,
    LockedOut = 3,
    RequiresTwoFactor = 4,
}
