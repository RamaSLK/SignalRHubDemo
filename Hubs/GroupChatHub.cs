using Microsoft.AspNetCore.SignalR;

namespace SignalRHubDemo.Hubs;

public class GroupChatHub : Hub
{
    public async Task SendMessageToGroup(string groupName, string user, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
    }

    public async Task AddToGroup(string groupName)
    {
        try
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("GroupResponse",$"Joined group {groupName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await Clients.Caller.SendAsync("GroupResponse", $"Could not join {groupName}");
        }
    }

    public async Task RemoveFromGroup(string groupName)
    {
        try
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("GroupResponse", $"Left group {groupName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await Clients.Caller.SendAsync("GroupResponse", $"Could not leave {groupName}");
        }
    }
}