using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryItemManager : DomainService
{
    private readonly IDictionaryItemRepository _dictionaryItemRepository;

    public DictionaryItemManager(IDictionaryItemRepository dictionaryItemRepository)
    {
        _dictionaryItemRepository = dictionaryItemRepository;
    }
}
