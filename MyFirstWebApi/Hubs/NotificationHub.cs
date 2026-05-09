using System;
using Microsoft.AspNetCore.SignalR;

namespace MyFirstWebApi.Hubs;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        System.Console.WriteLine($"Client Connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        System.Console.WriteLine($"Client Disconnected: {Context.ConnectionId}");
    }

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients
            .Group(groupName)
            .SendAsync("ReceivedMessage", "System", $"{Context.ConnectionId} joined {groupName}");
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients
            .Group(groupName)
            .SendAsync("ReceivedMessage", "System", $"{Context.ConnectionId} left {groupName}");
    }

    public async Task SendToGroup(string groupName, string user, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceivedMessage", user, message);
    }
}
