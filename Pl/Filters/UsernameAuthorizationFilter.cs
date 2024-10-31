using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UpDownCryptorollBackend.Filters;

public class UsernameAuthorizationFilter : IAuthorizationFilter
{
    private readonly string _parameterName;

    public UsernameAuthorizationFilter(string parameterName = "username")
    {
        _parameterName = parameterName;
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var tokenUsername = context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

        if (tokenUsername == null)
        {
            context.Result = new ForbidResult();
            return;
        }

        if (context.RouteData.Values.TryGetValue(_parameterName, out var usernameObj) ||
            context.HttpContext.Request.Query.TryGetValue(_parameterName, out var usernameValue))
        {
            var username = usernameObj?.ToString() ?? usernameValue.ToString();

            if (tokenUsername != username)
            {
                context.Result = new ForbidResult();
            }
        }
        else
        {
            context.Result = new BadRequestObjectResult($"Parameter '{_parameterName}' is required.");
        }
    }
}