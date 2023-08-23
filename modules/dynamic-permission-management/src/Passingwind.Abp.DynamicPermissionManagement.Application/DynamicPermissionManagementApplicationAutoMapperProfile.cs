using AutoMapper;
using Passingwind.Abp.DynamicPermissionManagement.Permissions;

namespace Passingwind.Abp.DynamicPermissionManagement;

public class DynamicPermissionManagementApplicationAutoMapperProfile : Profile
{
    public DynamicPermissionManagementApplicationAutoMapperProfile()
    {
        CreateMap<DynamicPermissionDefinition, DynamicPermissionDefinitionDto>();
        CreateMap<DynamicPermissionGroupDefinition, DynamicPermissionGroupDefinitionDto>();
    }
}
