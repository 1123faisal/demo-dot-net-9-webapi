using System;

namespace MyFirstWebApi.Services;

public interface IAuthService
{
    Task<string?> RegisterAsync(string username, string password, string role);
    Task<string?> LoginAsync(string username, string password);
}
