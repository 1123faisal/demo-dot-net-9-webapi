using System;
using Microsoft.AspNetCore.SignalR;
using MyFirstWebApi.Hubs;

namespace MyFirstWebApi.Services;

public class NotificationService : INotificationService
{
    private const string _ProductNotifyKey = "ProductNotification";
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyProductCreated(string productName, double price)
    {
        await _hubContext.Clients.All.SendAsync(
            _ProductNotifyKey,
            new
            {
                Type = "Created",
                Message = $"New Product Added: {productName} at Rs.{price}",
                Time = DateTime.UtcNow,
            }
        );
    }

    public async Task NotifyProductDeleted(string productName)
    {
        await _hubContext.Clients.All.SendAsync(
            _ProductNotifyKey,
            new
            {
                Type = "Deleted",
                Message = $"Product Deleted: {productName}",
                Time = DateTime.UtcNow,
            }
        );
    }

    public async Task NotifyProductUpdated(string productName)
    {
        await _hubContext.Clients.All.SendAsync(
            _ProductNotifyKey,
            new
            {
                Type = "Updated",
                Message = $"Product Updated: {productName}",
                Time = DateTime.UtcNow,
            }
        );
    }
}
