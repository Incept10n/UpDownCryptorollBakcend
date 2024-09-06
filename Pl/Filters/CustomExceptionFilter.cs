using Bll.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UpDownCryptorollBackend.Filters;

public class CustomExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var statusCode = context.Exception switch
        {
            UserNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = "An error occured while processing the request",
            Detail = context.Exception.Message,
            Instance = context.HttpContext.Request.Path,
        };

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = statusCode,
        };

        context.ExceptionHandled = true;
    }
}