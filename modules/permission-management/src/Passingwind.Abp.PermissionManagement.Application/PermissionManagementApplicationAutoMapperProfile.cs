using AutoMapper;
using Passingwind.Abp.PermissionManagement.DynamicPermissions;

namespace Passingwind.Abp.PermissionManagement;

public class PermissionManagementApplicationAutoMapperProfile : Profile
{
    public PermissionManagementApplicationAutoMapperProfile()
    {
        CreateMap<DynamicPermissionDefinition, DynamicPermissionDefinitionDto>();
        CreateMap<DynamicPermissionGroupDefinition, DynamicPermissionGroupDefinitionDto>();
    }
}
