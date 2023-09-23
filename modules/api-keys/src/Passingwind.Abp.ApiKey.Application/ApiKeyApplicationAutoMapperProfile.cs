using AutoMapper;

namespace Passingwind.Abp.ApiKey;

public class ApiKeyApplicationAutoMapperProfile : Profile
{
    public ApiKeyApplicationAutoMapperProfile()
    {
        CreateMap<ApiKeyRecord, ApiKeyRecordDto>();
    }
}
