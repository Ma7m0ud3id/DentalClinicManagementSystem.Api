using FluentValidation;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DentalClinicManagementSystem.Apis.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse();

            switch (exception)
            {
                case KeyNotFoundException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.StatusCode = 404;
                    response.Message = ex.Message;
                    break;

                case UnauthorizedAccessException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.StatusCode = 401;
                    response.Message = ex.Message;
                    break;

                case ValidationException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusCode = 400;
                    response.Message = "Validation failed";
                    response.Errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                    break;

                case InvalidOperationException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusCode = 400;
                    response.Message = ex.Message;
                    break;

                case ArgumentException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusCode = 400;
                    response.Message = ex.Message;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.StatusCode = 500;
                    response.Message = "An unexpected error occurred";
                    break;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            return context.Response.WriteAsJsonAsync(response, options);
        }

        private class ErrorResponse
        {
            [JsonPropertyName("statusCode")]
            public int StatusCode { get; set; }

            [JsonPropertyName("message")]
            public string Message { get; set; } = string.Empty;

            [JsonPropertyName("errors")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public List<string>? Errors { get; set; }
        }
    }
}