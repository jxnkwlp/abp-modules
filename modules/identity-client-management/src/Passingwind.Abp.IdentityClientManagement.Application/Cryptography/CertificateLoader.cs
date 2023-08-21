using System.Security.Cryptography.X509Certificates;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.IdentityClientManagement.Cryptography;

public class CertificateLoader : ICertificateLoader, ISingletonDependency
{
    public X509Certificate2 Create(string certPem, string? keyPem = null)
    {
        if (!string.IsNullOrWhiteSpace(keyPem))
        {
            return X509Certificate2.CreateFromPem(certPem, keyPem);
        }

        return X509Certificate2.CreateFromPem(certPem);
    }
}
