//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Identity;
//using Volo.Abp.Identity;
//using IdentityUser = Volo.Abp.Identity.IdentityUser;

//namespace Passingwind.Abp.Identity;

///// <summary>
///// Used for authenticator code verification.
///// </summary>
//public class AuthenticatorTokenProviderV2 : AuthenticatorTokenProvider<IdentityUser>
//{
//    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<IdentityUser> manager, IdentityUser user)
//    {
//        return Task.FromResult(true);
//    }
//}
