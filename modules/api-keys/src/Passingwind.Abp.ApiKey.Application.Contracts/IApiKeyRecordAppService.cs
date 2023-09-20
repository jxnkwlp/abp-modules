using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.ApiKey;

/// <summary>
///  Application service contracts for ApiKeyRecord
/// </summary>
public interface IApiKeyRecordAppService : IApplicationService
{
    /// <summary>
    ///  Get ApiKeyRecord list by paged params
    /// </summary>
    /// <param name="input"></param>
    Task<PagedResultDto<ApiKeyRecordDto>> GetListAsync(ApiKeyRecordListRequestDto input);

    /// <summary>
    ///  Create ApiKeyRecord
    /// </summary>
    /// <param name="input"></param>
    Task<ApiKeyRecordDto> CreateAsync(ApiKeyRecordCreateOrUpdateDto input);

    /// <summary>
    ///  Delete ApiKeyRecord by id
    /// </summary>
    /// <param name="id"></param>
    Task DeleteAsync(Guid id);
}
