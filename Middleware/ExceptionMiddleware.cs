
using System.Net.Mime;
using System.Net;
using System.Text.Json;
using ASPNETCoreWithHeadersMiddleware.Exceptions;
using ASPNETCoreWithHeadersMiddleware.DTOs;

namespace ASPNETCoreWithHeadersMiddleware.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleErrorResponseAsync(context, ex);
            }
        }

        private async Task HandleErrorResponseAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            if (ex is ApiException apiException)
            {
                context.Response.StatusCode = apiException.Status;
                _logger.LogWarning(ex, ex.Message);
            }
            else
            {
                _logger.LogError(ex, ex.Message);
            }

            var errorResponse = new Error(ex.Message);

            var json = JsonSerializer.Serialize(errorResponse,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await context.Response.WriteAsync(json);
        }
    }
}
