using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.CustomException_handler
{

    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

       
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

      
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                _logger.LogError(error, $"An error occurred during request processing (Status Code: {context.Response.StatusCode})");

                switch (error)
                {
                    case ApplicationException e: 
                        _logger.LogError(e, "Custom application error occurred.");
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        _logger.LogError(e, "Key not found error occurred.");
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case ArgumentException e: 
                        _logger.LogError(e, "Invalid argument error occurred.");
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case InvalidOperationException e: 
                        _logger.LogError(e, "Invalid operation error occurred.");
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case HttpRequestException e: 
                        _logger.LogError(e, "HTTP request error occurred.");
                        response.StatusCode = (int)HttpStatusCode.BadGateway;
                        break;
                    case NotImplementedException e: 
                        _logger.LogError(e, "Not implemented error occurred.");
                        response.StatusCode = (int)HttpStatusCode.NotImplemented;
                        break;
                    case UnauthorizedAccessException e: 
                        _logger.LogError(e, "Unauthorized access error occurred.");
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case FormatException e: 
                        _logger.LogError(e, "Format error occurred.");
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case TimeoutException e: 
                        _logger.LogError(e, "Timeout error occurred.");
                        response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                        break;
                    case ValidationException e: 
                        _logger.LogError(e, "Validation error occurred.");
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        _logger.LogError(error, "An unexpected error occurred.");
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}