using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.AuditLogging;

public interface IEntityChangeAppService : IApplicationService
{
    Task<ListResultDto<EntityChangeDto>> GetListAsync(EntityChangeListRequestDto input);

    Task<EntityChangeDto> GetAsync(Guid id);
}
