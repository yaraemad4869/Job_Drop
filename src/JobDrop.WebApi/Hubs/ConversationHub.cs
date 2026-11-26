using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace JobDrop.WebApi.Hubs;

[Authorize]
public sealed class ConversationHub : Hub
{
    public async Task JoinConversation(string conversationId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, "conv_" + conversationId);

    public async Task LeaveConversation(string conversationId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, "conv_" + conversationId);

    public async Task ReceiveMessage(object message)
        => await Clients.Group("conv_" + "convId").SendAsync("ReceiveMessage", message);

    public async Task UserTyping(string conversationId, string userCode)
        => await Clients.OthersInGroup("conv_" + conversationId).SendAsync("UserTyping", conversationId, userCode);

    public async Task MessageRead(string conversationId, Guid messageId, DateTime readAtUtc)
        => await Clients.Group("conv_" + conversationId).SendAsync("MessageRead", conversationId, messageId, readAtUtc);
}