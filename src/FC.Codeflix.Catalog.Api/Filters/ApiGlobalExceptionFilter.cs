using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FC.Codeflix.Catalog.Api.Filters;

public class ApiGlobalExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _env;

    public ApiGlobalExceptionFilter(IHostEnvironment env)
    {
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        var details = new ProblemDetails();
        var exception = context.Exception;

        if (_env.IsDevelopment())
        {
            details.Extensions.Add("StackTrace", exception.StackTrace);
        }

        if (exception is EntityValidationException)
        {
            var ex = exception as EntityValidationException;
            details.Title = "One or more validation errors occurred.";
            details.Type = "UnprocessableEntity";
            details.Status = StatusCodes.Status422UnprocessableEntity;
            details.Detail = ex!.Message;
        }
        else if (exception is NotFoundException)
        {
            details.Title = "Not Found";
            details.Type = "NotFound";
            details.Status = StatusCodes.Status404NotFound;
            details.Detail = exception.Message;
        }
        else if (exception is UnauthorizedAccessException)
        {
            details.Title = "Unauthorized";
            details.Type = "Unauthorized";
            details.Status = StatusCodes.Status401Unauthorized;
            details.Detail = exception.Message;
        }
        else
        {
            details.Title = "An unexpected error occurred.";
            details.Type = "Unexpected";
            details.Status = StatusCodes.Status422UnprocessableEntity;
            details.Detail = exception.Message;
        }

        context.HttpContext.Response.StatusCode = details.Status!.Value;
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;
    }
}