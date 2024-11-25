using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.IdentityClient.Identity;

public class IdentityClaimMapRunner : IIdentityClaimMapRunner, ISingletonDependency
{
    public virtual Task<List<Claim>> RunAsync(IEnumerable<Claim> source, List<IdentityClientClaimMap> claimMaps, CancellationToken cancellationToken = default)
    {
        var cliams = source.ToList();

        foreach (var claimMap in claimMaps)
        {
            switch (claimMap.Action)
            {
                case IdentityClientClaimMapAction.AddIfNotExists:
                    {
                        if (!cliams.Any(x => x.Type == claimMap.ClaimType))
                        {
                            var claim = CreateClaim(source, claimMap);
                            if (claim != null)
                            {
                                cliams.Add(claim);
                            }
                        }
                        break;
                    }
                case IdentityClientClaimMapAction.AddOrUpdate:
                    {
                        var claim = CreateClaim(source, claimMap);
                        if (claim != null)
                        {
                            cliams.RemoveAll(x => x.Type == claim.Type);
                            cliams.Add(claim);
                        }
                        break;
                    }
                case IdentityClientClaimMapAction.Append:
                    {
                        var claim = CreateClaim(source, claimMap);
                        if (claim != null)
                        {
                            cliams.Add(claim);
                        }
                        break;
                    }
                case IdentityClientClaimMapAction.Remove:
                    {
                        cliams.RemoveAll(x => x.Type == claimMap.ClaimType);
                        break;
                    }
            }
        }

        return Task.FromResult(cliams);
    }

    protected Claim? CreateClaim(IEnumerable<Claim> source, IdentityClientClaimMap map)
    {
        if (!string.IsNullOrWhiteSpace(map.RawValue))
        {
            return new Claim(map.ClaimType, map.RawValue);
        }
        else if (!string.IsNullOrWhiteSpace(map.ValueFromType))
        {
            var value = source.FirstOrDefault(x => x.Type == map.ValueFromType);
            if (!string.IsNullOrWhiteSpace(value?.Value))
            {
                return new Claim(map.ClaimType, value.Value);
            }
        }
        return null;
    }
}
