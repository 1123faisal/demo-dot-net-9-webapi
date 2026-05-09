using System;

namespace MyFirstWebApi.Services;

public interface INotificationService
{
    Task NotifyProductCreated(string productName, double price);
    Task NotifyProductUpdated(string productName);
    Task NotifyProductDeleted(string productName);
}
