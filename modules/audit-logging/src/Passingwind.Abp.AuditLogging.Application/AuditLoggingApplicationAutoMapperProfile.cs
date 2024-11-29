using AutoMapper;
using Volo.Abp.AuditLogging;

namespace Passingwind.Abp.AuditLogging;

public class AuditLoggingApplicationAutoMapperProfile : Profile
{
    public AuditLoggingApplicationAutoMapperProfile()
    {
        CreateMap<AuditLog, AuditLogDto>();
        CreateMap<AuditLogAction, AuditLogActionDto>();
        CreateMap<EntityChange, EntityChangeDto>();
        CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();
    }
}
