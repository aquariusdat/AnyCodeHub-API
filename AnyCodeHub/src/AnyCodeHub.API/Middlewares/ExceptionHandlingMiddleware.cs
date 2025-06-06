﻿using AnyCodeHub.Domain.Exceptions;
using Newtonsoft.Json;
using static AnyCodeHub.Domain.Exceptions.IdentityException;

namespace AnyCodeHub.API.Middlewares;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        var response = new
        {
            title = GetTitle(exception),
            status = statusCode,
            detail = exception.Message,
            errors = GetErrors(exception),
            isSuccess = false,
            isFailure = true,
        };

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            ExistsException => StatusCodes.Status400BadRequest,
            RegisterException.UserNameExistsException => StatusCodes.Status400BadRequest,
            RegisterException.PhoneNumberExistsException => StatusCodes.Status400BadRequest,
            RegisterException.EmailExistsException => StatusCodes.Status400BadRequest,
            PostCategoryException.PostCategoryNotFoundException => StatusCodes.Status400BadRequest,
            PostException.PostNotFoundException => StatusCodes.Status400BadRequest,
            PostException.PostInsertFailedException => StatusCodes.Status400BadRequest,
            PostException.PostUpdateFailedException => StatusCodes.Status400BadRequest,
            AlreadyDeletedException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            //Application.Exceptions.ValidationException => StatusCodes.Status422UnprocessableEntity,
            FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
            FormatException => StatusCodes.Status422UnprocessableEntity,
            TokenException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitle(Exception exception) =>
        exception switch
        {
            DomainException applicationException => applicationException.Title,
            _ => "Server Error"
        };

    private static IReadOnlyCollection<Application.Exceptions.ValidationError> GetErrors(Exception exception)
    {
        IReadOnlyCollection<Application.Exceptions.ValidationError> errors = null;

        if (exception is Application.Exceptions.ValidationException validationException)
        {
            errors = validationException.Errors;
        }

        return errors;
    }

}
