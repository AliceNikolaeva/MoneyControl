using System.Security.Claims;
using MoneyControl.Application;

namespace MoneyControl.Server;

public class UserContextMiddleware
{
    private readonly RequestDelegate _next;

    public UserContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            await _next.Invoke(context);
            return;
        }
        
        UserContext.SetUserContext(new Guid(userId));
        await _next.Invoke(context);
    }
}