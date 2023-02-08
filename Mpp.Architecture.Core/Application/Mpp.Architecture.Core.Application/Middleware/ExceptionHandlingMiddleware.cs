namespace Mpp.Architecture.Core.Application.Middleware;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var problemDetails = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7807#section-3",
            Title = "Internal Server Error",
            Status = (int)HttpStatusCode.InternalServerError,
            Detail = ex.InnerException != null ? ex.InnerException.Message : ex.Message,
            Instance = context.Request.Path
        };
        problemDetails.Extensions.Add("traceId", System.Diagnostics.Activity.Current?.Id ?? context.TraceIdentifier);
        return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}

