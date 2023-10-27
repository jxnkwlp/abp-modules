using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.IdentityClient;

public interface ISaml2AppService : IApplicationService
{
    /// <summary>
    ///  Response metadata content
    /// </summary>
    /// <param name="baseUri"></param>
    /// <param name="name"></param>
    Task<string> GetMetadataDescriptorAsync(Uri baseUri, string name);

    /// <summary>
    ///  Receives a logout request and send a response.
    /// </summary>
    /// <param name="name"></param>
    Task<string> LogoutAsync(string name);

    ///// <summary>
    /////  After successfully or failing logout the logged out method receive the response.
    ///// </summary>
    ///// <param name="name"></param>
    //Task LoggedOutAsync(string name);

    ///// <summary>
    /////  Receives a Single Logout request and send a response.
    ///// </summary>
    ///// <param name="name"></param>
    //Task<string> SingleLogoutAsync(string name);
}
