using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Passingwind.Abp.Account.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace Sample;

public class UserLoginEventHandler : ILocalEventHandler<UserLoginEvent>, ITransientDependency
{
    private readonly ILogger<UserLoginEventHandler> _logger;

    public UserLoginEventHandler(ILogger<UserLoginEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleEventAsync(UserLoginEvent eventData)
    {
        _logger.LogInformation($"User with id '{eventData.UserId}' logined by '{eventData.Event}' ");

        return Task.CompletedTask;
    }
}
