using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CelebRateApi.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            context.Response.StatusCode = exception switch
            {
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ArgumentNullException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status400BadRequest,
                DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
                DbUpdateException => StatusCodes.Status500InternalServerError,
                InvalidOperationException => StatusCodes.Status409Conflict,
                TimeoutException => StatusCodes.Status408RequestTimeout,
                TaskCanceledException => StatusCodes.Status408RequestTimeout,
                _ => StatusCodes.Status500InternalServerError,
            };

            context.Response.ContentType = "application/json";

            var response = new 
            { 
                message = "Something went wrong", 
                detail = exception.Message 
            };

            await context.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}
