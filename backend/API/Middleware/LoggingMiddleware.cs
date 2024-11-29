
namespace Patient.API.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(
            RequestDelegate next,
            ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            Action<string, object[]> logAction = _logger.LogInformation;
            var correlationId = Guid.NewGuid();
            _logger.LogInformation($"starting scope: {correlationId}");
            using (_logger.BeginScope(correlationId))
            {
                try
                {
                    logAction(
                        "Request from {ip} {method} {url}", new object[]
                         {
                             context.Connection?.RemoteIpAddress,
                             context.Request?.Method,
                             context.Request?.Path.Value
                         });
                    await _next(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Request Failed: " + ex.ToString());
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                }
                finally
                {
                    logAction(
                        "Response => {statusCode}", new object[]
                        {
                            context.Response?.StatusCode
                        });
                }
            }
        }
    }
}
