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
            InvalidBetAmountException => StatusCodes.Status400BadRequest,
            MatchNotFoundException => StatusCodes.Status404NotFound,
            TaskOutOfRangeException => StatusCodes.Status400BadRequest,
            UnknownCoinTypeException => StatusCodes.Status400BadRequest,
            UserAlreadyExistsException => StatusCodes.Status409Conflict,
            UserAlreadyInMatchException => StatusCodes.Status400BadRequest,
            UserNotFoundException => StatusCodes.Status404NotFound,
            WrongPredictionTimeframeException => StatusCodes.Status400BadRequest,
            IncorrectUsernameOrPassword => StatusCodes.Status401Unauthorized,
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