using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace CustomAuthorize.Securities;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomAuthorize : Attribute, IAsyncActionFilter
{
    private readonly string _role;

    public CustomAuthorize(string role)
    {
        _role = role;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var accessor = context.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
        var roles = JsonConvert.DeserializeObject<List<string>>(accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role) ?? "[]") ?? new List<string>();
        
        if (!roles.Contains(_role))
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                Error = HttpStatusCode.Forbidden,
                Message = "Forbidden"
            });
            return;
        }
        await next();
    }
}