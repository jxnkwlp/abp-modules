using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SharpCompress.Archives.Zip;
using SharpCompress.Readers;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.FileManagement;

/// <summary>
///  Default implement for <see cref="IFileCompressionProvider"/>
/// </summary>
/// <remarks>
///  documents:
///  https://github.com/adamhathcock/sharpcompress/blob/master/USAGE.md
///  https://github.com/adamhathcock/sharpcompress/blob/master/FORMATS.md
/// </remarks>
public class DefaultFileCompressionProvider : IFileCompressionProvider, ISingletonDependency
{
    public Task<Stream> CompressAsync(IEnumerable<FileDescriptor> files, string? password = null, CancellationToken cancellationToken = default)
    {
        var ms = new MemoryStream();
        using (var archive = ZipArchive.Create())
        {
            foreach (var item in files)
            {
                archive.AddEntry(item.FileName, item.Stream, modified: item.Created);
            }

            var options = new SharpCompress.Writers.WriterOptions(SharpCompress.Common.CompressionType.Deflate)
            {
                LeaveStreamOpen = true,
                ArchiveEncoding = new SharpCompress.Common.ArchiveEncoding()
                {
                    Default = System.Text.Encoding.UTF8,
                }
            };

            archive.SaveTo(ms, options);
        }

        // TODO: password

        ms.Seek(0, SeekOrigin.Begin);
        return Task.FromResult<Stream>(ms);
    }

    public Task<ImmutableArray<FileDescriptor>> DecompressAsync(Stream fileStream, CancellationToken cancellationToken)
    {
        if (fileStream is null)
        {
            throw new ArgumentNullException(nameof(fileStream));
        }

        var list = new List<FileDescriptor>();

        using (var reader = ReaderFactory.Open(fileStream))
        {
            while (reader.MoveToNextEntry())
            {
                if (!reader.Entry.IsDirectory)
                {
                    var fileItem = new FileDescriptor()
                    {
                        FileName = reader.Entry.Key,
                        Stream = new MemoryStream(),
                    };

                    using (var entryStream = reader.OpenEntryStream())
                    {
                        entryStream.CopyTo(fileItem.Stream);
                    }

                    list.Add(fileItem);
                }
            }
        }

        return Task.FromResult(ImmutableArray.CreateRange(list));
    }
}
