using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.ApiKey;

/// <summary>
///  Application service for ApiKeyRecord
/// </summary>
[Authorize]
public class ApiKeyRecordAppService : ApiKeyAppService, IApiKeyRecordAppService
{
    private readonly IApiKeyRecordManager _apiKeyRecordManager;
    private readonly IApiKeyRecordRepository _apiKeyRecordRepository;

    public ApiKeyRecordAppService(IApiKeyRecordManager apiKeyRecordManager, IApiKeyRecordRepository apiKeyRecordRepository)
    {
        _apiKeyRecordManager = apiKeyRecordManager;
        _apiKeyRecordRepository = apiKeyRecordRepository;
    }

    /// <inheritdoc/>
    public virtual async Task<PagedResultDto<ApiKeyRecordDto>> GetListAsync(ApiKeyRecordListRequestDto input)
    {
        var count = await _apiKeyRecordRepository.GetCountAsync(userId: CurrentUser.Id!);
        var list = await _apiKeyRecordRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, userId: CurrentUser.Id!, sorting: nameof(ApiKeyRecord.CreationTime) + " desc");

        var dtoList = ObjectMapper.Map<List<ApiKeyRecord>, List<ApiKeyRecordDto>>(list);

        dtoList.ForEach(item => item.Secret = string.Empty);

        return new PagedResultDto<ApiKeyRecordDto>()
        {
            Items = dtoList,
            TotalCount = count,
        };
    }

    /// <inheritdoc/>
    public virtual async Task<ApiKeyRecordDto> CreateAsync(ApiKeyRecordCreateOrUpdateDto input)
    {
        var value = await _apiKeyRecordManager.GenerateValueAsync(CurrentUser.Id!.Value);

        var entity = new ApiKeyRecord(
            id: GuidGenerator.Create(),
            userId: CurrentUser.Id!.Value,
            name: input.Name,
            secret: value,
            expirationTime: input.ExpirationTime,
            tenantId: CurrentTenant.Id);

        input.MapExtraPropertiesTo(entity);

        await _apiKeyRecordRepository.InsertAsync(entity);

        return ObjectMapper.Map<ApiKeyRecord, ApiKeyRecordDto>(entity);
    }

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await _apiKeyRecordRepository.GetAsync(id);

        if (entity.UserId == CurrentUser.Id)
            await _apiKeyRecordRepository.DeleteAsync(entity);
    }
}
