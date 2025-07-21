using Domain.Exceptions;
using Domain.Messages;
using Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filter;

public class AppExceptionFilter(ILogger<AppExceptionFilter> logger) : IExceptionFilter
{
    private readonly ILogger<AppExceptionFilter> _logger = logger;
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
        
        var logMessage = new LogMessage(appBaseException.GetErrors(), context.HttpContext.Request.Path);
        _logger.LogError($"Ocorreu um erro: {logMessage}");

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