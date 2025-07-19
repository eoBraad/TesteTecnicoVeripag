using Domain.Exceptions;
using Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filter;

public class AppExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is AppBaseException)
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnknowError(context);
        }
    }
    
    private void HandleProjectException(ExceptionContext context)
    {
        var appBaseException = (AppBaseException)context.Exception;
        var errorResponse = new ResponseErrorJson(appBaseException.GetErrors());

        context.HttpContext.Response.StatusCode = appBaseException.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }
    
    private void ThrowUnknowError(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorJson(["An unexpected error occurred. Please try again later."]);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}