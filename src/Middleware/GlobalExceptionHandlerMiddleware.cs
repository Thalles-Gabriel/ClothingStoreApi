using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.API.Middleware;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{

    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error interno de servidor.");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            ProblemDetails problem = new()
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "Erro de servidor",
                Title = e.InnerException.ToString(),
                Detail = e.Message
            };

            await context.Response.WriteAsJsonAsync<ProblemDetails>(problem);
        }
    }
}
