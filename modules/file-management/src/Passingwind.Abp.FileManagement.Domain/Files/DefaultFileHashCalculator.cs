using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement.Files;

public class DefaultFileHashCalculator : IFileHashCalculator
{
    public virtual Task<string> GetAsync(byte[] bytes, CancellationToken cancellationToken = default)
    {
        var hash = GetMd5(bytes);

        return Task.FromResult(string.Concat(hash.Select(x => x.ToString("x2"))));
    }

    protected virtual IEnumerable<byte> GetMd5(byte[] fileContent)
    {
        using MD5 md5 = MD5.Create();
        return md5.ComputeHash(fileContent);
    }
}