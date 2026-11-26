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
            await Groups.AddToGroupAsync(Context.ConnectionId, "user_" + userId);
        await base.OnConnectedAsync();
    }

    public async Task DropDeliveredLive(object notification)
        => await Clients.Group("company_" + "companyId").SendAsync("DropDeliveredLive", notification);

    public async Task SlaTimerTicked(object tick)
        => await Clients.Caller.SendAsync("SlaTimerTicked", tick);

    public async Task DropRefundedLive(object refund)
        => await Clients.User(Context.UserIdentifier!).SendAsync("DropRefundedLive", refund);
}