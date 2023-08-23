using AutoMapper;
using Passingwind.Abp.DictionaryManagement.Dictionaries;

namespace Passingwind.Abp.DictionaryManagement;

public class DictionaryManagementApplicationAutoMapperProfile : Profile
{
    public DictionaryManagementApplicationAutoMapperProfile()
    {
        CreateMap<DictionaryGroup, DictionaryGroupDto>();
        CreateMap<DictionaryItem, DictionaryItemDto>();
        CreateMap<DictionaryItem, DictionaryResultDto>();
    }
}
