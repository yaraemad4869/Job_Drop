using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace JobDrop.WebApi.Hubs;

[Authorize]
public sealed class ConversationHub : Hub
{
    public async Task JoinConversation(string conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"conv_{conversationId}");
    }

    public async Task LeaveConversation(string conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conv_{conversationId}");
    }

    public async Task SendMessage(string conversationId, string messageJson)
    {
        await Clients.Group($"conv_{conversationId}").SendAsync("ReceiveMessage", messageJson);
    }

    public async Task SendTyping(string conversationId, string userCode)
    {
        await Clients.OthersInGroup($"conv_{conversationId}").SendAsync("UserTyping", conversationId, userCode);
    }

    public async Task SendReadReceipt(string conversationId, string messageId, DateTime readAtUtc)
    {
        await Clients.OthersInGroup($"conv_{conversationId}").SendAsync("MessageRead", conversationId, messageId, readAtUtc);
    }
}