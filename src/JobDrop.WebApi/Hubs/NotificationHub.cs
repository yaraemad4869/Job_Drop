using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace JobDrop.WebApi.Hubs;

[Authorize]
public sealed class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        await base.OnConnectedAsync();
    }

    public async Task SendDropDeliveredLive(string dropNotificationJson)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Clients.Group($"user_{userId}").SendAsync("DropDeliveredLive", dropNotificationJson);
        }
    }

    public async Task SendSlaTimerTicked(string slaTickJson)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Clients.Group($"user_{userId}").SendAsync("SlaTimerTicked", slaTickJson);
        }
    }
}