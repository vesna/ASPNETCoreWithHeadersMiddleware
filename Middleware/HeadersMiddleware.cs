using ASPNETCoreWithHeadersMiddleware.Configuration;
using Microsoft.Extensions.Options;

namespace ASPNETCoreWithHeadersMiddleware.Middleware
{
    public class HeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HeadersMiddlewareSettings _settings;

        public HeadersMiddleware(RequestDelegate next, IOptions<HeadersMiddlewareSettings> settings)
        {
            _next = next;
            _settings = settings.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Add headers for requests
            foreach (var header in _settings.Requests.Where(header => header.IsActive && !context.Request.Headers.ContainsKey(header.HeaderName)))
            {
                context.Request.Headers.Add(header.HeaderName, header.HeaderValue);
            }
            // Add headers for responses
            context.Response.OnStarting(() =>
            {
                foreach (var header in _settings.Response.Where(header => header.IsActive && !context.Response.Headers.ContainsKey(header.HeaderName)))
                {
                    context.Response.Headers.Add(header.HeaderName, header.HeaderValue);
                }
                return Task.CompletedTask;
            });
            await _next(context);
        }
    }
}
