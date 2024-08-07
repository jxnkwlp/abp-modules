﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

[Area(DictionaryManagementRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = DictionaryManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/dictionaries")]
public class DictionaryController : DictionaryManagementController, IDictionaryAppService
{
    private readonly IDictionaryAppService _service;

    public DictionaryController(IDictionaryAppService service)
    {
        _service = service;
    }

    [HttpGet("{name}")]
    public virtual Task<DictionaryResultDto> GetAsync(string name)
    {
        return _service.GetAsync(name);
    }

    [HttpGet("groups")]
    public virtual Task<DictionaryGroupListResultDto> GetGroupListAsync(string? parentName = null)
    {
        return _service.GetGroupListAsync(parentName);
    }

    [HttpGet("groups/{groupName}")]
    public virtual Task<DictionaryListResultDto> GetListByGroupAsync(string groupName)
    {
        return _service.GetListByGroupAsync(groupName);
    }
}
