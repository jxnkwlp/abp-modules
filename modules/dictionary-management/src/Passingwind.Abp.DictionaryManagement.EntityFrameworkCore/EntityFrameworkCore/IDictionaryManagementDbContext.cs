using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.DictionaryManagement.Dictionaries;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.DictionaryManagement.EntityFrameworkCore;

[ConnectionStringName(DictionaryManagementDbProperties.ConnectionStringName)]
public interface IDictionaryManagementDbContext : IEfCoreDbContext
{
    DbSet<DictionaryGroup> DictionaryGroups { get; }
    DbSet<DictionaryItem> DictionaryItems { get; }
}
