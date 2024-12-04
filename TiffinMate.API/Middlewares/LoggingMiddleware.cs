using System.Diagnostics;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities;

namespace TiffinMate.API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        {
            // Record the start time
            var stopwatch = Stopwatch.StartNew();

            // Log Request
            var requestBody = await ReadRequestBody(context);
            var originalResponseBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context); // Process request

                // Log Response
                var responseBodyText = await ReadResponseBody(context);

                // Stop the stopwatch to get the time taken
                stopwatch.Stop();

                var log = new ApiLog
                {
                    HttpMethod = context.Request.Method,
                    RequestPath = context.Request.Path,
                    QueryString = context.Request.QueryString.ToString(),
                    RequestBody = requestBody,
                    ResponseStatusCode = context.Response.StatusCode,
                    ResponseBody = responseBodyText,
                    LoggedAt = DateTime.UtcNow,
                    TimeTaken = stopwatch.ElapsedMilliseconds 
                };

                // Save log to the database
                dbContext.ApiLogs.Add(log);
                await dbContext.SaveChangesAsync();

                await responseBody.CopyToAsync(originalResponseBodyStream);
            }
        }

        private async Task<string> ReadRequestBody(HttpContext context)
        {
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0; 
            return body;
        }

        private async Task<string> ReadResponseBody(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            return body;
        }
    }
}
