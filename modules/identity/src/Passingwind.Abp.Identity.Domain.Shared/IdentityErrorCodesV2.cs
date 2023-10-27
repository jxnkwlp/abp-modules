namespace Passingwind.Abp.Identity;

public static class IdentityErrorCodesV2
{
    /*
    * 01: user
    * 02: role
    * 03: orgs
    * 04: claim type
    */

    public const string UserExternalLoginUserNameNotFound = "Identity:Error:01050";

    public const string ClaimTypeNameExists = "Identity:Error:04001";
}
