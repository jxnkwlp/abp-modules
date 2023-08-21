using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;

namespace Passingwind.Abp.IdentityClientManagement.Identity;

public interface IIdentityClaimMapRunner
{
    Task<List<Claim>> RunAsync(IEnumerable<Claim> source, List<IdentityClientClaimMap> claimMaps, CancellationToken cancellationToken = default);
}
