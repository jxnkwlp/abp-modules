using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

[Authorize(IdentityPermissionNamesV2.ClaimTypes.Default)]
public class IdentityClaimTypeAppService : IdentityAppBaseService, IIdentityClaimTypeAppService
{
    protected IIdentityClaimTypeRepository ClaimTypeRepository { get; }
    protected IdentityClaimTypeManager ClaimTypeManager { get; }

    public IdentityClaimTypeAppService(IIdentityClaimTypeRepository claimTypeRepository, IdentityClaimTypeManager claimTypeManager)
    {
        ClaimTypeRepository = claimTypeRepository;
        ClaimTypeManager = claimTypeManager;
    }

    public virtual async Task<ListResultDto<IdentityClaimTypeDto>> GetAllListAsync()
    {
        var list = await ClaimTypeRepository.GetListAsync();

        return new ListResultDto<IdentityClaimTypeDto>(ObjectMapper.Map<List<IdentityClaimType>, List<IdentityClaimTypeDto>>(list));
    }

    public virtual async Task<PagedResultDto<IdentityClaimTypeDto>> GetListAsync(IdentityClaimTypePagedListRequestDto input)
    {
        var count = await ClaimTypeRepository.GetCountAsync(input.Filter);
        var list = await ClaimTypeRepository.GetListAsync(input.Sorting ?? nameof(IdentityClaimType.Name), input.MaxResultCount, input.SkipCount, input.Filter);

        return new PagedResultDto<IdentityClaimTypeDto>(count, ObjectMapper.Map<List<IdentityClaimType>, List<IdentityClaimTypeDto>>(list));
    }

    [Authorize(IdentityPermissionNamesV2.ClaimTypes.Create)]
    public virtual async Task<IdentityClaimTypeDto> CreateAsync(IdentityClaimTypeCreateDto input)
    {
        if (await ClaimTypeRepository.AnyAsync(input.Name))
        {
            throw new BusinessException(IdentityErrorCodesV2.ClaimTypeNameExists).WithData("name", input.Name);
        }

        var entity = new IdentityClaimType(GuidGenerator.Create(), input.Name, input.Required, input.IsStatic, input.Regex, input.RegexDescription, input.Description, input.ValueType);

        entity = await ClaimTypeManager.CreateAsync(entity);

        return ObjectMapper.Map<IdentityClaimType, IdentityClaimTypeDto>(entity);
    }

    [Authorize(IdentityPermissionNamesV2.ClaimTypes.Update)]
    public virtual async Task<IdentityClaimTypeDto> UpdateAsync(Guid id, IdentityClaimTypeUpdateDto input)
    {
        var entity = await ClaimTypeRepository.GetAsync(id);

        if (await ClaimTypeRepository.AnyAsync(input.Name, id))
        {
            throw new BusinessException(IdentityErrorCodesV2.ClaimTypeNameExists).WithData("name", input.Name);
        }

        entity.SetName(input.Name);
        entity.Required = input.Required;
        entity.Regex = input.Regex;
        entity.RegexDescription = input.RegexDescription;
        entity.Description = input.Description;
        entity.ValueType = input.ValueType;

        entity = await ClaimTypeManager.UpdateAsync(entity);

        return ObjectMapper.Map<IdentityClaimType, IdentityClaimTypeDto>(entity);
    }

    [Authorize(IdentityPermissionNamesV2.ClaimTypes.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await ClaimTypeRepository.DeleteAsync(id);
    }
}
