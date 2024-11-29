using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passingwind.Abp.AuditLogging.Options;
using Volo.Abp.AuditLogging;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.AuditLogging.Cleanup.Providers;

[ExposeServices(typeof(IAuditLogCleanupSaveToFileProvider))]
public class DefaultSaveToFileProvider : IAuditLogCleanupSaveToFileProvider, IScopedDependency
{
    protected ILogger<DefaultSaveToFileProvider> Logger { get; }
    protected IAuditLogRepository AuditLogRepository { get; }
    protected IHostEnvironment HostEnvironment { get; }
    protected AuditLoggingCleanupOptions AuditLoggingCleanupOptions { get; }

    public DefaultSaveToFileProvider(
        ILogger<DefaultSaveToFileProvider> logger,
        IAuditLogRepository auditLogRepository,
        IHostEnvironment hostEnvironment,
        IOptions<AuditLoggingCleanupOptions> auditLoggingCleanupOptions)
    {
        Logger = logger;
        AuditLogRepository = auditLogRepository;
        HostEnvironment = hostEnvironment;
        AuditLoggingCleanupOptions = auditLoggingCleanupOptions.Value;
    }

    public virtual async Task SaveToFileAsync(DateTime endTime, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(AuditLoggingCleanupOptions.BackupToDir))
        {
            Logger.LogWarning("The options 'BackupToDir' value is empty.");
            return;
        }

        var rootDir = AuditLoggingCleanupOptions.BackupToDir!;
        if (!Path.IsPathRooted(rootDir))
        {
            rootDir = Path.Combine(HostEnvironment.ContentRootPath, rootDir);
        }

        //
        const int perCount = 1000;
        int index = 0;

        while (true)
        {
            int skipCount = index * perCount;

            var list = await AuditLogRepository.GetListAsync(
                maxResultCount: perCount,
                skipCount: skipCount,
                endTime: endTime,
                includeDetails: true,
                cancellationToken: cancellationToken);

            if (list.Count == 0)
                break;

            SaveToFile(index + 1, rootDir, list);

            index++;
        }

        // delete source data
        await AuditLogRepository.DeleteDirectAsync(x => x.ExecutionTime <= endTime, cancellationToken);
    }

    protected void SaveToFile(int index, string rootDir, IEnumerable<AuditLog> logs)
    {
        var saveDir = Path.Combine(rootDir, DateTime.Now.ToString("yyyyMMdd-HHmm"));

        if (!Directory.Exists(saveDir))
        {
            Directory.CreateDirectory(saveDir);
        }

        var filePath = Path.Combine(saveDir, $"{index}.data");

        try
        {
            using var writer = new StreamWriter(filePath, true, Encoding.UTF8, 65536);

            foreach (var item in logs)
            {
                var jsonText = JsonSerializer.Serialize(item);

                writer.WriteLine(jsonText);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Write file failed.");
            throw;
        }
    }
}
