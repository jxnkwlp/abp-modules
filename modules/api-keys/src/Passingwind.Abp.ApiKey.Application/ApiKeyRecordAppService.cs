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
    protected IApiKeyRecordManager ApiKeyRecordManager { get; }
    protected IApiKeyRecordRepository ApiKeyRecordRepository { get; }

    public ApiKeyRecordAppService(IApiKeyRecordManager apiKeyRecordManager, IApiKeyRecordRepository apiKeyRecordRepository)
    {
        ApiKeyRecordManager = apiKeyRecordManager;
        ApiKeyRecordRepository = apiKeyRecordRepository;
    }

    /// <inheritdoc/>
    public virtual async Task<PagedResultDto<ApiKeyRecordDto>> GetListAsync(ApiKeyRecordListRequestDto input)
    {
        var count = await ApiKeyRecordRepository.GetCountAsync(userId: CurrentUser.Id!);
        var list = await ApiKeyRecordRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, userId: CurrentUser.Id!, sorting: nameof(ApiKeyRecord.CreationTime) + " desc");

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
        var value = await ApiKeyRecordManager.GenerateValueAsync(CurrentUser.Id!.Value);

        var entity = new ApiKeyRecord(
            id: GuidGenerator.Create(),
            userId: CurrentUser.Id!.Value,
            name: input.Name,
            secret: value,
            expirationTime: input.ExpirationTime,
            tenantId: CurrentTenant.Id);

        input.MapExtraPropertiesTo(entity);

        await ApiKeyRecordRepository.InsertAsync(entity);

        return ObjectMapper.Map<ApiKeyRecord, ApiKeyRecordDto>(entity);
    }

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await ApiKeyRecordRepository.GetAsync(id);

        if (entity.UserId == CurrentUser.Id)
            await ApiKeyRecordRepository.DeleteAsync(entity);
    }
}
