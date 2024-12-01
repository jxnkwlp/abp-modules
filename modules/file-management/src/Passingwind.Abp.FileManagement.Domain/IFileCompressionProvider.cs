using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement;

public interface IFileCompressionProvider
{
    Task<Stream> CompressAsync(IEnumerable<FileDescriptor> files, string? password = null, CancellationToken cancellationToken = default);
    Task<ImmutableArray<FileDescriptor>> DecompressAsync(Stream fileStream, CancellationToken cancellationToken = default);
}
