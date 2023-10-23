using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.IdentityClient.Identity;

public interface IIdentityClaimMapRunner
{
    Task<List<Claim>> RunAsync(IEnumerable<Claim> source, List<IdentityClientClaimMap> claimMaps, CancellationToken cancellationToken = default);
}
