using System.Security.Cryptography.X509Certificates;

namespace Passingwind.Abp.IdentityClient.Cryptography;

public interface ICertificateLoader
{
    X509Certificate2 Create(string certPem, string? keyPem = null);
}
