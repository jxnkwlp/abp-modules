using System.Collections.Generic;
using System.Security.Claims;

namespace Passingwind.Abp.Identity;

public class ClaimEqualityComparer : IEqualityComparer<Claim>, System.Collections.IEqualityComparer
{
    public bool Equals(Claim? x, Claim? y)
    {
        return x?.Type == y?.Type && x?.Value == y?.Value;
    }

    public int GetHashCode(Claim obj)
    {
        return obj.Type.GetHashCode() & obj.Value.GetHashCode();
    }

    public new bool Equals(object? x, object? y)
    {
        if (x == y)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        if (x is Claim a
            && y is Claim b)
        {
            return Equals(a, b);
        }

        throw new System.ArgumentException("", nameof(x));
    }

    public int GetHashCode(object obj)
    {
        if (obj == null)
        {
            return 0;
        }

        if (obj is Claim x)
        {
            return GetHashCode(x);
        }

        throw new System.ArgumentException("", nameof(obj));
    }
}
