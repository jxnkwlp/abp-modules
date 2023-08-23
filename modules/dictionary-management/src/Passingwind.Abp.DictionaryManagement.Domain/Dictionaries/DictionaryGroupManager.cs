using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryGroupManager : DomainService
{
    private readonly IDictionaryGroupRepository _dictionaryGroupRepository;

    public DictionaryGroupManager(IDictionaryGroupRepository dictionaryGroupRepository)
    {
        _dictionaryGroupRepository = dictionaryGroupRepository;
    }
}
